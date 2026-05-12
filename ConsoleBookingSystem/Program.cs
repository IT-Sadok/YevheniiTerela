using ConsoleBookingSystem;

Console.WriteLine("\n============= Hello, this is Booking System app! =============\n");

var hostStorage = new InMemoryHostStorage();
var hostsService = new HostService(hostStorage);
var hostsList = hostsService.GetHosts();
var io = new ConsoleInputOutput();

void ShowAllHosts()
{
    io.Write("\n===== List of all Hosts =====\n");
    
    foreach (var host in hostsList)
    {
        io.Write(host.ToString());
    }
}


void ShowHostDetails()
{
    var searchedHostId = io.ReadInt(
        "\nEnter ID of the Host you are searching for (confirm input by pressing Enter):",
        true,
        "\nInvalid input - Host ID must be an integer. Please try again."
    );
    
    var host = hostsService.GetHostById(searchedHostId);
    if (host is null) 
    {
        io.Write($"\nNo host with found with ID = {searchedHostId}.");
        return;
    }

    io.Write("\n==== Search results: ====\n");
    io.Write($"{host}:");
    
    if (host.Apartments.Count == 0)
    {
        io.Write("No apartments found for current Host.");
        return;
    }

    foreach (var apartment in host.Apartments)
    {
        io.Write($" - {apartment}");
    }
}


void ExitApp()
{
    io.Write("\nExiting... See you next time!");
}


void HandleInvalidActionInput()
{
    io.Write("\nInvalid number: a number for an action must be between 0 and 2.");
}


int pressedNumericKey;

while (true)
{
    io.Write("\nSelect action:");
    io.Write("- press 1 to show all Hosts;");
    io.Write("- press 2 to show all available Apartments for a Host (by Host's ID);");
    io.Write("- press 0 to exit the app.");
    
    io.Write("");

    pressedNumericKey = io.ReadIntFromKey("", true, "\nInvalid input: please enter a number between 0 and 2.");
    
    io.Write("");

    switch (pressedNumericKey)
    {
        case 1: ShowAllHosts(); break;
        case 2: ShowHostDetails(); break;
        case 0: ExitApp(); return;
        default: HandleInvalidActionInput(); continue; // "continue;" keyword is for new iteration of while loop, related to switch{}
    }

    io.RequireAnyKey("\nPress any key to continue");
    
    io.Write("\n-------");
}