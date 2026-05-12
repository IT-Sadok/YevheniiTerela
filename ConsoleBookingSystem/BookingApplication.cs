namespace ConsoleBookingSystem;

public class BookingApplication
{
    private IHostStorage _hostsStorage;
    private HostService _hostsService;
    private ConsoleInputOutput _io;
    private Dictionary<BookingApplicationActionType, string> _actions =  new Dictionary<BookingApplicationActionType, string>
    {
        { BookingApplicationActionType.ShowAllHosts, "Show all Hosts" },
        { BookingApplicationActionType.ShowHostDetails, "Show all available Apartments for a Host (by Host's ID)" },
        { BookingApplicationActionType.AddNewHost, "Add new Host" },
        { BookingApplicationActionType.ExitApplication, "Exit the application" },
    };
    
    
    public BookingApplication()
    {
        _hostsStorage = new InMemoryHostStorage();
        _hostsService = new HostService(_hostsStorage);
        _io = new ConsoleInputOutput();
    }
    
    
    void HandleShowAllHostsAction()
    {
        _io.Write("\n===== List of all Hosts =====\n");
        
        var hosts = _hostsService.GetHosts();
        foreach (var host in hosts)
        {
            _io.Write(host.ToString());
        }
    }
    
    
    void HandleShowHostDetailsAction()
    {
        var searchedHostId = _io.ReadInt(
            "\nEnter ID of the Host you are searching for (confirm input by pressing Enter):",
            true,
            "\nInvalid input - Host ID must be an integer. Please try again."
        );
    
        var host = _hostsService.GetHostById(searchedHostId);
        if (host is null) 
        {
            _io.Write($"\nNo host with found with ID = {searchedHostId}.");
            return;
        }

        _io.Write("\n==== Search results: ====\n");
        _io.Write(host.ToString());
    
        if (host.Apartments.Count == 0)
        {
            _io.Write("No apartments found for current Host.");
            return;
        }

        foreach (var apartment in host.Apartments)
        {
            _io.Write($" - {apartment}");
        }
    }


    void HandleAddNewHostAction()
    {
        _io.Write("\n==== You are adding new Host: ====\n");
        
        string newHostName = _io.ReadString("Enter new Host name:", true, "Host name can not be empty, please try again.");
        string newHostAddress = _io.ReadString("Enter new Host address:", true, "Host address can not be empty, please try again.");
        
        try
        {
            _hostsService.AddHost(newHostName, newHostAddress);
            _io.Write("\nHost added successfully!");
        }
        catch (Exception ex)
        {
            _io.Write($"\n{ex.Message}");
        }
    }
    
    
    void HandleExitAppAction()
    {
        _io.Write("\nExiting... See you next time!");
    }
    
    
    void HandleInvalidActionInput()
    {
        _io.Write("\nInvalid number: a number for an action must be between 0 and 2.");
    }


    void ShowActionsMenu()
    {
        BookingApplicationActionType currentActionType;
        int loopCounter = 1;
        bool isLastIteration;
        
        while (true)
        {
            _io.Write("\nSelect action:");
            
            foreach (var actionType in _actions)
            {
                isLastIteration = loopCounter++ == _actions.Count;
                _io.Write($"- press {(int)actionType.Key} to {actionType.Value}{(isLastIteration ? "." : ";")}");
            }

            currentActionType = _io.ReadEnumValue<BookingApplicationActionType>("", true, "Invalid action selected selected, please try again.");

            switch (currentActionType)
            {
                case BookingApplicationActionType.ShowAllHosts: HandleShowAllHostsAction(); break;
                case BookingApplicationActionType.ShowHostDetails: HandleShowHostDetailsAction(); break;
                case BookingApplicationActionType.AddNewHost: HandleAddNewHostAction(); break;
                case BookingApplicationActionType.ExitApplication:
                {
                    HandleExitAppAction();
                    return;
                }
                default:
                {
                    HandleInvalidActionInput();
                    // "continue;" keyword is for new iteration of while loop, related to switch {}
                    continue;
                }
            }

            _io.RequireAnyKey("\n==== Press any key to continue ====");

            _io.Write("\n-------");
        }
    }


    public void LaunchApplication()
    {
        _io.Write("\n============= Hello, this is Booking System app! =============\n");
        ShowActionsMenu();
    }

}