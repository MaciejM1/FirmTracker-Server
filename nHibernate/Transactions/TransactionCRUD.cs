﻿using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate.Reports;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionCRUD
    {
        public void AddTransaction(Transaction transaction)
        {
            using (var session = SessionFactory.OpenSession())
            using (var sessionTransaction = session.BeginTransaction())
            {
                try
                {
                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {
                        var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            transaction.TotalPrice += ((transactionProduct.Quantity * product.Price) * ((1 - (transaction.Discount / 100))));
                        }
                        else
                        {
                            transaction.TotalPrice += (product.Price) * ((1 - (transaction.Discount / 100)));
                        }
                        transactionProduct.TransactionId = transaction.Id;

                        session.Save(transactionProduct);
                    }
                    transaction.TotalPrice = Math.Round(transaction.TotalPrice, 2, MidpointRounding.AwayFromZero);
                    session.Save(transaction);

                    // Decrease product quantities based on transaction
                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {
                      
                        var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            product.Availability -= transactionProduct.Quantity;
                            session.Update(product);
                        }
                    }

                    sessionTransaction.Commit();
                }
                catch
                {
                    sessionTransaction.Rollback();
                    throw;
                }
            }
        }

        //usage of HQL 
        public Transaction2 GetTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery(@"
            SELECT t
            FROM Transaction2 t
            LEFT JOIN FETCH t.TransactionProducts tp
            LEFT JOIN FETCH tp.Product
            WHERE t.Id = :transactionId
        ");
                query.SetParameter("transactionId", transactionId);

                var transaction = query.UniqueResult<Transaction2>();
                return transaction;
            }
        }




        public void UpdateTransaction(Transaction transaction)
        {
            using (var session = SessionFactory.OpenSession())
            using (var sessionTransaction = session.BeginTransaction())
            {
                try
                {

                    var oldTransaction = session.Get<Transaction>(transaction.Id);
                    foreach (var tp in oldTransaction.TransactionProducts)
                    {
                        var product = session.Get<Product>(tp.ProductID);
                        
                        if(tp.Quantity < 0)
                        {
                            
                        }
                        if (product.Type != 0)
                        {
                            product.Availability += tp.Quantity;
                        }
                        session.Update(product);
                    }
                    session.Flush();
                    session.Clear();

                    transaction.TotalPrice = 0;

                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {
                        var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            transaction.TotalPrice += ((transactionProduct.Quantity * product.Price) * ((1 - (transaction.Discount / 100))));
                        }
                        else
                        {
                            transaction.TotalPrice += (product.Price) * ((1 - (transaction.Discount / 100)));
                        }

                        transactionProduct.TransactionId = transaction.Id;
                        session.SaveOrUpdate(transactionProduct);
                    }
                    transaction.TotalPrice = Math.Round(transaction.TotalPrice, 2, MidpointRounding.AwayFromZero);
                    session.Update(transaction);

                    // Decrease product quantities based on transaction
                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {

                        var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            if (transactionProduct.Quantity > product.Availability)
                            {
                                throw new InvalidOperationException($"Nie można dodać {product.Name} do transakcji. Dostępność: {product.Availability}, Zażądano: {transactionProduct.Quantity}");
                            }
                            else
                            {
                                product.Availability -= transactionProduct.Quantity;
                                session.Update(product);
                            }

                        }
                    }
                    sessionTransaction.Commit();
                }
                catch
                {
                    sessionTransaction.Rollback();
                    throw;
                }
            }
        }
        public void UpdateTransactionProduct(TransactionProduct transactionProduct)
        {
            using (var session = SessionFactory.OpenSession())
            using (var t = session.BeginTransaction())
            {
                try
                {
                    session.Update(transactionProduct);
                    t.Commit();
                }
                catch
                {
                    t.Rollback();
                    throw;
                }
            }
        }

        public void DeleteTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var t = session.BeginTransaction())
            {
                try
                {
                    var transaction = session.Get<Transaction>(transactionId);
                    if (transaction != null)
                    {
                        var criteria = session.CreateCriteria<ReportTransaction>();
                        criteria.Add(Restrictions.Eq("Transaction.Id", transactionId));
                        var reportTransactions = criteria.List<ReportTransaction>();

                        if (reportTransactions.Any())
                        {
                            throw new InvalidOperationException("Nie można usunąć transakcji. Transakcja jest ujęta w co najmniej jednym z raportów.");
                        }

                        foreach (var transactionProduct in transaction.TransactionProducts)
                        {
                            var product = session.Get<Product>(transactionProduct.ProductID);
                            if (product.Type != 0)
                            {
                                product.Availability += transactionProduct.Quantity;
                                session.Update(product);
                            }
                        }
                        session.Delete(transaction);
                        t.Commit();
                    }
                }
                catch
                {
                    t.Rollback();
                    throw;
                }
            }
        }
        public void AddTransactionProductToTransaction(int transactionId, TransactionProduct transactionProduct)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var transactionToUpdate = session.Get<Transaction>(transactionId);
                    if (transactionToUpdate != null)
                    {
                        var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            transactionToUpdate.TotalPrice += ((transactionProduct.Quantity * product.Price) * ((1 - (transactionToUpdate.Discount / 100))));
                        }
                        else
                        {
                            transactionToUpdate.TotalPrice += (product.Price) * ((1 - (transactionToUpdate.Discount / 100)));
                        }

                        transactionProduct.TransactionId= transactionToUpdate.Id;
                        session.Save(transactionProduct);

                        transactionToUpdate.TotalPrice = Math.Round(transactionToUpdate.TotalPrice, 2, MidpointRounding.AwayFromZero);
                        session.Update(transactionToUpdate);
                        //var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            product.Availability -= transactionProduct.Quantity;
                            session.Update(product);
                        }
                        transaction.Commit();
                    }
                    else
                    {
                        throw new Exception("Transaction not found.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public void DeleteTransactionProduct(int transactionId, int productId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var t = session.BeginTransaction())
            {
                try
                {
                    // Get the transaction to update
                    var transaction = session.Get<Transaction>(transactionId);
                    if (transaction == null)
                    {
                        throw new InvalidOperationException($"Transaction with ID {transactionId} not found.");
                    }

                    // Find the transaction product to remove
                    var transactionProduct = transaction.TransactionProducts.FirstOrDefault(tp => tp.ProductID == productId);
                    if (transactionProduct == null)
                    {
                        throw new InvalidOperationException($"Product with ID {productId} not found in the transaction.");
                    }

                    // Get the product to update availability
                    var product = session.Get<Product>(productId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product with ID {productId} not found.");
                    }

                    // Revert the product availability
                    if (product.Type != 0)
                    {
                        product.Availability += transactionProduct.Quantity;
                        session.Update(product);
                    }

                    // Remove the product from the transaction
                    transaction.TotalPrice = (transaction.TotalPrice * (1 + (transaction.Discount / 100))) - (transactionProduct.Quantity * product.Price );
                    transaction.TotalPrice = Math.Round(transaction.TotalPrice, 2, MidpointRounding.AwayFromZero);

                    // Remove the product from the Transaction's Product list
                    transaction.TransactionProducts.Remove(transactionProduct);

                    // Now delete the transaction product
                    session.Delete(transactionProduct);

                    // Update the transaction total price
                    session.Update(transaction);

                    t.Commit();
                }
                catch (Exception ex)
                {
                    t.Rollback();
                    throw new InvalidOperationException($"Error while deleting product from transaction: {ex.Message}");
                }
            }
        }


        public IList<Transaction2> GetAllTransactions()
        {
            using (var session = SessionFactory.OpenSession())
            {
                var transactions = session.Query<Transaction2>()
                    .FetchMany(t => t.TransactionProducts)
                    .ThenFetch(tp => tp.Product)
                    .ToList();

                return transactions;
            }
        }

    }
}
