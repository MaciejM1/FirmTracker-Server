using FirmTracker_Server.Entities;

namespace FirmTracker_Server.nHibernate
{
    public class Workday
    {
        public virtual int Id { get; set; }
        public virtual DateTime StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }  // Nullable EndTime, if not finished
        public virtual TimeSpan WorkedHours
        {
            get
            {
                // Calculate the worked hours, using 5 PM as the fallback for the EndTime
                return (EndTime ?? DateTime.Today.AddHours(24)) - StartTime;
            }
            set
            {
               
            }
        }
        public virtual User User { get; set; } 
    }
}
