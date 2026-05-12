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
}