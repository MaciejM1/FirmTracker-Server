using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Expenses
{
    public class ExpenseMapping : ClassMap<Expense>
    {
        public ExpenseMapping()
        {
            Table("Expenses");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.Value);
            Map(x => x.Description);

        }
    }
}
