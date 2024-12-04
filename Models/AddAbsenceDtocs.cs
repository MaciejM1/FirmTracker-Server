namespace FirmTracker_Server.Models
{
    public class AddAbsenceDto
    {
        public string userEmail { get; set; }
        public string AbsenceType { get; set; } // e.g., "Sick", "Vacation", etc.
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
      
    }

}
