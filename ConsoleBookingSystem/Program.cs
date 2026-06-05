using ConsoleBookingSystem;


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

var bookingApplication = new BookingApplication(hostService, io);
bookingApplication.LaunchApplication();



// // ======= Race condition simulation for Hosts & Apartments (confirmed at June 4, 2026, 13:00 UTC and fixed in 71dceaf2f6fcc250abec3eb3158430880cfae957 commit)
// var raceConditionRepository = new InMemoryHostRepository((new RaceConditionSimulatorHostSeeder()).GetHosts());
// var raceConditionSimulator = new HostRaceConditionSimulator(raceConditionRepository);
//
// for (int i = 0; i < 50; i++)
// {
//     await raceConditionSimulator.IncreasePriceForSharedApartment(5);
// }
//     
// // ======= Race condition simulation for Hosts & Apartments (confirmed at June 4, 2026, 13:00 UTC and fixed in 71dceaf2f6fcc250abec3eb3158430880cfae957 commit)