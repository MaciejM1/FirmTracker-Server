using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportTransactionMapping : ClassMap<ReportTransaction>
    {
        public ReportTransactionMapping() 
        {
            Table("ReportTransactions");
            Id(x => x.Id);
            References(x => x.ReportId, "ReportId");
            References(x => x.TransactionId, "TransactionId");
        }
    }
}
