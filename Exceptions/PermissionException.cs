namespace FirmTracker_Server.Exceptions
{
    public class PermissionException : Exception
    {
        public PermissionException() : base("Brak uprawnień") { }

        public PermissionException(string message) : base(message) { }

        public PermissionException(string message, Exception innerException) : base(message, innerException) { }
    }
}