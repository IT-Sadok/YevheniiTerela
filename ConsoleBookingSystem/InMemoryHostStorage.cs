namespace ConsoleBookingSystem;

public class InMemoryHostStorage : IHostStorage
{
    private List<Host> _hosts;
    

    public InMemoryHostStorage()
    {
        // creating hardcoded in-memory Hosts
        var host1 = new Host { Id = 5, Name = "Hotel \'California\'", Address = "Main Street, 1" };
        var host2 = new Host { Id = 8, Name = "Hotel \'Minnesota\'", Address = "Main Street, 5" };
        var host3 = new Host { Id = 14, Name = "Hotel \'Minnesota\'", Address = "Secondary Street, 53" };


        // adding Apartments to Hosts with creating Apartments "on the fly"
        // (leaving host3 without added Apartments intentionally)
        host1.Apartments.AddRange(new List<Apartment>
        {
            new Apartment{ Id = 1, Number = 21,  Price = 150.1 },
            new Apartment{ Id = 2, Number = 12, Price = 110.4, IsBooked = true},
            new Apartment{ Id = 3, Number = 45, Price = 20.7 },
            new Apartment{ Id = 4, Number = 100, Price = 560 },
        });

        host2.Apartments.AddRange(new List<Apartment>
        {
            new Apartment{ Id = 5,  Number = 14,  Price = 340.4 },
            new Apartment{ Id = 6, Number = 11, Price = 10, IsBooked = true },
            new Apartment { Id = 7, Number = 65, Price = 240.1 }
        });
        
        _hosts = new List<Host>{ host1, host2, host3 };
    }
    
    
    public List<Host> FindAllHosts()
    {
        return _hosts;
    }
    

    public Host? FindHostById(int hostId)
    {
        return _hosts.FirstOrDefault(host => host.Id == hostId);
    }


    public Host? FindHostByNameAndAddress(string hostName, string address)
    {
        return _hosts.FirstOrDefault(host => host.Name.ToLower() == hostName.ToLower() && host.Address.ToLower() == address.ToLower());
    }


    public Host? FindLastAddedHost()
    {
        return _hosts.LastOrDefault();
    }
    

    public void SaveHost(Host host)
    {
        _hosts.Add(host);
    }


    public int FindHostsCount()
    {
        return _hosts.Count;
    }


    public void RemoveHost(Host host)
    {
        _hosts.Remove(host);
    }


    public void UpdateHost(int hostId, string hostName, string address)
    {
        var hostToUpdate = _hosts.FirstOrDefault(h => h.Id == hostId);
        
        if (hostToUpdate == null) 
            throw new Exception($"Update failed. Host with ID = {hostId} not found.");
        
        hostToUpdate.Name = hostName;
        hostToUpdate.Address = address;
    }
}