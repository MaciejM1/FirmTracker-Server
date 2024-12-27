namespace FirmTracker_Server.Models
{
    public class CreateUserDto
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
        public bool NewEncryption { get; set; } = true;
    }
}
