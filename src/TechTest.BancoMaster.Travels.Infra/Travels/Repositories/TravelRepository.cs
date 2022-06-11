using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;
using TechTest.BancoMaster.Travels.Infra.Shared;

namespace TechTest.BancoMaster.Travels.Infra.Travels.Repositories;

public class TravelRepository : BaseInMemoryRepository<Travel, string>, ITravelRepository
{
    private Dictionary<string, Travel> _travels;

    public TravelRepository(ILogger<TravelRepository> logger, Dictionary<string, Travel> travels) : base(logger)
    {
        _travels = travels ?? throw new ArgumentNullException(nameof(travels));
    }

    protected override Dictionary<string, Travel> Data => _travels;

    protected override Travel NullValue => Travel.Null;

    public Task<IEnumerable<Travel>> GetConnectionLocations(Location startingPoint, Location destination) =>
        GetWhereAsync(x => x.Id.Contains(startingPoint) || x.Id.Contains(destination));
}

