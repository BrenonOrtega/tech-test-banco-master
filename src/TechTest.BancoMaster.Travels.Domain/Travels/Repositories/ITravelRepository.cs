using Awarean.Sdk.SharedKernel;

namespace TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

public interface ITravelRepository : IQueryRepository<Travel, string>, ICommandRepository<Travel, string>
{
    Task<IEnumerable<Travel>> GetConnectionLocations(Location startingPoint, Location destination);
    Task<IEnumerable<Travel>> GetTravelsAsync(int offset = 0, int size = 100);
}
