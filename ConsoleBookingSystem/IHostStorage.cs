namespace ConsoleBookingSystem;

public interface IHostStorage
{
    public List<Host> FindAllHosts();
    
    public Host? FindHostById(int hostId);
}