namespace FirmTracker_Server.Models
{
    public class ResetPasswordDto
    {
        public string UserMail { get; set; }  
        public string NewPassword { get; set; }  
    }
}
