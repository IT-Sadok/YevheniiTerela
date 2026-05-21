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
        return File.Exists(_filePath) 
            ? DeserializeData(File.ReadAllText(_filePath))
            : default;
    }
    
    /// <inheritdoc />
    public void WriteData(T data)
    {
        File.WriteAllText(_filePath, SerializeData(data));
    }

    private string SerializeData(T data, JsonSerializerOptions? options = null)
    {
        return JsonSerializer.Serialize(data, options);
    }
    
    private T? DeserializeData(string data)
    {
        return JsonSerializer.Deserialize<T>(data);
    }
}