namespace ConsoleBookingSystem;

public class InMemoryHostRepository : IHostRepository
{
    private List<Host> _hosts;
    private int _currentLargestHostId; // is used internally to generate id for newly-added Hosts
    private int _currentLargestApartmentId;

    public InMemoryHostRepository(List<Host> hosts)
    {
        _hosts = hosts;

        //storing largest host-ID to _currentLargestHostId
        foreach (var host in _hosts)
        {
            if (_currentLargestHostId < host.Id) _currentLargestHostId = host.Id;
            
            foreach (var apartment in host.Apartments)
            {
                if (_currentLargestApartmentId < apartment.Id) _currentLargestApartmentId = apartment.Id;
            }
        }
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

    public void CreateHost(CreateHostData createHostData)
    {
        _hosts.Add(new Host { Id = ++_currentLargestHostId, Name = createHostData.Name, Address = createHostData.Address });
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
            throw new HostNotFoundException($"Update failed. Host with ID = {hostId} not found.");
        
        hostToUpdate.Name = updateHostData.Name;
        hostToUpdate.Address = updateHostData.Address;
    }

    public List<Apartment> FindAllHostApartments(int hostId)
    {
        return _hosts.FirstOrDefault(host => host.Id == hostId)?.Apartments.ToList() ?? [];
    }

    public int FindHostApartmentsCount(int hostId)
    {
        return FindHostById(hostId)?.Apartments.Count ?? 0;
    }
    
    public Apartment? FindApartmentById(int hostId, int apartmentId)
    {
        return FindHostById(hostId)?.Apartments.FirstOrDefault(apartment => apartment.Id == apartmentId);
    }

    public void CreateApartment(int hostId, CreateApartmentData createApartmentData)
    {
        var hostForApartment = FindHostById(hostId);
        if (hostForApartment == null)
            throw new HostNotFoundException($"Can not create Apartment for Host with ID = {hostId}: specified Host not found.");
        
        hostForApartment.Apartments.Add(
            new Apartment
            {
                Id = ++_currentLargestApartmentId,
                Number = createApartmentData.Number,
                Price = createApartmentData.Price,
                IsBooked = createApartmentData.IsBooked
            }
        );
    }
    
    public void UpdateApartment(int hostId, int apartmentId, UpdateApartmentData updateApartmentData)
    {
        var apartmentToUpdate = FindApartmentById(hostId, apartmentId);
        
        if (apartmentToUpdate == null)
            throw new ApartmentNotFoundException("Apartment with specified ID not found.", apartmentId);

        if (updateApartmentData.Number != null && updateApartmentData.Number != apartmentToUpdate.Number)
        {
            apartmentToUpdate.Number = (int)updateApartmentData.Number;    
        }
        
        if (updateApartmentData.Price != null && Math.Abs((double)updateApartmentData.Price - apartmentToUpdate.Price) > 0.00001)
        {
            apartmentToUpdate.Price = (double)updateApartmentData.Price;    
        }
        
        if (updateApartmentData.IsBooked != null)
        {
            apartmentToUpdate.IsBooked = (bool)updateApartmentData.IsBooked;
        }
    }

    public void RemoveApartment(int hostId, int apartmentId)
    {
        var apartmentHost = FindHostById(hostId);
        if (apartmentHost == null)
            throw new HostNotFoundException($"Host with ID = {hostId} not found.", hostId);
        
        var apartmentToRemove = FindApartmentById(hostId, apartmentId);
        if (apartmentToRemove == null)
            throw new ApartmentNotFoundException("Apartment with specified ID not found.", apartmentId);
        
        apartmentHost.Apartments.Remove(apartmentToRemove);
    }
    
    public void SaveChanges()
    {
        // this method is not expected to do anything for current implementation of IHostRepository
    }
}