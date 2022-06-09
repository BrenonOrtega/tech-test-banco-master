using Awarean.Sdk.SharedKernel;

namespace TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

public interface ITravelConnectionRepository : IQueryRepository<TravelConnection, string>, ICommandRepository<TravelConnection, string>
{
    Task<IEnumerable<TravelConnection>> GetConnectionLocations(Location startingPoint, Location destination);
}
