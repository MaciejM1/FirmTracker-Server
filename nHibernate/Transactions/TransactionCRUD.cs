using FirmTracker_Server.nHibernate.Products;
using NHibernate;
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
                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {
                        /*var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            transaction.TotalPrice += ((transactionProduct.Quantity * product.Price) * ((1 - (transaction.Discount / 100))));
                        }
                        else
                        {
                            transaction.TotalPrice += (product.Price) * ((1 - (transaction.Discount / 100)));
                        }*/

                        transactionProduct.TransactionId = transaction.Id;
                        session.Update(transactionProduct);
                    }
                    session.Update(transaction);

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
                        transaction.Commit();

                        session.Update(transactionToUpdate);
                        //var product = session.Get<Product>(transactionProduct.ProductID);
                        if (product.Type != 0)
                        {
                            product.Availability -= transactionProduct.Quantity;
                            session.Update(product);
                        }
                        
                    }
                    else
                    {
                        throw new Exception("Transaction not found.");
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
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
