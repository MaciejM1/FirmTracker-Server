using FirmTracker_Server.nHibernate.Products;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

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
                        transactionProduct.TransactionId = transaction.Id; 
                        session.Save(transactionProduct);
                    }
                    session.Save(transaction);

                    // Decrease product quantities based on transaction
                    foreach (var transactionProduct in transaction.TransactionProducts)
                    {
                        var product = session.Get<Product>(transactionProduct.Product.Id);
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
        public Transaction GetTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var query = session.CreateQuery(@"
            SELECT t 
            FROM Transaction t 
            LEFT JOIN FETCH t.TransactionProducts tp 
            LEFT JOIN FETCH tp.Product 
            WHERE t.Id = :transactionId
        ");
                query.SetParameter("transactionId", transactionId);
                var transaction = query.UniqueResult<Transaction>();
                return transaction;
            }
        }


        public void UpdateTransaction(Transaction transaction)
        {
            using (var session = SessionFactory.OpenSession())
            using (var t = session.BeginTransaction())
            {
                try
                {
                    session.Update(transaction);
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
                        transactionProduct.TransactionId = transactionToUpdate.Id;
                        session.Save(transactionProduct);
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
                    throw ex;
                }
            }
        }


        public IList<Transaction> GetAllTransactions()
        {
            using (var session = SessionFactory.OpenSession())
            {
                var transactions = session.Query<Transaction>()
                    .FetchMany(t => t.TransactionProducts) 
                    .ThenFetch(tp => tp.Product) 
                    .ToList();

                return transactions;
            }
        }

    }
}
