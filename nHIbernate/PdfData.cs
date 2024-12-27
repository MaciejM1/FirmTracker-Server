using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Products;
using FirmTracker_Server.nHibernate.Transactions;
using NHibernate;
using Transaction = FirmTracker_Server.nHibernate.Transactions.Transaction;

namespace FirmTracker_Server.nHibernate
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
    }

    public interface IExpenseRepository
    {
        List<Expense> GetAllExpenses();
        Expense GetExpense(int expenseId);
        void AddExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void DeleteExpense(int expenseId);
    }
    public interface ITransactionRepository
    {
        List<Transaction> GetAllTransactions();
        Transaction GetTransaction(int transactionId);
        List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate);
        List<TransactionProduct> GetTransactionProducts(int transactionId);
        void AddTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(int transactionId);
        List<TransactionProduct> GetTransactionProductsForTransactions(List<int> transactionIds);
    }
    public class ProductRepository : IProductRepository
    {
        public Product GetProduct(int id)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<Product>(id);
            }
        }
    }

    public class TransactionRepository : ITransactionRepository
    {
        // Retrieve all transactions
        public List<Transaction> GetAllTransactions()
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<Transaction>().ToList();
            }
        }
        public List<TransactionProduct> GetTransactionProductsForTransactions(List<int> transactionIds)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<TransactionProduct>()
                    .Where(tp => transactionIds.Contains(tp.TransactionId))
                    .ToList();
            }
        }

        public Transaction GetTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<Transaction>(transactionId);
            }
        }

 
        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<Transaction>()
                    .Where(t => t.Date >= startDate && t.Date <= endDate)
                    .ToList();
            }
        }


        public List<TransactionProduct> GetTransactionProducts(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<TransactionProduct>()
                    .Where(tp => tp.TransactionId == transactionId)
                    .ToList();
            }
        }


        public void AddTransaction(Transaction transaction)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transactionScope = session.BeginTransaction())
            {
                try
                {
                    session.Save(transaction);
                    transactionScope.Commit();
                }
                catch
                {
                    transactionScope.Rollback();
                    throw;
                }
            }
        }

        // Update an existing transaction
        public void UpdateTransaction(Transaction transaction)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transactionScope = session.BeginTransaction())
            {
                try
                {
                    session.Update(transaction);
                    transactionScope.Commit();
                }
                catch
                {
                    transactionScope.Rollback();
                    throw;
                }
            }
        }

 
        public void DeleteTransaction(int transactionId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transactionScope = session.BeginTransaction())
            {
                try
                {
                    var transaction = session.Get<Transaction>(transactionId);
                    if (transaction != null)
                    {
                        session.Delete(transaction);
                    }
                    transactionScope.Commit();
                }
                catch
                {
                    transactionScope.Rollback();
                    throw;
                }
            }
        }
    }
    public class ExpenseRepository : IExpenseRepository
    {
        // Retrieve all expenses
        public List<Expense> GetAllExpenses()
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<Expense>().ToList();
            }
        }

        // Retrieve a specific expense by ID
        public Expense GetExpense(int expenseId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Get<Expense>(expenseId);
            }
        }

        // Add a new expense
        public void AddExpense(Expense expense)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Save(expense);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Update an existing expense
        public void UpdateExpense(Expense expense)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    session.Update(expense);
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // Delete an expense by ID
        public void DeleteExpense(int expenseId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    var expense = session.Get<Expense>(expenseId);
                    if (expense != null)
                    {
                        session.Delete(expense);
                    }
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
