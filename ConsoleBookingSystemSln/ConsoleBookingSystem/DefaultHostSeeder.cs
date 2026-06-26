namespace ConsoleBookingSystem;

public class DefaultHostSeeder : IHostSeeder
{
    public List<Host> GetHosts()
    {
        // creating hardcoded in-memory Hosts
        var host1 = new Host { Id = 14, Name = "Hotel \'Minnesota\'", Address = "Secondary Street, 53" };
        var host2 = new Host { Id = 5, Name = "Hotel \'California\'", Address = "Main Street, 1" };
        var host3 = new Host { Id = 8, Name = "Hotel \'Minnesota\'", Address = "Main Street, 5" };


        // adding Apartments to Hosts with creating Apartments "on the fly"
        // (leaving host3 without added Apartments intentionally)
        host1.Apartments.AddRange(new List<Apartment>
        {
            new Apartment { Id = 1, Number = 21,  Price = 150.1 },
            new Apartment { Id = 2, Number = 12, Price = 110.4, IsBooked = true},
            new Apartment { Id = 3, Number = 45, Price = 20.7 },
            new Apartment { Id = 4, Number = 100, Price = 560 },
        });

        host2.Apartments.AddRange(new List<Apartment>
        {
            new Apartment { Id = 5,  Number = 14,  Price = 340.4 },
            new Apartment { Id = 6, Number = 11, Price = 10, IsBooked = true },
            new Apartment { Id = 7, Number = 65, Price = 240.1 }
        });
        
        return [host1, host2, host3];
    }
}