namespace TechTest.BancoMaster.Travels.Domain.Travels;

public interface ITravelService 
{
    Task<IEnumerable<Travel>> GetByStartingPointAsync(Location startingPoint);
    Task<IEnumerable<Travel>> GetByDestinationAsync(Location destination);
    Task<IEnumerable<Travel>> GetCheapestPathAsync(Location startingPoint, Location destination);
}