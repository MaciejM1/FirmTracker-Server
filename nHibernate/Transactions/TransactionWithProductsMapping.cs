using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionWithProductsMapping : ClassMap<TransactionWithProducts>
    {
        public TransactionWithProductsMapping()
        {
            Table("TransactionProducts");
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.TransactionId).Column("TransactionId").Not.Nullable();
            References(x => x.Product).Column("ProductId").Not.Nullable();

            Map(x => x.Quantity);

        }
    }
}
