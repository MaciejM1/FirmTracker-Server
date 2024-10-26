namespace FirmTracker_Server.Entities
{
    public class User
    {
        public virtual int Id { get; set; }
        public virtual string Login { get; set; }
        public virtual string Email { get; set; }
        public virtual string Role { get; set; } = "User";
        public virtual string PassHash { get; set; }
        public virtual bool NewEncryption { get; set; }
    }
}
