namespace ConsoleBookingSystem;

public class HostService : IHostService
{
    private bool _anyUnsavedChanges = false;
    private IHostRepository _hostRepository;
    private IHostFileStorage _hostFileStorage;

    public HostService(IHostRepository hostRepository, IHostFileStorage hostFileStorage)
    {
        _hostRepository = hostRepository;
        _hostFileStorage = hostFileStorage;
    }

    public List<Host> GetHosts()
    {
        return _hostRepository.FindAllHosts();
    }
    
    public Host? GetHostById(int hostId)
    {
        return _hostRepository.FindHostById(hostId);
    }
    
    public bool HostExistsById(int hostId)
    {
        return _hostRepository.FindHostById(hostId) != null;
    }
    
    public bool HostExistsByNameAndAddress(string name, string address, int? hostIdToIgnore = null)
    {
        return _hostRepository.FindHostByNameAndAddress(
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
        
        _hostRepository.CreateHost(new CreateHostData { Name = hostName, Address = address });
        
        SetAnyUnsavedChanges(true);
    }

    public void RemoveHostById(int hostId)
    {
        if (_hostRepository.FindHostsCount() == 0)
        {
            throw new Exception("No Hosts added yet.");
        }
            
        var host = _hostRepository.FindHostById(hostId);
        if (host == null)
        {
            throw new Exception($"Host with ID = {hostId} not found.");
        }
        
        _hostRepository.RemoveHost(host);
        
        SetAnyUnsavedChanges(true);
    }

    public void EditHostById(int hostId, string updatedHostName, string updatedAddress)
    {
        if (!HostExistsById(hostId))
            throw new Exception($"Host with ID = {hostId} not found.");

        if (HostExistsByNameAndAddress(updatedHostName, updatedAddress, hostId))
            throw new Exception("Host with specified Name and Address already exists.");
        
        _hostRepository.UpdateHost(hostId, new UpdateHostData { Name = updatedHostName, Address = updatedAddress });
        
        SetAnyUnsavedChanges(true);
    }

    public void SaveChanges()
    {
        _hostFileStorage.WriteData(_hostRepository.FindAllHosts());
        SetAnyUnsavedChanges(false);
    }
    
    private void SetAnyUnsavedChanges(bool anyUnsavedChanges)
    {
        _anyUnsavedChanges = anyUnsavedChanges;
    }
    
    public bool AnyUnsavedChanges()
    {
        return _anyUnsavedChanges;
    }
}