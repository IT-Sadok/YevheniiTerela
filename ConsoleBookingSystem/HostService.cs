using System.Data;

namespace ConsoleBookingSystem;

public class HostService : IHostService
{
    private bool _anyUnsavedChanges = false;
    private IHostRepository _hostRepository;

    public HostService(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
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
            throw new ArgumentException("Host with specified name and address already exists.");
        
        _hostRepository.CreateHost(new CreateHostData { Name = hostName, Address = address });
        
        SetAnyUnsavedChanges(true);
    }

    public void RemoveHostById(int hostId)
    {
        if (hostId <= 0)
            throw new ArgumentOutOfRangeException(nameof(hostId), "Host ID cannot be zero or negative.");
        
        if (_hostRepository.FindHostsCount() == 0)
            throw new HostNotFoundException("No Hosts found.");
            
        var host = _hostRepository.FindHostById(hostId);
        if (host == null)
            throw new HostNotFoundException("Host with specified ID not found.", hostId);
        
        _hostRepository.RemoveHost(host);
        
        SetAnyUnsavedChanges(true);
    }

    public void EditHostById(int hostId, string updatedHostName, string updatedAddress)
    {
        if (!HostExistsById(hostId))
            throw new HostNotFoundException("Host with specified ID not found.", hostId);

        if (HostExistsByNameAndAddress(updatedHostName, updatedAddress, hostId))
            throw new ArgumentException("Host with specified Name and Address already exists.");
        
        _hostRepository.UpdateHost(hostId, new UpdateHostData { Name = updatedHostName, Address = updatedAddress });
        
        SetAnyUnsavedChanges(true);
    }
    
    public bool ApartmentExistsById(int hostId, int apartmentId)
    {
        return _hostRepository.FindApartmentById(hostId, apartmentId) != null;
    }

    public void AddApartment(int hostId, CreateApartmentData createApartmentData)
    {
        if (!HostExistsById(hostId))
            throw new HostNotFoundException("Host with specified ID not found.", hostId);
        
        _hostRepository.CreateApartment(hostId, new CreateApartmentData { Number = createApartmentData.Number, Price = createApartmentData.Price });
        
        SetAnyUnsavedChanges(true);
    }
    
    public void EditApartmentById(int hostId, int apartmentId, UpdateApartmentData updateApartmentData)
    {
        if (!HostExistsById(hostId))
            throw new HostNotFoundException("Host with specified ID not found.", hostId);
        
        if (!ApartmentExistsById(hostId, apartmentId))
            throw new ApartmentNotFoundException("Apartment with specified ID not found.", apartmentId);
        
        _hostRepository.UpdateApartment(hostId, apartmentId, updateApartmentData);
        
        SetAnyUnsavedChanges(true);
    }
    
    public void RemoveApartmentById(int hostId, int apartmentId)
    {
        if (hostId <= 0)
            throw new ArgumentOutOfRangeException(nameof(hostId), "Host ID cannot be zero or negative.");
        
        if (apartmentId <= 0)
            throw new ArgumentOutOfRangeException(nameof(apartmentId), "Apartment ID cannot be zero or negative.");
        
        if (_hostRepository.FindHostsCount() == 0)
            throw new HostNotFoundException("No Hosts found.");
        
        if (_hostRepository.FindHostApartmentsCount(hostId) == 0)
            throw new ApartmentNotFoundException("No Apartments found in the Host with specified ID.", apartmentId);
        
        _hostRepository.RemoveApartment(hostId, apartmentId);
        
        SetAnyUnsavedChanges(true);
    }

    public void SaveChanges()
    {
        _hostRepository.SaveChanges();
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