namespace ConsoleBookingSystem;

public interface IPersistence<T>
{
    /// <summary>
    /// Loads persisted data as T.
    /// </summary>
    public T? ReadData();
    /// <summary>
    /// Saves data to persistent storage.
    /// </summary>
    public void WriteData(T data);
}