namespace ConsoleBookingSystem;

public interface IHostStorage
{
    public List<Host> FindAllHosts();
    
    public Host? FindHostById(int hostId);
    
    public Host? FindHostByNameAndAddress(string hostName, string address);
    
    public Host? FindLastAddedHost();
    
    public void SaveHost(Host host);
    
    public int FindHostsCount();
    
    public void RemoveHost(Host host);
}