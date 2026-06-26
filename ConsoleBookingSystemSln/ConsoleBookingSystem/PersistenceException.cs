namespace ConsoleBookingSystem;

public class PersistenceException : Exception
{
    public PersistenceException(string message, Exception innerException)
        : base(message, innerException)
    {
        
    }
}