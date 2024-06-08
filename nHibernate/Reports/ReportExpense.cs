using FirmTracker_Server.nHibernate.Expenses;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportExpense
    {
        public virtual int Id { get; set; }
        public virtual int ReportId { get; set; }
        public virtual int ExpenseId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ReportExpense)obj;
            return ReportId == other.ReportId && ExpenseId == other.ExpenseId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReportId, ExpenseId);
        }
    }

}
