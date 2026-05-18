using System.Text.Json;

namespace ConsoleBookingSystem;

public class JsonHostFileStorage : IHostFileStorage
{
    private readonly string _filePath = "hosts.json";
    
    public List<Host> ReadData()
    {
        return DeserializeHosts() ?? [];
    }

    public void WriteData(List<Host> hosts)
    {
        File.WriteAllText(_filePath, SerializeHosts(hosts));
    }

    private string SerializeHosts(List<Host> hosts)
    {
        return JsonSerializer.Serialize<List<Host>>(
            hosts, 
            new JsonSerializerOptions { WriteIndented = true } // leaving new-lines and spaced intentionally for more comfortable reading
        );
    }
    
    private List<Host>? DeserializeHosts()
    {
        return File.Exists(_filePath) 
            ? JsonSerializer.Deserialize<List<Host>>(File.ReadAllText(_filePath))
            : null;
    }
}