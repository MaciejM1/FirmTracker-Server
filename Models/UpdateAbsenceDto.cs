namespace FirmTracker_Server.Models
{
    public class UpdateAbsenceDto
    {
        public string NewAbsenceType { get; set; } // e.g., "Sick", "Vacation", etc.
        public DateTime NewStartTime { get; set; }
        public DateTime NewEndTime { get; set; }
       
    }
}
