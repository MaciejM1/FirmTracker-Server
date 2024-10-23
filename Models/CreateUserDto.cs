namespace FirmTracker_Server.Models
{
    public class CreateUserDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public bool NewEncryption { get; set; } = true;
    }
}
