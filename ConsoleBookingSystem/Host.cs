namespace ConsoleBookingSystem;

public class Host
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Address { get; set; }
    
    public List<Apartment> Apartments { get; set; } = [];


    public override string ToString()
    {
        return $"ID={Id} --> {Name}, {Address} ({Apartments.Count} apartments)";
    }
}