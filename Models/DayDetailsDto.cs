using FirmTracker_Server.nHibernate;

namespace FirmTracker_Server.Models
{
    public class DayDetailsDto
    {
        public required string Email { get; set; }
        public DateTime Date { get; set; }
        public required string TotalWorkedHours { get; set; }
        public required List<Workday> WorkdayDetails { get; set; }
    }
}
