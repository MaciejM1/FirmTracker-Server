using FluentNHibernate.Mapping;
namespace FirmTracker_Server.nHibernate
{
    public class WorkdayMapping : ClassMap<Workday>
    {
        public WorkdayMapping()
        {
            Table("Workdays"); // Make sure the table name matches the one in the database
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.StartTime);
            Map(x => x.EndTime);
            References(x => x.User).Column("UserId"); // Assuming Workday is related to a User
            Map(x => x.Absence);
        }
    }
}
