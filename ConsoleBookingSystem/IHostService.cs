namespace ConsoleBookingSystem;

public interface IHostService
{
    public List<Host> GetHosts();
    public Host? GetHostById(int hostId);
    public bool HostExistsById(int hostId);
    public bool HostExistsByNameAndAddress(string name, string address, int? hostIdToIgnore = null);
    public void AddHost(string hostName, string address);
    public void RemoveHostById(int hostId);
    public void EditHostById(int hostId, string updatedHostName, string updatedAddress);
    public void AddApartment(int hostId, CreateApartmentData createApartmentData);
    public bool ApartmentExistsById(int hostId, int apartmentId);
    public void EditApartmentById(int hostId, int apartmentId, UpdateApartmentData updateApartmentData);
    public void SaveChanges();
    public bool AnyUnsavedChanges();
}