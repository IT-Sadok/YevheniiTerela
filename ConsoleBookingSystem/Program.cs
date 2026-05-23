using ConsoleBookingSystem;


// initializing needed dependencies
var io = new ConsoleInputOutput();

var jsonFilePersistence = new JsonFilePersistence<List<Host>>("hosts.json");
IHostRepository hostRepository;

try
{
    hostRepository = new JsonFileHostRepository(jsonFilePersistence);
}
catch (PersistenceException exception)
{
    io.Write("\nSomething went wrong during in initializing data storage:");
    io.Write(exception.Message);
    return;
}

var hostService = new HostService(hostRepository);

// creating booking app instance with passing needed dependencies
var bookingApplication = new BookingApplication(hostService, io);
bookingApplication.LaunchApplication();