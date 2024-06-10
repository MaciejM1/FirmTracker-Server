﻿using FluentNHibernate.Mapping;
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

            /*HasMany(x => x.ReportTransactions)
            .KeyColumn("ReportId")
                .Cascade.AllDeleteOrphan()
                .Inverse();

            HasMany(x => x.ReportExpenses)
            .KeyColumn("ReportId")
                .Cascade.AllDeleteOrphan()
                .Inverse();*/

        }

    }
}