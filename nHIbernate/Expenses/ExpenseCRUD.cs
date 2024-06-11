/*
 * This file is part of FirmTracker - Server.
 *
 * FirmTracker - Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * FirmTracker - Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with FirmTracker - Server. If not, see <https://www.gnu.org/licenses/>.
 */

using FirmTracker_Server.nHibernate;
using FirmTracker_Server.nHibernate.Products;

namespace FirmTracker_Server.nHibernate.Expenses
{
    public class ExpenseCRUD
    {
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
        public Expense GetExpense(int expenseId) {
            using (var session = SessionFactory.OpenSession())
            {
                /*var query = session.CreateQuery(@"
                SELECT e
                FROM Expense e
                WHERE e.Id = expenseId
                ");
                query.SetParameter("expenseId", expenseId);
                var expense = query.UniqueResult<Expense>();*/
                return session.Get<Expense>(expenseId);
            }
        }
        public DateTime GetExpenseDate(int expenseId)
        {
            using(var session = SessionFactory.OpenSession())
            {
                var expense = session.Query<Expense>()
                    .Where(e => e.Id == expenseId)
                    .Select(e => e.Date)
                    .FirstOrDefault();
                return expense;
            }
        }
        public decimal GetExpenseValue(int expenseId)
        {
            using (var session = SessionFactory.OpenSession())
            {
                var expense = session.Query<Expense>()
                    .Where(e => e.Id == expenseId)
                    .Select(e => e.Value)
                    .FirstOrDefault();
                return expense;
            }
        }

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
        
        public void DeleteExpense(int expenseId)
        {
            using (var session = SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            try
            {
                var expense = session.Get<Expense>(expenseId);
                if (expense != null)
                    {
                        session.Delete(expense);
                        transaction.Commit();
                    }

            }
            catch {
                    transaction.Rollback();
                    throw;
                }
        }

        public IList<Expense> GetAllExpenses()
        {
            using (var session = SessionFactory.OpenSession())
            {
                return session.Query<Expense>().ToList();
            }
        }
    }
}
