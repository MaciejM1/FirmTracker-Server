using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Transactions;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class Report
    {
        public virtual int Id { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual IList<Transaction> Transactions { get; set;} = new List<Transaction>();
        public virtual IList<Expense> Expenses { get; set; } = new List<Expense>(); 
        public virtual decimal TotalIncome { get; set; }
        public virtual decimal TotalExpenses { get; set; }
        public virtual decimal TotalBalance { get; set; }
        public Report() { 
            Transactions = new List<Transaction>();
            Expenses = new List<Expense>();
        }


    }
}
