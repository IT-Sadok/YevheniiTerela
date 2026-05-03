namespace ConsoleBookingSystem;

public class Host
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public List<Apartment> Apartments { get; set; } = [];
    

    public Host(int id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }


    public override string ToString()
    {
        return $"ID={Id} --> {Name}, {Address} ({Apartments.Count} apartments)";
    }
}