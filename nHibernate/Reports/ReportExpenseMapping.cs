using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportExpenseMapping : ClassMap<ReportExpense>
    {
        public ReportExpenseMapping()
        {
            Table("ReportExpenses");
            Id(x => x.Id);
            References(x => x.ReportId, "ReportId");
            References(x => x.ExpenseId, "ExpenseId");
        }
    }
}
