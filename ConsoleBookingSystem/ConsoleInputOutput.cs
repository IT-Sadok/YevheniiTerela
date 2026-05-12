namespace ConsoleBookingSystem;

public class ConsoleInputOutput
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
}