namespace ConsoleBookingSystem;

public class HostService
{
    private IHostStorage _hostStorage;
    

    public HostService(IHostStorage hostStorage)
    {
        _hostStorage = hostStorage;
    }
    

    public List<Host> GetHosts()
    {
        return _hostStorage.FindAllHosts();
    }
    
    
    public Host? GetHostById(int hostId)
    {
        return _hostStorage.FindHostById(hostId);
    }


    public bool HostExistsByNameAndAddress(string name, string address)
    {
        return _hostStorage.FindHostByNameAndAddress(name, address) != null;
    }


    public void AddHost(string hostName, string address)
    {
        if (HostExistsByNameAndAddress(hostName, address))
        {
            throw new Exception("Host already exists");
        }
        
        var newHostId = (_hostStorage.FindLastAddedHost()?.Id ?? 1) + 1;
        
        _hostStorage.SaveHost(new Host { Id = newHostId, Name = hostName, Address = address });
    }


    public void RemoveHostById(int hostId)
    {
        if (_hostStorage.FindHostsCount() == 0)
        {
            throw new Exception("No Hosts added yet.");
        }
            
        var host = _hostStorage.FindHostById(hostId);
        if (host == null)
        {
            throw new Exception($"Host with ID = {hostId} not found.");
        }
        
        _hostStorage.RemoveHost(host);
    }
}