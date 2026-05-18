namespace ConsoleBookingSystem;

public interface IHostFileStorage
{
    public List<Host> ReadData();
    public void WriteData(List<Host> hosts);
}