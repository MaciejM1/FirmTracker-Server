using FirmTracker_Server.nHibernate.Reports;

namespace FirmTracker_Server.nHibernate.Expenses
{
    public class Expense
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual decimal Value { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Report> Reports { get; set; } = new List<Report>();
    }
}
