namespace ConsoleBookingSystem;

public class JsonFileHostRepository : IHostRepository
{
    private List<Host> _hosts;
    private int _currentLargestHostId; // is used internally to generate id for newly-added Hosts
    private IPersistence<List<Host>> _hostsPersistenceStorage;
    private IHostSeeder? _hostSeeder;

    public JsonFileHostRepository(IPersistence<List<Host>> hostsPersistenceStorage, IHostSeeder? hostSeeder = null)
    {
        // initializing  dependencies
        _hostsPersistenceStorage = hostsPersistenceStorage;
        _hostSeeder = hostSeeder;
    }

    public void LoadHostsFromPersistentStorage()
    {
        _hosts = _hostsPersistenceStorage.ReadData() ?? [];
        SetCurrentLargestHostId();
    }

    private void SetCurrentLargestHostId()
    {
        //storing largest host-ID to _currentLargestHostId
        foreach (var host in _hosts)
        {
            if (_currentLargestHostId < host.Id) _currentLargestHostId = host.Id;
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
            throw new Exception($"Update failed. Host with ID = {hostId} not found.");
        
        hostToUpdate.Name = updateHostData.Name;
        hostToUpdate.Address = updateHostData.Address;
    }

    public void SaveChanges()
    {
        _hostsPersistenceStorage.WriteData(_hosts);
    }
}