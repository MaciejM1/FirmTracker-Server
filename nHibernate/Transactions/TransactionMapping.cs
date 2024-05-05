using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionMapping:ClassMap<Transaction>
    {
        public TransactionMapping()
        {
            Table("Transactions");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.EmployeeId);
            Map(x => x.PaymentType);
            Map(x => x.Discount);
            Map(x => x.Description);

            HasManyToMany(x => x.Products)
                .Table("TransactionProducts") 
                .ParentKeyColumn("TransactionId")
                .ChildKeyColumn("ProductId")
                .Cascade.All();
        }
    }
}
