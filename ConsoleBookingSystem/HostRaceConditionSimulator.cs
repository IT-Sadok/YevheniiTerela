namespace ConsoleBookingSystem;

public class HostRaceConditionSimulator
{
    private IHostRepository _hostRepository;
    private static SemaphoreSlim _semaphore;

    public HostRaceConditionSimulator(IHostRepository hostRepository)
    {
        _hostRepository = hostRepository;
        _semaphore = new SemaphoreSlim(1, 1);
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
        
        // resetting shared Apartment's price to initial (once - cause apartment if a reference type variable here)
        await UpdateApartmentPrice(initialSharedApartmentPrice, hostOneWithSharedApartment.Id, sharedApartmentId);
        
        var hostOneTask = Task.Run(async () =>
        {
            await _semaphore.WaitAsync();
            try { await UpdateApartmentPrice(deltaPrice, hostOneWithSharedApartment.Id, sharedApartmentId, true); }
            finally { _semaphore.Release(); }
        } );
        var hostTwoTask = Task.Run(async () =>
        {
             await _semaphore.WaitAsync();
            try { await UpdateApartmentPrice(deltaPrice, hostTwoWithSharedApartment.Id, sharedApartmentId, true); }
            finally { _semaphore.Release(); }
        });
        var hostThreeTask = Task.Run(async () =>
        {
            await _semaphore.WaitAsync();
            try { await UpdateApartmentPrice(deltaPrice, host3WithSharedApartment.Id, sharedApartmentId, true); }
            finally { _semaphore.Release(); }
        });
        var hostFourTask = Task.Run(async () =>
        {
            await _semaphore.WaitAsync();
            try { await UpdateApartmentPrice(deltaPrice, host4WithSharedApartment.Id, sharedApartmentId, true); }
            finally { _semaphore.Release(); }
        });
        var hostFiveTask = Task.Run(async () =>
        {
            await _semaphore.WaitAsync();
            try { await UpdateApartmentPrice(deltaPrice, host5WithSharedApartment.Id, sharedApartmentId, true); }
            finally { _semaphore.Release(); }
        });
        
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

    private async Task UpdateApartmentPrice(double price, int hostId, int apartmentId, bool toIncreasePrice = false)
    {
        var apartment = _hostRepository.FindApartmentById(hostId, apartmentId);
        var newPrice = toIncreasePrice ? price + apartment!.Price : price;

        // mocks some async work (e.g. I/O, HTTP query) so lock unusable here - SemaphoreSlim required instead
        await Task.Yield();
        
        _hostRepository.UpdateApartment(hostId, apartmentId, new UpdateApartmentData { Price = newPrice });
    }
}