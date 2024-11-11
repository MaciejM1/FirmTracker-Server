using System.Collections.Generic;
using System.Linq;
using FirmTracker_Server.nHibernate.Expenses;
using NHibernate;

namespace FirmTracker_Server.nHibernate
{
    public interface IExpenseRepository
    {
        List<Expense> GetAllExpenses();
        Expense GetExpense(int expenseId);
        void AddExpense(Expense expense);
        void UpdateExpense(Expense expense);
        void DeleteExpense(int expenseId);
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
