namespace ConsoleBookingSystem;

public class BookingApplication
{
    private IHostRepository _hostsRepository;
    private IHostService _hostsService;
    private ConsoleInputOutput _io;
    private Dictionary<BookingApplicationActionType, string> _actions =  new Dictionary<BookingApplicationActionType, string>
    {
        { BookingApplicationActionType.ShowAllHosts, "Show all Hosts" },
        { BookingApplicationActionType.ShowHostDetails, "Show all available Apartments for a Host (by Host's ID)" },
        { BookingApplicationActionType.AddNewHost, "Add new Host" },
        { BookingApplicationActionType.RemoveHost, "Remove Host by ID" },
        { BookingApplicationActionType.UpdateHost, "Update Host by ID" },
        { BookingApplicationActionType.ExitApplication, "Exit the application" },
    };
    
    
    public BookingApplication()
    {
        _hostsRepository = new InMemoryHostRepository();
        _hostsService = new HostService(_hostsRepository);
        _io = new ConsoleInputOutput();
    }


    int TryRetrieveHostId(string message, string invalidInputMessage)
    {
        int hostIdToRetrieve;
        
        // iterating until user inputs valid and existing host-id
        while (true)
        {
            hostIdToRetrieve = _io.ReadInt(message, true, invalidInputMessage);
            if (_hostsService.HostExistsById(hostIdToRetrieve))
            {
                break;
            }
            _io.Write($"Host with ID = {hostIdToRetrieve} does not exist. Please try again.");
        }

        return hostIdToRetrieve;
    }
    
    
    void HandleShowAllHostsAction()
    {
        _io.Write("\n===== List of all Hosts =====\n");
        
        var hosts = _hostsService.GetHosts();

        if (hosts.Count == 0)
        {
            _io.Write("No Hosts added yet.");
            return;
        }
        
        foreach (var host in hosts)
        {
            _io.Write(host.ToString());
        }
    }
    
    
    void HandleShowHostDetailsAction()
    {
        var searchedHostId = TryRetrieveHostId(
            "\nEnter ID of the Host you are searching for (confirm input by pressing Enter):",
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
        
        string newHostName = _io.ReadString("Enter new Host name:", true, "Host name can not be empty, please try again.").Trim();
        string newHostAddress = _io.ReadString("Enter new Host address:", true, "Host address can not be empty, please try again.").Trim();
        
        try
        {
            _hostsService.AddHost(newHostName, newHostAddress);
            // this line is expected to be shown when Host removed successfully
            _io.Write("\nHost added successfully!");
        }
        catch (Exception exception)
        {
            // this line is expected to be shown when there was an error during adding a Host
            // (Host with specified name and address already exists, etc)
            _io.Write($"\n{exception.Message}");
        }
    }


    void HandleRemoveHostAction()
    {
        _io.Write("\n==== You are removing a Host: ====");
        
        var hostIdToRemove = TryRetrieveHostId(
            "\nEnter ID of the Host that needs to be removed (confirm input by pressing Enter):",
            "\nInvalid host ID entered, please try again."
        );
        try
        {
            _hostsService.RemoveHostById(hostIdToRemove);
            // this line is expected to be shown when Host removed successfully
            _io.Write("\nHost removed successfully!");
        }
        catch (Exception exception)
        {
            // this line is expected to be shown when there was an error during deleting a Host
            // (Host with specified ID was not found, etc)
            _io.Write($"\n{exception.Message}");
        }
    }
    
    
    void HandleUpdateHostAction()
    {
        _io.Write("\n==== You are updating a particular Host's data ====");
        
        try
        {
            int hostIdToUpdate = TryRetrieveHostId(
                "\nEnter ID of the Host that needs to be updated (confirm input by pressing Enter):",
                "\nInvalid host ID entered, please try again.");
            
            var newHostName = _io.ReadString("Enter updated Host name:", true, "Host name can not be empty, please try again.").Trim();
            var newHostAddress = _io.ReadString("Enter updated Host address:", true, "Host address can not be empty, please try again.").Trim();
            
            _hostsService.EditHostById(hostIdToUpdate, newHostName, newHostAddress);
            
            // this line is expected to be shown when Host removed successfully
            _io.Write("\nHost is updated successfully!");
        }
        catch (Exception exception)
        {
            // this line is expected to be shown when there was an error during deleting a Host
            // (Host with specified ID was not found, etc)
            _io.Write($"\n{exception.Message}");
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


    ProcessedBookingApplicationActionResult ProcessAction()
    {
        BookingApplicationActionType currentActionType;
            
        currentActionType = _io.ReadEnumValue<BookingApplicationActionType>("", true, "Invalid action selected selected, please try again.");

        switch (currentActionType)
        {
            case BookingApplicationActionType.ShowAllHosts: HandleShowAllHostsAction(); break;
            case BookingApplicationActionType.ShowHostDetails: HandleShowHostDetailsAction(); break;
            case BookingApplicationActionType.AddNewHost: HandleAddNewHostAction(); break;
            case BookingApplicationActionType.RemoveHost: HandleRemoveHostAction(); break;
            case BookingApplicationActionType.UpdateHost: HandleUpdateHostAction(); break;
            case BookingApplicationActionType.ExitApplication:
            {
                HandleExitAppAction();
                return ProcessedBookingApplicationActionResult.GotExitApplicationRequest;
            }
            default:
            {
                HandleInvalidActionInput();
                // "continue;" keyword is for new iteration of while loop, related to switch {}
                return ProcessedBookingApplicationActionResult.GotInvalidAction;
            }
        }
        
        return ProcessedBookingApplicationActionResult.GotAndProcessedValidAction;
    }


    void ShowActionsMenu()
    {
        int loopCounter;
        bool isLastMenuItemsIteration;
        ProcessedBookingApplicationActionResult actionResult;
        
        while (true)
        {
            loopCounter = 1; // resetting counter at the beginning of every iteration
            
            _io.Write("\nSelect action:");
            
            foreach (var actionType in _actions)
            {
                isLastMenuItemsIteration = loopCounter++ == _actions.Count;
                _io.Write($"- press {(int)actionType.Key} to {actionType.Value}{(isLastMenuItemsIteration ? "." : ";")}");
            }
            
            actionResult = ProcessAction();
            
            // changing normal infinite-menu-display flow in case if run into edge case in ProcessCurrentAction()
            // (like when user requested to exit app; or when invalid action received from user
            switch (actionResult)
            {
                case ProcessedBookingApplicationActionResult.GotExitApplicationRequest: 
                    return; // finishing the program
                case ProcessedBookingApplicationActionResult.GotInvalidAction:
                    continue; // interrupting current iteration here and starting new iteration
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