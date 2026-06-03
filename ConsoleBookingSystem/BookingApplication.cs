namespace ConsoleBookingSystem;

public class BookingApplication
{
    private IHostService _hostsService;
    private IConsoleInputOutput _io;
    private Dictionary<BookingApplicationActionType, string> _actions = new Dictionary<BookingApplicationActionType, string>
    {
        { BookingApplicationActionType.ShowAllHosts, "Show all Hosts" },
        { BookingApplicationActionType.ShowHostDetails, "Show all available Apartments for a Host (by Host's ID)" },
        { BookingApplicationActionType.AddNewHost, "Add new Host" },
        { BookingApplicationActionType.RemoveHost, "Remove Host by ID" },
        { BookingApplicationActionType.UpdateHost, "Update Host by ID" },
        { BookingApplicationActionType.AddNewApartment, "Add new Apartment" },
        { BookingApplicationActionType.SaveChanges, "Save Changes" },
        { BookingApplicationActionType.ExitApplication, "Exit the application" },
    };
    
    public BookingApplication(IHostService hostService, IConsoleInputOutput io)
    {
        _hostsService = hostService;
        _io = io;
    }
        
    public void LaunchApplication()
    {
        _io.Write("\n============= Hello, this is Booking System app! =============\n");
        ShowActionsMenu();
    }

    private string FormatActionMenuItemName(KeyValuePair<BookingApplicationActionType, string> actionType, string appendString)
    {
        string actionMenuItemName = $"- press {(int)actionType.Key} to {actionType.Value}";
        
        switch (actionType.Key)
        {
            case BookingApplicationActionType.SaveChanges:
            {
                bool anyChanges = _hostsService.AnyUnsavedChanges();
                actionMenuItemName += anyChanges ? " (there are unsaved changes)" : " (no unsaved changes)";
                break;
            }
        }
        
        return actionMenuItemName + appendString;
    }

    private void ShowActionsMenu()
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
                _io.Write(FormatActionMenuItemName(actionType, isLastMenuItemsIteration ? "." : ";"));
            }
            
            actionResult = ProcessAction();
            
            // changing normal infinite-menu-display flow in case if run into edge case in ProcessCurrentAction()
            // (like when user requested to exit app; or when invalid action received from user
            switch (actionResult)
            {
                case ProcessedBookingApplicationActionResult.GotExitApplicationRequest:
                {
                    if (ConfirmExitApplication()) 
                    {
                        HandleExitAppAction();
                        return; // finishing the program
                    }
                    continue; // otherwise - going back to the Menu
                }
                case ProcessedBookingApplicationActionResult.GotInvalidAction:
                    continue; // interrupting current iteration here and starting new iteration
            }

            _io.RequireAnyKey("\n==== Press any key to continue ====");

            _io.Write("\n-------");
        }
    }

    private ProcessedBookingApplicationActionResult ProcessAction()
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
            case BookingApplicationActionType.AddNewApartment: HandleAddNewApartmentAction(); break;
            case BookingApplicationActionType.SaveChanges: HandleSaveChangesAction(); break;
            case BookingApplicationActionType.ExitApplication:
            {
                return ProcessedBookingApplicationActionResult.GotExitApplicationRequest;
            }
            default:
            {
                HandleInvalidActionInput();
                // "continue;" keyword is for new iteration of while loop, and is not related to switch {}
                return ProcessedBookingApplicationActionResult.GotInvalidAction;
            }
        }
        
        return ProcessedBookingApplicationActionResult.GotAndProcessedValidAction;
    }

    private int TryRetrieveHostId(string message, string invalidInputMessage)
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

    private bool ConfirmExitApplication()
    {
        if (!_hostsService.AnyUnsavedChanges())
            return true;
        
        var pressedKey = _io.ReadPressKey("\nThere are unsaved changes which will be lost after exit.\nPress Enter to confirm exit. Press any other key to go back to the Menu.");
        return pressedKey == ConsoleKey.Enter;
    }
    
    private void HandleShowAllHostsAction()
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
    
    private void HandleShowHostDetailsAction()
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

    private void HandleAddNewHostAction()
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
        catch (ArgumentException exception)
        {
            // this line is expected to be shown when there was an error during adding a Host
            // (Host with specified name and address already exists, etc)
            _io.Write("\nInvalid arguments entered.");
            _io.Write($"{exception.Message}");
        }
    }

    private void HandleRemoveHostAction()
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
        catch (ArgumentOutOfRangeException exception)
        {
            _io.Write("\nInvalid arguments entered.");
            _io.Write($"{exception.Message}");
        }
        catch (HostNotFoundException exception)
        {
            if (exception.HostId != null)
                _io.Write($"\nHost with with ID = {exception.HostId} not found!");
            else 
                _io.Write("\nNo data found.");
        }
    }
    
    private void HandleUpdateHostAction()
    {
        _io.Write("\n==== You are updating a particular Host's data ====");

        try
        {
            int hostIdToUpdate = TryRetrieveHostId(
                "\nEnter ID of the Host that needs to be updated (confirm input by pressing Enter):",
                "\nInvalid host ID entered, please try again.");

            var newHostName = _io.ReadString("Enter updated Host name:", true,
                "Host name can not be empty, please try again.").Trim();
            var newHostAddress = _io.ReadString("Enter updated Host address:", true,
                "Host address can not be empty, please try again.").Trim();

            _hostsService.EditHostById(hostIdToUpdate, newHostName, newHostAddress);

            // this line is expected to be shown when Host removed successfully
            _io.Write("\nHost is updated successfully!");
        }
        catch (ArgumentException exception)
        {
            _io.Write("\nInvalid argument entered.");
            _io.Write($"{exception.Message}");
        }
        catch (HostNotFoundException exception)
        {
            if (exception.HostId != null)
                _io.Write($"\nHost with with ID = {exception.HostId} not found!");
            else 
                _io.Write("\nNo data found.");
        }
    }

    private void HandleAddNewApartmentAction()
    {
        _io.Write("\n==== You are adding new Apartment ====");
        
        var targetHostId = TryRetrieveHostId("\nEnter ID of the Host you want to add new Apartment to:", "Please enter valid Host ID again.");
        
        var newApartmentNumber = _io.ReadInt("\nEnter new Apartment number:", true);
        var newApartmentPrice = _io.ReadDouble("\nEnter new Apartment price (decimal point delimiter is a dot (e.g., \"120.20\"):", true);

        try
        {
            _hostsService.AddApartment(targetHostId, new CreateApartmentData { Number = newApartmentNumber, Price = newApartmentPrice });
            _io.Write($"New Apartment added successfully to Host with ID = {targetHostId}!");
        }
        catch (HostNotFoundException exception)
        {
            if (exception.HostId != null)
                _io.Write($"\nHost with with ID = {exception.HostId} not found!");
            else 
                _io.Write("\nSomething went wrong during adding new Apartment.");
        }
    }

    private void HandleSaveChangesAction()
    {
        if (_hostsService.AnyUnsavedChanges())
        {
            try
            {
                _hostsService.SaveChanges();
                _io.Write("\nChanges were saved successfully!");
            }
            catch (Exception exception)
            {
                _io.Write("Something went wrong during saving changes: " + exception.Message);
            }
            
            return;
        }
        
        _io.Write("\nNo changes to save yet.");
    }
    
    private void HandleExitAppAction()
    {
        _io.Write("\nExiting... See you next time!");
    }
    
    private void HandleInvalidActionInput()
    {
        _io.Write("\nInvalid number: a number for an action must be between 0 and 2.");
    }
}