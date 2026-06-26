namespace ConsoleBookingSystem;

public class HostNotFoundException : Exception
{
    public int? HostId { get; private set; }
    
    public HostNotFoundException(string message, int? hostId = null)
        : base(message)
    {
        HostId = hostId;
    }
}