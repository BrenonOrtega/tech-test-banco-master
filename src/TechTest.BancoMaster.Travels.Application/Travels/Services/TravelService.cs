using Awarean.Sdk.Result;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Contracts;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Application.Travels.Services;

internal class TravelService : ITravelService
{
    private readonly ILogger<TravelService> _logger;
    private readonly ITravelRepository _repository;
    private readonly ICheapestTravelFinder _finder;

    public TravelService(ILogger<TravelService> logger, ITravelRepository repository, ICheapestTravelFinder finder)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _finder = finder ?? throw new ArgumentNullException(nameof(finder));
    }

    public async Task<Result<IEnumerable<Travel>>> GetByDestinationAsync(Location destination)
    {
        var query = (await _repository.GetWhereAsync(x => x.Id.EndsWith(destination))).ToList();

        if (query.Count == 0)
        {
            _logger.LogInformation("Found {travelCount} for destination {destination}", query.Count, destination);
            return Result<IEnumerable<Travel>>.Fail(ApplicationErrors.TravelNotFound);
        }

        return Result<IEnumerable<Travel>>.Success(query);
    }

    public async Task<Result<IEnumerable<Travel>>> GetByStartingPointAsync(Location startingPoint)
    {
        var query = (await _repository.GetWhereAsync(x => x.Id.StartsWith(startingPoint))).ToList();

        if (query.Count == 0)
        {
            _logger.LogInformation("Found {travelCount} for startingPoint {startingPoint}", query.Count, startingPoint);

            return Result<IEnumerable<Travel>>.Fail(ApplicationErrors.TravelNotFound);
        }

        return Result<IEnumerable<Travel>>.Success(query);
    }

    public async Task<Result<Travel>> GetTravelAsync(Location startingPoint, Location destination)
    {
        var query = (await _repository.GetByIdAsync($"{startingPoint}#{destination}"));

        return query == Travel.Null
            ? Result<Travel>.Fail(ApplicationErrors.TravelNotFound)
            : Result<Travel>.Success(query);
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
            return Result<ICheapestTravelResponse>.Success(new CheapestTravelResponse(startingPoint, destination, result.Value.Last.Value.CostFromSource, result.Value));

        return Result<ICheapestTravelResponse>.Fail(result.Error);
    }

    public async Task<Result<string>> AddTravelAsync(string startingPoint, string destination, decimal amount)
    {
        try
        {
            var travel = new Travel(startingPoint, destination, amount);
            var id = await _repository.SaveAsync(travel);
            _logger.LogInformation("Added Travel to records - Travel Data {travel}", JsonSerializer.Serialize(travel));
            return Result<string>.Success(id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed adding Travel to records - Travel Data {travel} - Exception {exception}",
                JsonSerializer.Serialize(new { startingPoint, destination, amount }), ex);
            return Result<string>.Fail(ApplicationErrors.FailedAddingTravel);
        }
    }

    public async Task<Result> UpdateTravelAsync(string startingPoint, string destination, decimal amount)
    {
        var actual = await _repository.GetByIdAsync($"{startingPoint}#{destination}");

        if(actual is not null && actual != Travel.Null)
        {
            var canUpdate = actual.Connection.StartingPoint == startingPoint && actual.Connection.Destination == destination;

            if(canUpdate)
            {
                actual = new Travel(startingPoint, destination, amount);
                await _repository.UpdateAsync(actual.Id, actual);
                return Result.Success();
            }

            return Result.Fail(ApplicationErrors.CannotChangeTravelsName);
        }

        return Result.Fail(ApplicationErrors.TravelNotFound);
    }

    public async Task<Result> DeleteTravelAsync(string travelId)
    {
        try
        {
            await _repository.DeleteAsync(travelId);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed deleting Travel from records - Travel Id {travelId} - Exception {exception}", travelId, ex);
            return Result.Fail(ApplicationErrors.FailedDeletingTravel);
        }
    }
}