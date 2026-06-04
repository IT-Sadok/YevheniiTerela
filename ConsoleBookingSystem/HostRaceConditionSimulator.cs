namespace ConsoleBookingSystem;

public class HostRaceConditionSimulator
{
    private IHostRepository _hostRepository;

    public HostRaceConditionSimulator(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
    }

    public async Task<bool> IncreasePriceForSharedApartment(int deltaPrice)
    {
        byte hostOneId = 14;
        byte hostTwoId = 5;
        byte hostThreeId = 8;
        byte hostFourId = 12;
        byte hostFiveId = 101;
        
        byte sharedApartmentId = 4;
        
        double initialSharedApartmentPrice = 560;
        
        var hostOneWithSharedApartment = _hostRepository.FindHostById(hostOneId);
        var hostTwoWithSharedApartment = _hostRepository.FindHostById(hostTwoId);
        var host3WithSharedApartment = _hostRepository.FindHostById(hostThreeId);
        var host4WithSharedApartment = _hostRepository.FindHostById(hostFourId);
        var host5WithSharedApartment = _hostRepository.FindHostById(hostFiveId);

        if (hostOneWithSharedApartment == null || hostTwoWithSharedApartment == null || host3WithSharedApartment == null || host4WithSharedApartment == null || host5WithSharedApartment == null)
        {
            throw new HostNotFoundException("Test Hosts with specified IDs are not found.");
        }
        
        // resetting apartment price to initial (once - cause apartment if a reference type variable here)
        UpdateApartmentPrice(initialSharedApartmentPrice, hostOneWithSharedApartment.Id, sharedApartmentId);
        
        var hostOneTask = Task.Run(() => UpdateApartmentPrice(deltaPrice, hostOneWithSharedApartment.Id, sharedApartmentId, true));
        var hostTwoTask = Task.Run(() => UpdateApartmentPrice(deltaPrice, hostTwoWithSharedApartment.Id, sharedApartmentId, true));
        var hostThreeTask = Task.Run(() => UpdateApartmentPrice(deltaPrice, host3WithSharedApartment.Id, sharedApartmentId, true));
        var hostFourTask = Task.Run(() => UpdateApartmentPrice(deltaPrice, host4WithSharedApartment.Id, sharedApartmentId, true));
        var hostFiveTask = Task.Run(() => UpdateApartmentPrice(deltaPrice, host5WithSharedApartment.Id, sharedApartmentId, true));
        
       await Task.WhenAll(hostOneTask, hostTwoTask,  hostThreeTask, hostFourTask, hostFiveTask);
       
       var expectedSharedApartmentPrice = initialSharedApartmentPrice + deltaPrice * 5;
       var finalSharedApartmentPrice = _hostRepository.FindApartmentById(hostOneId, sharedApartmentId)!.Price;
       var isRaceConditionOccurred = Math.Abs(finalSharedApartmentPrice - expectedSharedApartmentPrice) < 0.00001;
       
       Console.WriteLine($"{(isRaceConditionOccurred ? "" : "=========== Race Condition OCCURRED ==========")}");
       Console.WriteLine($"host #1 With Shared Apartment, apartment's id=4, Price = {_hostRepository.FindApartmentById(hostOneId, sharedApartmentId)?.Price}, expected Price is {expectedSharedApartmentPrice}");
       Console.WriteLine($"host #2 With Shared Apartment, apartment's id=4, Price = {_hostRepository.FindApartmentById(hostTwoId, sharedApartmentId)?.Price}, expected Price is {expectedSharedApartmentPrice}");
       Console.WriteLine($"host #3 With Shared Apartment, apartment's id=4, Price = {_hostRepository.FindApartmentById(hostThreeId, sharedApartmentId)?.Price}, expected Price is {expectedSharedApartmentPrice}");
       Console.WriteLine($"host #4 With Shared Apartment, apartment's id=4, Price = {_hostRepository.FindApartmentById(hostFourId, sharedApartmentId)?.Price}, expected Price is {expectedSharedApartmentPrice}");
       Console.WriteLine($"host #5 With Shared Apartment, apartment's id=4, Price = {_hostRepository.FindApartmentById(hostFiveId, sharedApartmentId)?.Price}, expected Price is {expectedSharedApartmentPrice}");
       Console.WriteLine($"{(isRaceConditionOccurred ? "" : "=========== Race Condition OCCURRED ==========")}");
       Console.WriteLine();
       Console.WriteLine();
       Console.WriteLine();
       
       return true;
    }

    private void UpdateApartmentPrice(double price, int hostId, int apartmentId, bool toIncreasePrice = false)
    {
        var apartment = _hostRepository.FindApartmentById(hostId, apartmentId);
        var newPrice = toIncreasePrice ? price + apartment!.Price : price;
        
        _hostRepository.UpdateApartment(hostId, apartmentId, new UpdateApartmentData { Price = newPrice });
    }
}