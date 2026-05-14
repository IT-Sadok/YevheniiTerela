namespace ConsoleBookingSystem;

public class InMemoryHostStorage : IHostStorage
{
    private List<Host> _hosts;
    
    private int _currentLargestHostId; // is used internally to generate id for newly-added Hosts

    public InMemoryHostStorage()
    {
        // creating hardcoded in-memory Hosts
        var host1 = new Host { Id = 14, Name = "Hotel \'Minnesota\'", Address = "Secondary Street, 53" };
        var host2 = new Host { Id = 5, Name = "Hotel \'California\'", Address = "Main Street, 1" };
        var host3 = new Host { Id = 8, Name = "Hotel \'Minnesota\'", Address = "Main Street, 5" };


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

        //storing largest host-ID to _currentLargestHostId
        foreach (var host in _hosts)
        {
            if (_currentLargestHostId < host.Id) _currentLargestHostId = host.Id;
        }
    }


    private static int CompareHostsById(Host host1, Host host2)
    {
        return host1.Id.CompareTo(host2.Id);
    }
    
    
    public List<Host> FindAllHosts()
    {
        return _hosts;
    }
    

    public Host? FindHostById(int hostId)
    {
        return _hosts.FirstOrDefault(host => host.Id == hostId);
    }


    public Host? FindHostByNameAndAddress(string hostName, string address, List<int>? ignoredHostIds = null)
    {
        List<int> ignoredIds = ignoredHostIds != null ? ignoredHostIds : [];
        
        return _hosts.FirstOrDefault(host => 
            host.Name.ToLower() == hostName.ToLower() 
            && host.Address.ToLower() == address.ToLower() 
            && !ignoredIds.Contains(host.Id)
        );
    }
    

    public void CreateHost(string hostName, string address)
    {
        _hosts.Add(new Host { Id = ++_currentLargestHostId, Name = hostName, Address = address });
    }


    public int FindHostsCount()
    {
        return _hosts.Count;
    }


    public void RemoveHost(Host host)
    {
        _hosts.Remove(host);
    }


    public void UpdateHost(int hostId, UpdateHostData updateHostData)
    {
        var hostToUpdate = _hosts.FirstOrDefault(h => h.Id == hostId);
        
        if (hostToUpdate == null) 
            throw new Exception($"Update failed. Host with ID = {hostId} not found.");
        
        hostToUpdate.Name = updateHostData.Name;
        hostToUpdate.Address = updateHostData.Address;
    }
}