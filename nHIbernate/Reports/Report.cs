using FirmTracker_Server.nHibernate.Expenses;
using FirmTracker_Server.nHibernate.Transactions;
using Newtonsoft.Json;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class Report
    {
        public virtual int Id { get; set; }
        public virtual DateTime FromDate { get; set; }
        public virtual DateTime ToDate { get; set; }
        public virtual decimal TotalIncome { get; set; }
        public virtual decimal TotalExpenses { get; set; }
        public virtual decimal TotalBalance { get; set; }

        public virtual IList<ReportTransaction> ReportTransactions { get; set; } = new List<ReportTransaction>();
        public virtual IList<ReportExpense> ReportExpenses { get; set; } = new List<ReportExpense>();

       


        public class DateRangeDto
        {
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }

        }   
    }
}
