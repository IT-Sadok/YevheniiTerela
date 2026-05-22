namespace ConsoleBookingSystem;

public class HostNotFoundException : Exception
{
    public int? HostId { get; private set; }
    
    public HostNotFoundException()
    {
        
    }

    public HostNotFoundException(string message)
        : base(message)
    {
        
    }
    
    public HostNotFoundException(string message, int hostId)
        : base(message)
    {
        HostId = hostId;
    }

    public HostNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
        
    }
}