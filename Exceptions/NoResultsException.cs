namespace FirmTracker_Server.Exceptions
{
    public class NoResultsException : Exception
    {
        public NoResultsException() : base("Brak wyników") { }

        public NoResultsException(string message) : base(message) { }

        public NoResultsException(string message, Exception innerException) : base(message, innerException) { }
    }
}