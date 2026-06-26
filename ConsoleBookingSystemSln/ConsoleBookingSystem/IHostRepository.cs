namespace ConsoleBookingSystem;

public interface IHostRepository
{
    public List<Host> FindAllHosts();
    public Host? FindHostById(int hostId);
    public Host? FindHostByNameAndAddress(string hostName, string address, List<int>? ignoredHostIds = null);
    public void CreateHost(CreateHostData createHostData);
    public int FindHostsCount();
    public void RemoveHost(Host host);
    public void UpdateHost(int hostId, UpdateHostData updateHostData);
    public List<Apartment> FindAllHostApartments(int hostId);
    public Apartment? FindApartmentById(int hostId, int apartmentId);
    public void CreateApartment(int hostId, CreateApartmentData createApartmentData);
    public void UpdateApartment(int hostId, int apartmentId, UpdateApartmentData updateApartmentData);
    public void RemoveApartment(int hostId, int apartmentId);
    public int FindHostApartmentsCount(int hostId);
    public void SaveChanges();
}