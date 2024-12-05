using FirmTracker_Server.Entities;

namespace FirmTracker_Server.nHibernate
{
    public class Absence
    {
        public virtual int Id { get; set; }
        public virtual string AbsenceName { get; set; }
    }
}
