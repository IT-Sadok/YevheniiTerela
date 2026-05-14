namespace ConsoleBookingSystem;

public interface IHostStorage
{
    public List<Host> FindAllHosts();
    
    public Host? FindHostById(int hostId);
    
    public Host? FindHostByNameAndAddress(string hostName, string address, List<int>? ignoredHostIds = null);
    
    public void CreateHost(CreateHostData createHostData);
    
    public int FindHostsCount();
    
    public void RemoveHost(Host host);
    
    public void UpdateHost(int hostId, UpdateHostData updateHostData);
}