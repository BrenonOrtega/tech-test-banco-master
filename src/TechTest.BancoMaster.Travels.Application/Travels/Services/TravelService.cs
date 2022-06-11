using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Application.Travels.Services;

internal class TravelService : ITravelService
{
    private readonly ILogger<ITravelService> _logger;
    private readonly ITravelRepository _repository;

    public TravelService(ILogger<ITravelService> logger, ITravelRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<Result<IEnumerable<Travel>>> GetByDestinationAsync(Location destination)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Travel>>> GetByStartingPointAsync(Location startingPoint)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Travel>>> GetCheapestPathAsync(Location startingPoint, Location destination)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Travel>> GetTravelAsync(Location startingPoint, Location destination)
    {
        throw new NotImplementedException();
    }
}

