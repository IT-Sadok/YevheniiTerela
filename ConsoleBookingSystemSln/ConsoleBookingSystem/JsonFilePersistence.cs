using System.Security;
using System.Text.Json;

namespace ConsoleBookingSystem;

public class JsonFilePersistence<T> : IPersistence<T>
{
    private readonly string _filePath;
    
    public JsonFilePersistence(string filePath)
    {
        _filePath = filePath;
    }
    
    /// <inheritdoc />
    public T? ReadData()
    {
        string serializedJsonString;

        try
        {
            if (!File.Exists(_filePath))
            {
                return default;
            }

            serializedJsonString = File.ReadAllText(_filePath);
        }
        catch (Exception e)
        {
            throw ThrowFileException(e);
        }
        
        try
        {
            return string.IsNullOrEmpty(serializedJsonString) ? default : DeserializeData(serializedJsonString);
        }
        catch (Exception exception)
        {
            throw ThrowJsonException(exception);
        }
    }
    
    /// <inheritdoc />
    public void WriteData(T data)
    {
        string serializedJsonString;

        try
        {
            serializedJsonString = SerializeData(data);
        }
        catch (Exception exception)
        {
            throw ThrowJsonException(exception);
        }
        
        try
        {
            File.WriteAllText(_filePath, serializedJsonString);
        }
        catch (Exception exception)
        {
            throw ThrowFileException(exception);
        }
    }

    private string SerializeData(T data, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(data, options);
    }
    
    private T? DeserializeData(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }

    private PersistenceException ThrowFileException(Exception exception)
    {
        if (exception is ArgumentException or ArgumentNullException or PathTooLongException or NotSupportedException)
            return new PersistenceException("Provided path to the file is in invalid format", exception); 
        
        if (exception is DirectoryNotFoundException or FileNotFoundException)
            return new PersistenceException("No file found under provided path", exception);

        if (exception is UnauthorizedAccessException or SecurityException)
            return new PersistenceException("No access to specified directory or file", exception);
        
        return new PersistenceException("Error occured during file loading / saving from / into persistence storage", exception);
    }


    private PersistenceException ThrowJsonException(Exception exception)
    {
        if (exception is NotSupportedException)
            return new PersistenceException("Serialization/deserialization is not supported for provided T data type", exception);
        
        if (exception is ArgumentNullException or JsonException)
            return new PersistenceException("Invalid JSON string provided for deserialization", exception);
 
        return new PersistenceException("Error occurred during JSON serialization/deserialization", exception);
    }
}