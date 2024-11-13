using FirmTracker_Server.Entities;

namespace FirmTracker_Server.nHibernate
{
    public class Workday
    {
        public virtual int Id { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }  // Nullable EndTime, if not finished
        public virtual User User { get; set; } // Assuming a relationship to a User entity
    }
}
