using ConsoleBookingSystem;

Console.WriteLine("\n============= Hello, this is Booking System app! =============\n");

// creating Hosts
var host1 = new Host { Id = 5, Name = "Hotel \'California\'", Address = "Main Street, 1" };
var host2 = new Host { Id = 8, Name = "Hotel \'Minnesota\'", Address = "Main Street, 5" };
var host3 = new Host { Id = 14, Name = "Hotel \'Minnesota\'", Address = "Secondary Street, 53" };


// adding Apartments to Hosts with creating Apartments "on the fly"
// (leaving host3 without added Apartments intentionally)
host1.Apartments.AddRange(new List<Apartment>
{
    new Apartment{ Id = 1, Number = 21,  Price = 150.1 },
    new Apartment{ Id = 2, Number = 12, Price = 110.4, IsBooked = true},
    new Apartment{ Id = 3, Number = 45, Price = 20.7 },
    new Apartment{ Id = 4, Number = 100, Price = 560 },
});

host2.Apartments.AddRange(new List<Apartment>
{
    new Apartment{ Id = 5,  Number = 14,  Price = 340.4 },
    new Apartment{ Id = 6, Number = 11, Price = 10, IsBooked = true },
    new Apartment { Id = 7, Number = 65, Price = 240.1 }
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