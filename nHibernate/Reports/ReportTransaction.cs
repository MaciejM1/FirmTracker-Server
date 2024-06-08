using FirmTracker_Server.nHibernate.Transactions;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportTransaction
    {
        //public virtual int Id { get; set; }
        public virtual Report Report { get; set; }
        public virtual Transaction Transaction { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (ReportTransaction)obj;
            return Report != null && Transaction != null &&
                   Report.Id == other.Report.Id &&
                   Transaction.Id == other.Transaction.Id;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + (Report?.Id.GetHashCode() ?? 0);
                hash = hash * 23 + (Transaction?.Id.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

}
