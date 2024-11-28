using FirmTracker_Server.Entities;
using System;

namespace YourNamespace.Models
{
    public class Workday
    {
        public virtual int Id { get; set; }
        public virtual DateTime? StartTime { get; set; }
        public virtual DateTime? EndTime { get; set; }
         public TimeSpan WorkedHours { get; set; }
        // Many-to-One relationship to the User entity
        public virtual User User { get; set; }
    }
}
