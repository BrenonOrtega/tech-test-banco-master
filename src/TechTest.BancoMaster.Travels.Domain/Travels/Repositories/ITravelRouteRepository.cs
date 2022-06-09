using Awarean.Sdk.SharedKernel;

namespace TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

public interface ITravelConnectionRepository : IQueryRepository<Travel, string>, ICommandRepository<Travel, string>
{
    Task<IEnumerable<Travel>> GetConnectionLocations(Location startingPoint, Location destination);
}
