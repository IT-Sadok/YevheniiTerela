namespace ConsoleBookingSystem;

public class RaceConditionSimulatorHostSeeder : IHostSeeder
{
    public List<Host> GetHosts()
    {
        var host1 = new Host { Id = 14, Name = "Hotel \'Minnesota\'", Address = "Secondary Street, 53" };
        var host2 = new Host { Id = 5, Name = "Hotel \'California\'", Address = "Main Street, 1" };
        var host3 = new Host { Id = 8, Name = "Hotel \'Minnesota\'", Address = "Main Street, 12" };
        var host4 = new Host { Id = 12, Name = "Hotel \'New York\'", Address = "Main Street, 16" };
        var host5 = new Host { Id = 101, Name = "Hotel \'Manchester\'", Address = "Main Street, 44" };

        var sharedApartment = new Apartment { Id = 4, Number = 100, Price = 560 };
        
        host1.Apartments.AddRange(new List<Apartment>
        {
            new Apartment { Id = 1, Number = 21,  Price = 150.1 },
            new Apartment { Id = 2, Number = 12, Price = 110.4, IsBooked = true},
            new Apartment { Id = 3, Number = 45, Price = 20.7 },
            sharedApartment
        });

        host2.Apartments.AddRange(new List<Apartment>
        {
            new Apartment { Id = 5,  Number = 14,  Price = 340.4 },
            new Apartment { Id = 6, Number = 11, Price = 10, IsBooked = true },
            new Apartment { Id = 7, Number = 65, Price = 240.1 },
            sharedApartment
        });

        host3.Apartments.Add(sharedApartment);
        host4.Apartments.Add(sharedApartment);
        host5.Apartments.Add(sharedApartment);
        
        return [host1, host2, host3, host4, host5];
    }
}