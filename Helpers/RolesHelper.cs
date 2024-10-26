namespace FirmTracker_Server
{
    public static class RolesHelper
    {
        public static IEnumerable<string> GetRoles() => new List<string> { Roles.Admin, Roles.User };
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}