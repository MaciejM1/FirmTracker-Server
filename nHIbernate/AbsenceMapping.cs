using FirmTracker_Server.nHibernate;
using FluentNHibernate.Mapping;
namespace FirmTracker_Server.nHIbernate
{
    public class AbsenceMapping : ClassMap<Absence>
    {
        public AbsenceMapping()
        {
            Table("Absences"); 
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.AbsenceName);
        }
    }
}
