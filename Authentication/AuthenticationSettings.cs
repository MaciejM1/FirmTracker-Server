namespace FirmTracker_Server.Authentication
{
    public class AuthenticationSettings
    {
        public string JwtSecKey { get; set; }
        public int JwtExpireDays { get; set; }
        public string JwtIssuer { get; set; }
    }
}
