namespace ConsoleBookingSystem;

public class ApartmentNotFoundException : Exception
{
    public int? ApartmentId { get; private set; }
    
    public ApartmentNotFoundException(string message, int? apartmentId = null)
        : base(message)
    {
        ApartmentId = apartmentId;
    }
}