using ConsoleBookingSystem;

// initializing needed dependencies
var hostRepository = new InMemoryHostRepository();
var hostService = new HostService(hostRepository);
var io = new ConsoleInputOutput();

// creating booking app instance with passing needed dependencies
var bookingApplication = new BookingApplication(hostService, io);
bookingApplication.LaunchApplication();