using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Reports
{
    public class ReportExpenseMapping : ClassMap<ReportExpense>
    {
        public ReportExpenseMapping()
        {
            Table("ReportExpenses");
            CompositeId()
                .KeyReference(x => x.Report, "ReportId")
                .KeyReference(x => x.Expense, "ExpenseId");
        }
    }
}
