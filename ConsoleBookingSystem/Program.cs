using ConsoleBookingSystem;


// initializing needed dependencies
var io = new ConsoleInputOutput();

var jsonFilePersistence = new JsonFilePersistence<List<Host>>("hosts.json");

var hostRepository = new JsonFileHostRepository(jsonFilePersistence);
try
{
    hostRepository.LoadHostsFromPersistentStorage();
}
catch (Exception exception)
{
    io.Write("Something went wrong during loading data from storage:");
    io.Write(exception.Message);
    io.Write("Exiting the application...");
    return;
}

var hostService = new HostService(hostRepository);

// creating booking app instance with passing needed dependencies
var bookingApplication = new BookingApplication(hostService, io);
bookingApplication.LaunchApplication();