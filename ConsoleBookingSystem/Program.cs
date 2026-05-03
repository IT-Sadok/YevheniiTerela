using ConsoleBookingSystem;

Console.WriteLine("\n============= Helo, this is Booking System app! =============\n");

// creating Hosts
var host1 = new Host(1, "Hotel California", "Main Street, 1");
var host2 = new Host(2, "Hotel Minnesota", "Main Street, 5");
var host3 = new Host(3, "Hotel Minnesota", "Secondary Street, 53");


// adding Apartments to Hosts with creating Apartments "on the fly"
host1.Apartments.AddRange(new List<Apartment>
{
    new Apartment(1, 21, 150.1m),
    new Apartment(2, 12, 110.1m),
    new Apartment(3, 45, 20.1m),
    new Apartment(4, 100, 560.1m),
});


host2.Apartments.AddRange(new List<Apartment>
{
    new Apartment(5, 14, 340.1m),
    new Apartment(6, 11, 10.1m),
    new Apartment(7, 65, 240.1m)
});


host3.Apartments.AddRange(new List<Apartment>
{
    new Apartment(8, 201, 450.1m),
    new Apartment(9, 118, 117.1m),
});