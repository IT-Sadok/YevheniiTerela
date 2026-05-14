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
    
    
    public bool HostExistsById(int hostId)
    {
        return _hostStorage.FindHostById(hostId) != null;
    }


    public bool HostExistsByNameAndAddress(string name, string address, int? hostIdToIgnore = null)
    {
        return _hostStorage.FindHostByNameAndAddress(
            name, 
            address, 
            hostIdToIgnore != null ? new List<int> { (int)hostIdToIgnore } : []
        ) != null;
    }


    public void AddHost(string hostName, string address)
    {
        if (HostExistsByNameAndAddress(hostName, address))
        {
            throw new Exception("Host with specified name and address already exists.");
        }
        
        _hostStorage.CreateHost(new CreateHostData { Name = hostName, Address = address });
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


    public void EditHostById(int hostId, string updatedHostName, string updatedAddress)
    {
        if (!HostExistsById(hostId))
            throw new Exception($"Host with ID = {hostId} not found.");

        if (HostExistsByNameAndAddress(updatedHostName, updatedAddress, hostId))
            throw new Exception("Host with specified Name and Address already exists.");
        
        _hostStorage.UpdateHost(hostId, new UpdateHostData { Name = updatedHostName, Address = updatedAddress });
    }
}