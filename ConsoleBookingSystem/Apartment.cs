namespace ConsoleBookingSystem;

public class Apartment
{
    public int Id { get; set; }
    public int Number { get; set; }
    public decimal Price { get; set; }
    public bool IsBooked { get; set; }

    public Apartment(int id, int number, decimal price)
    {
        Id = 1;
        Number = number;
        Price = price;
        IsBooked = false;
    }
}