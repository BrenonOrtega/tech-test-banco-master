using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Contracts;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Application.Travels.Services;

internal class TravelService : ITravelService
{
    private readonly ILogger<ITravelService> _logger;
    private readonly ITravelRepository _repository;
    private readonly ICheapestTravelFinder _finder;

    public TravelService(ILogger<ITravelService> logger, ITravelRepository repository, ICheapestTravelFinder finder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
    }

    public Task<Result<IEnumerable<Travel>>> GetByDestinationAsync(Location destination)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<Travel>>> GetByStartingPointAsync(Location startingPoint)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Travel>> GetTravelAsync(Location startingPoint, Location destination)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<Travel>>> GetTravelsAsync(int offset = 0, int size = 100)
    {
        var travels = (await _repository.GetTravelsAsync(offset, size)).ToList();

        return Result<IEnumerable<Travel>>.Success(travels);
    }

    public async Task<Result<ICheapestTravelResponse>> GetCheapestPathAsync(Location startingPoint, Location destination)
    {
        var travels = (await _repository.GetConnectionLocations(startingPoint, destination)).ToList();
        var result = _finder.FindShortestPath(startingPoint, destination, travels);

        if (result.IsSuccess)
            return Result<ICheapestTravelResponse>.Success(new CheapestTravelResponse(startingPoint, destination, result.Value[destination], new()));

        return Result<ICheapestTravelResponse>.Fail(result.Error);
    }
}

