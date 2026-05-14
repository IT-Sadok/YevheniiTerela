using System.Globalization;

namespace ConsoleBookingSystem;

public class Apartment
{
    public int Id { get; set; }
    public int Number { get; set; }
    public double Price { get; set; }
    public bool IsBooked { get; set; } = false;

    public override string ToString()
    {
        return $"Apartment #{Number}, ${Price.ToString("F2", CultureInfo.InvariantCulture)} {(IsBooked ? "" : "(available for reservation)")}";
    }
}