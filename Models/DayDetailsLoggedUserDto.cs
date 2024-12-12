using FirmTracker_Server.nHibernate;

namespace FirmTracker_Server.Models
{
    public class DayDetailsLoggedUserDto
    {
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string TotalWorkedHours { get; set; }
        public List<Workday> WorkdayDetails { get; set; }
    }
}
