namespace ConsoleBookingSystem;

public class ConsoleInputOutput : IConsoleInputOutput
{
    public void Write(string message)
    {
        Console.WriteLine(message);
    }
    
    
    public int ReadInt(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.")
    {
        Write(message);
        
        int result;
        bool isParsed;

        while (true)
        {
            isParsed = int.TryParse(Console.ReadLine(), out result);
            
            if (!isParsed)
            {
                Write(retryMessage);
                if (retryOnInvalid) continue;
            }
            
            break;
        } 
        
        return result;
    }


    public int ReadIntFromKey(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.")
    {
        Write(message);
        
        int result;
        bool isParsed;

        while (true)
        {
            isParsed = int.TryParse(Console.ReadKey().KeyChar.ToString(), out result);
            
            if (!isParsed)
            {
                Write(retryMessage);
                if (retryOnInvalid) continue;
            }
            
            break;
        } 
        
        return result;
    }
    
    
    public void RequireAnyKey(string message = "")
    {
        Write(message);
        Console.ReadKey();
    }


    public T ReadEnumValue<T>(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.") where T : struct
    {
        Write(message);
        
        T returnValue;
        bool isParsed;

        while (true)
        {
            isParsed = Enum.TryParse<T>(Console.ReadKey().KeyChar.ToString(), out returnValue);
            
            if (!isParsed)
            {
                Write(retryMessage);
                if (retryOnInvalid) continue;
            }
            
            break;
        }
        
        return returnValue;
    }


    public string ReadString(string message, bool retryOnEmptyString = false, string retryMessage = "Input is empty, please enter a valid string.")
    {
        Write(message);

        string input;

        while (true)
        {
            input = Console.ReadLine() ?? string.Empty;
            if (string.IsNullOrEmpty(input))
            {
                Write(retryMessage);
                if (retryOnEmptyString) continue;
            }
            
            break;
        }
        
        return input;
    }
}