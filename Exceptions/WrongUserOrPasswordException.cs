namespace FirmTracker_Server.Exceptions
{
    public class WrongUserOrPasswordException : Exception
    {
        public WrongUserOrPasswordException() : base("Nieprawidłowy użytkownik lub hasło.") { }

        public WrongUserOrPasswordException(string message) : base(message) { }

        public WrongUserOrPasswordException(string message, Exception innerException) : base(message, innerException) { }
    }
}
