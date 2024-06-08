using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportTransactionMapping : ClassMap<ReportTransaction>
    {
        public ReportTransactionMapping()
        {
            Table("ReportTransactions");
            CompositeId()
                .KeyReference(x => x.Report, "ReportId")
                .KeyReference(x => x.Transaction, "TransactionId");
        }
    }
}
