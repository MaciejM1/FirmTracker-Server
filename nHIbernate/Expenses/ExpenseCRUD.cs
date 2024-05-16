using FirmTracker_Server.nHibernate;

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
    }
}
