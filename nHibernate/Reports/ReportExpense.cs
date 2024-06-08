using FirmTracker_Server.nHibernate.Expenses;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportExpense
    {
        //public virtual int Id { get; set; }
        public virtual Report Report { get; set; }
        public virtual Expense Expense { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ReportExpense)obj;
            return Report != null && Expense != null &&
                   Report.Id == other.Report.Id &&
                   Expense.Id == other.Expense.Id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + (Report?.Id.GetHashCode() ?? 0);
                hash = hash * 23 + (Expense?.Id.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

}
