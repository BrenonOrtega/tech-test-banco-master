using Awarean.Sdk.Result;

namespace TechTest.BancoMaster.Travels.Domain.Travels;

public interface ITravelService 
{
    Task<Result<IEnumerable<Travel>>> GetByStartingPointAsync(Location startingPoint);
    Task<Result<IEnumerable<Travel>>> GetByDestinationAsync(Location destination);
    Task<Result<Travel>> GetTravelAsync(Location startingPoint, Location destination);
    Task<Result<IEnumerable<Travel>>> GetCheapestPathAsync(Location startingPoint, Location destination);
}