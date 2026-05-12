using ConsoleBookingSystem;

Console.WriteLine("\n============= Hello, this is Booking System app! =============\n");

var hostStorage = new InMemoryHostStorage();
var hostsService = new HostService(hostStorage);
var hostsList = hostsService.GetHosts();


void ShowAllHosts()
{
    Console.WriteLine("\n===== List of all Hosts =====\n");
    
    foreach (var host in hostsList)
    {
        Console.WriteLine(host);
    }
}


void ShowHostDetails()
{
    Console.WriteLine("\nEnter ID of the Host you are searching for (confirm input by pressing Enter):");
    
    if (!int.TryParse(Console.ReadLine(), out var searchedHostId))
    {
        Console.WriteLine("\nInvalid input - Host ID must be an integer. Please try again.");
        return;
    } 
    
    var host = hostsService.GetHostById(searchedHostId);
    if (host is null) 
    {
        Console.WriteLine($"\nNo host with found with ID = {searchedHostId}.");
        return;
    }

    Console.WriteLine("\n==== Search results: ====\n");
    Console.WriteLine($"{host}:");
    
    if (host.Apartments.Count == 0)
    {
        Console.WriteLine("No apartments found for current Host.");
        return;
    }

    foreach (var apartment in host.Apartments)
    {
        Console.WriteLine($" - {apartment}");
    }
}


void ExitApp()
{
    Console.WriteLine("\nExiting... See you next time!");
}


void HandleInvalidInput()
{
    Console.WriteLine("\nInvalid input: please enter a number between 0 and 2.");
}


void HandleInvalidActionInput()
{
    Console.WriteLine("\nInvalid number: a number for an action must be between 0 and 2.");
}


int pressedNumericKey;

while (true)
{
    Console.WriteLine("\nSelect action:");
    Console.WriteLine("- press 1 to show all Hosts;");
    Console.WriteLine("- press 2 to show all available Apartments for a Host (by Host's ID);");
    Console.WriteLine("- press 0 to exit the app.");
    
    Console.WriteLine("");

    if (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out pressedNumericKey))
    {
        HandleInvalidInput();
        continue;
    }
    
    Console.WriteLine("");

    switch (pressedNumericKey)
    {
        case 1: ShowAllHosts(); break;
        case 2: ShowHostDetails(); break;
        case 0: ExitApp(); return;
        default: HandleInvalidActionInput(); continue;
    }

    Console.WriteLine("\nPress any key to continue");
    Console.ReadKey();
    
    Console.WriteLine("\n-------");
}