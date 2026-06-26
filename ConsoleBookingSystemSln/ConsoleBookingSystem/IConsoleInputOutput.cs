namespace ConsoleBookingSystem;

public interface IConsoleInputOutput
{
    public void Write(string message);
    public int ReadInt(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.");
    public int ReadIntFromKey(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.");
    public void RequireAnyKey(string message = "");
    public T ReadEnumValue<T>(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.") where T : struct;
    public string ReadString(string message, bool retryOnEmptyString = false, string retryMessage = "Input is empty, please enter a valid string.");
    public ConsoleKey ReadPressKey(string message = "");
    public double ReadDouble(string message, bool retryOnInvalid = false, string retryMessage = "Invalid input. Please try again.");
}