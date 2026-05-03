using ConsoleBookingSystem;

Console.WriteLine("\n============= Hello, this is Booking System app! =============\n");

// creating Hosts
var host1 = new Host(5, "Hotel \'California\'", "Main Street, 1");
var host2 = new Host(8, "Hotel \'Minnesota\'", "Main Street, 5");
var host3 = new Host(14, "Hotel \'Minnesota\'", "Secondary Street, 53");


// adding Apartments to Hosts with creating Apartments "on the fly"
// (leaving host3 without added Apartments intentionally)
host1.Apartments.AddRange(new List<Apartment>
{
    new Apartment(1, 21, 150.1),
    new Apartment(2, 12, 110.4, true),
    new Apartment(3, 45, 20.7),
    new Apartment(4, 100, 560),
});

host2.Apartments.AddRange(new List<Apartment>
{
    new Apartment(5, 14, 340.4),
    new Apartment(6, 11, 10, true),
    new Apartment(7, 65, 240.1)
});




var hostsList = new List<Host> { host1, host2, host3 };


void ShowAllHosts()
{
    Console.WriteLine("\n===== List of all Hosts =====\n");
    
    int counter = 1;
    foreach (var host in hostsList)
    {
        Console.WriteLine($"{counter++}. {host}");
    }
}


Host? FindHostById(int id)
{
    return hostsList.FirstOrDefault(h => h.Id == id);
}


void ShowHostDetails()
{
    Console.WriteLine("\nEnter ID of the Host you are searching for (confirm input by pressing Enter):");
    
    if (!int.TryParse(Console.ReadLine(), out var searchedHostId))
    {
        Console.WriteLine("\nInvalid input - Host ID must be an integer. Please try again.");
        return;
    } 
    
    var host = FindHostById(searchedHostId);
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
    
    Console.WriteLine("\n-------");
}