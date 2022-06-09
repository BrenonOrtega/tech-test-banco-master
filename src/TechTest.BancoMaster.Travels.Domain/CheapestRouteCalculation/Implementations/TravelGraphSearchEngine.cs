using Awarean.Sdk.Result;
using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.Extensions;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public class TravelGraphSearchEngine : ITravelGraphEngine
{
    private readonly ITravelConnectionRepository _repository;
    private readonly ITravelGraphBuilder _graphBuilder;
    private readonly ITravelNodeBuilder _nodeBuilder;
    private readonly Action<string> _log;
    private static void EmptyLog(string message) { }

    public TravelGraphSearchEngine(ITravelConnectionRepository repository, ITravelGraphBuilder graphBuilder, ITravelNodeBuilder nodeBuilder, Action<string> log)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _graphBuilder = graphBuilder ?? throw new ArgumentNullException(nameof(graphBuilder));
        _nodeBuilder = nodeBuilder ?? throw new ArgumentNullException(nameof(nodeBuilder));
        _log = log ?? EmptyLog;
    }

    public async Task<Result<DirectedGraph<Location, decimal>>> MakeDirectedGraphAsync(ISearchTravelCommand command)
    {
        try
        {
            var query = await _repository.GetConnectionLocations(command.From, command.To);

            if (query == Enumerable.Empty<Travel>())
                return Result<DirectedGraph<Location, decimal>>.Fail("QUERY_FAILED", "Could not retrieve travels to these locations");

            var travels = query.ToList();
            var locations = GetAllLocations(travels);

            _log($"Found a total of {travels.Count} - { travels.FormatStringFor(x => x.Connection.FromTo) }");

            var graph = BuildGraph(travels, locations);

            return Result<DirectedGraph<Location, decimal>>.Success(graph);
        } 
        catch(Exception ex)
        {
            return HandleException(ex);
        }
    }

    private DirectedGraph<Location, decimal> BuildGraph(List<Travel> travels, HashSet<Location> locations)
    {
        _log($"Building Graph for {locations.ToFormatString()}");
        foreach (var location in locations)
        {
            var nodeLinks = travels
                .Where(x => x.Connection.StartingPoint == location)
                .Select(x => (x.Connection.Destination, x.Amount));

            _nodeBuilder.Create(location);

            var node = LinkAndBuildNode(nodeLinks);
            _graphBuilder.AddNode(node);
        }

        var graph = _graphBuilder.Build();

        return graph;
    }

    private Node<Location, decimal> LinkAndBuildNode(IEnumerable<(Location destination, Money weight)> nodeLinks)
    {
        foreach (var (destination, weight) in nodeLinks)
            _nodeBuilder.LinkTo(destination, weight);

        var node = _nodeBuilder.Build();
        _log($"Graph's node Build for location: {node.Id} containing {node.Links.Count} links to other places - { node.Links.ToFormatString() }");

        return node;
    }

    private static HashSet<Location> GetAllLocations(List<Travel> travels)
    {
        var locations = new HashSet<Location>();
        var connections = travels.Select(x => x.Connection);
        foreach (var connection in connections)
        {
            locations.Add(connection.StartingPoint);
            locations.Add(connection.Destination);
        }

        return locations;
    }

    private Result<DirectedGraph<Location, decimal>> HandleException(Exception ex)
    {
        var message = $"Failed trying to build graph for travel route, exception happened : {ex}";
        _log(message);
        return Result<DirectedGraph<Location, decimal>>.Fail("GRAPH_BUILDING_ERROR", message);
    }
}
