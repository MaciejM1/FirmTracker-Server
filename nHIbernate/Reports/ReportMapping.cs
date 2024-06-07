using FluentNHibernate.Mapping;
namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportMapping : ClassMap<Report>
    {
        public ReportMapping()
        {
            Table("Reports");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.FromDate);
            Map(x => x.ToDate);
            Map(x => x.TotalIncome);
            Map(x => x.TotalExpenses);
            Map(x => x.TotalBalance);

            HasManyToMany(x => x.Transactions)
                .Cascade.All()
                .Table("ReportTransactions")
                .ParentKeyColumn("ReportId")
                .ChildKeyColumn("TransactionId");

            HasManyToMany(x => x.Expenses)
                .Cascade.All()
                .Table("ReportExpenses")
                .ParentKeyColumn("ReportId")
                .ChildKeyColumn("ExpenseId");
        }

    }
}
