using FirmTracker_Server.nHibernate.Transactions;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportTransaction
    {
        public virtual int Id { get; set; }
        public virtual int ReportId { get; set; }
        public virtual int TransactionId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var other = (ReportTransaction)obj;
            return ReportId == other.ReportId && TransactionId == other.TransactionId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ReportId, TransactionId);
        }
    }

}
