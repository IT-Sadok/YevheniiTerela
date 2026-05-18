using ConsoleBookingSystem;

// getting Hosts data from persistent storage with fallback to use "seed" data
var jsonHostsStorage = new JsonHostFileStorage();
var hosts = jsonHostsStorage.ReadData();

if (hosts.Count == 0)
{
    var hostsSeeder = new DefaultHostSeeder();
    hosts = hostsSeeder.GetDefaultHosts();
}

// initializing needed dependencies
var hostRepository = new InMemoryHostRepository(hosts);
var hostService = new HostService(hostRepository);
var io = new ConsoleInputOutput();

// creating booking app instance with passing needed dependencies
var bookingApplication = new BookingApplication(hostService, io);
bookingApplication.LaunchApplication();