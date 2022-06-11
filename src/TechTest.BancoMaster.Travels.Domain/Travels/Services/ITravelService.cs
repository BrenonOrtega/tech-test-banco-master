using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;

namespace TechTest.BancoMaster.Travels.Domain.Travels;

public interface ITravelService 
{
    Task<Result<IEnumerable<Travel>>> GetByStartingPointAsync(Location startingPoint);
    Task<Result<IEnumerable<Travel>>> GetByDestinationAsync(Location destination);
    Task<Result<Travel>> GetTravelAsync(Location startingPoint, Location destination);
    Task<Result<ICheapestTravelResponse>> GetCheapestPathAsync(Location startingPoint, Location destination);
    Task<Result<IEnumerable<Travel>>> GetTravelsAsync(int offset=0, int size=100);
}