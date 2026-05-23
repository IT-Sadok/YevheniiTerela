namespace ConsoleBookingSystem;

public interface IPersistence<T>
{
    /// <summary>
    /// Loads persisted data as T.
    /// <exception cref="PersistenceException">
    /// Is thrown during load data from persistent storage and converting it to T
    /// </exception>
    /// </summary>
    public T? ReadData();
    /// <summary>
    /// Saves data to persistent storage.
    /// <exception cref="PersistenceException">
    /// /// Is thrown during writing T data from persistent storage
    /// </exception>
    /// </summary>
    public void WriteData(T data);
}