using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Domain.RouteCalculation;

public class CheapestTravelSearchEngine : ICheapestTravelSearchEngine
{
    private readonly ITravelConnectionRepository _repository;
    private readonly Action<string> _log;

    public CheapestTravelSearchEngine(ITravelConnectionRepository repository, Action<string> log)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _log = log ?? EmptyLog;
    }

    public async Task<Result<ICheapestTravelResponse>> SearchAsync(ISearchTravelCommand command)
    {
        var enumerableVertexes = await _repository.GetConnectionLocations(command.From, command.To);
        var vertexes = enumerableVertexes.ToList();

        _log($"Found a total of { vertexes.Count } - {string.Join(",", vertexes.Select(x => x.Connection.FromTo))}");

        var graph = new Graph<TravelConnection>(vertexes);

        return Result<ICheapestTravelResponse>.Fail("STILL_NOT_IMPLEMENTED", $"This method is not implemented yet {nameof(CheapestTravelSearchEngine)}.{nameof(SearchAsync)}");
    }

    private static void EmptyLog(string message) { }
}
