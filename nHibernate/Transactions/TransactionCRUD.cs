using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Products;
using NHibernate;
using System.Collections.Generic;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using FirmTracker_Server.nHibernate.Transactions;
using NHibernate.Linq;

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
                    session.Save(transaction);
                    sessionTransaction.Commit();
                }
                catch
                {
                    sessionTransaction.Rollback();
                    throw;
                }
            }
        }

        public Transaction GetTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var transaction = session.Query<Transaction>()
                    .Fetch(t => t.Products)
                    .FirstOrDefault(t => t.Id == transactionId);
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
                    var transaction = session.Get<Product>(transactionId);
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

        public IList<Transaction> GetAllTransactions()
        {
            using (var session = SessionFactory.OpenSession())
            {
                var transactions = session.Query<Transaction>()
                    .FetchMany(t => t.Products)
                    .ToList();
                return transactions;
            }
        }
    }

}
