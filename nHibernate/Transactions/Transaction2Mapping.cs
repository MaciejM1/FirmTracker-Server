using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class Transaction2Mapping : ClassMap<Transaction2>
    {
        public Transaction2Mapping()
        {
            Table("Transactions");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Date);
            Map(x => x.EmployeeId);
            Map(x => x.PaymentType);
            Map(x => x.Discount);
            Map(x => x.Description);
            Map(x => x.TotalPrice);

            HasMany(x => x.TransactionProducts)
                .KeyColumn("TransactionId")
                .Cascade.AllDeleteOrphan()
                .Inverse(); // Ustawienie Inverse() wskazuje, że to `TransactionProduct` jest właścicielem relacji

        }
    }
}
