using Awarean.Sdk.SharedKernel.Delegates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Infra.Travels.Repositories;

public class TravelRepository : ITravelRepository
{
    private readonly TravelsContext _context;
    private readonly ILogger<TravelRepository> _logger;

    public TravelRepository(ILogger<TravelRepository> logger, TravelsContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task DeleteAsync(string id)
    {
        var model = _context.Travels.FirstOrDefault(x => x.Id == id);

        if (model is not null)
            _context.Travels.Remove(model);
    }

    public async Task<Travel> GetByIdAsync(string id) => _context.Travels.AsNoTracking().SingleOrDefault(x => x.Id == id) ?? Travel.Null;

    public async Task<IEnumerable<Travel>> GetConnectionLocations(Location startingPoint, Location destination)
    {
        var firstQuery = await _context.Travels.Where(x => x.Id.Contains(startingPoint) || x.Id.Contains(destination)).ToListAsync();
        var locations = firstQuery.SelectMany(x => new[] { x.StartingPoint, x.Destination }).ToHashSet();

        var queried = await _context.Travels
            .Where(x => locations.Contains(x.StartingPoint) || locations.Contains(x.Destination))
            .Select(x => x.ToDomainEntity())
            .ToListAsync();

        return queried;
    }

    public async Task<IEnumerable<Travel>> GetTravelsAsync(int offset = 0, int size = 100) => await _context.Travels
        .Skip(offset * size)
        .Take(size)
        .Select(x => x.ToDomainEntity())
        .ToListAsync();

    public async Task<IEnumerable<Travel>> GetWhereAsync(GetWhereSelector<Travel> filter) => await _context.Travels
        .Where(x => filter(x)).Select(x => x.ToDomainEntity()).ToListAsync();

    public async Task<string> SaveAsync(Travel entity)
    {
        _context.Travels.Add(entity);
        await _context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task UpdateAsync(string id, Travel updatedEntity)
    {
        var queriedModel = _context.Travels
            .Where(x => x.Id == id)
            .Single();

        if (queriedModel is not null )
        {
            queriedModel.NewAmount(updatedEntity.Amount, nameof(TravelRepository));

            await _context.SaveChangesAsync();
        }
    }
}
