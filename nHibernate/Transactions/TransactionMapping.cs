﻿using FluentNHibernate.Mapping;

namespace FirmTracker_Server.nHibernate.Transactions
{
    public class TransactionMapping : ClassMap<Transaction>
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

            HasMany(x => x.TransactionProducts)
                .KeyColumn("TransactionId")
                .Cascade.AllDeleteOrphan()
                .Inverse(); // Ustawienie Inverse() wskazuje, że to `TransactionProduct` jest właścicielem relacji

            HasManyToMany(x => x.Reports)
            .Cascade.All()
            .Table("ReportTransactions")
            .ParentKeyColumn("TransactionId")
            .ChildKeyColumn("ReportId");
        }
    }
}
