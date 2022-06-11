
using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;

public class CheapestTravelFinder : ICheapestTravelFinder
{
    private readonly ITravelGraphBuildEngine _graphEngine;

    public CheapestTravelFinder(ITravelGraphBuildEngine graphEngine)
    {
        _graphEngine = graphEngine ?? throw new ArgumentNullException(nameof(graphEngine));
    }

    public Result<SortedDictionary<string, decimal>> FindShortestPath(Location startingPoint, Location destination, List<Travel> travels)
    {
        var (startPointExists, destinationExists) = CheckLocations(travels, startingPoint, destination);

        if (startPointExists is false || destinationExists is false)
            return LocationNotFound(startPointExists);

        var graph = MakeGraph(travels);
        var nodes = graph.Nodes;

        var source = nodes[startingPoint];

        var unvisited = new PriorityQueue<Node, decimal>();
        var distance = new SortedDictionary<string, decimal>();

        unvisited.Enqueue(source, 0);
        distance.Add(source.Name, 0);

        foreach (var (key, node) in nodes)
        {
            if (node != source)
            {
                distance.Add(key, decimal.MaxValue);
                unvisited.Enqueue(node, distance[node.Name]);
            }
        }

        while (unvisited.Count > 0)
        {
            var node = unvisited.Dequeue();

            foreach (var link in node.Links)
            {
                var alt = distance[node.Name] + link.Weight;

                if (alt < distance[link.Destination])
                {
                    distance[link.Destination] = alt;
                }
            }
        }

        return Result<SortedDictionary<string, decimal>>.Success(distance);
    }

    private static Result<SortedDictionary<string, decimal>> LocationNotFound(bool startPointExists)
    {
        if (startPointExists is false)
            return Result<SortedDictionary<string, decimal>>.Fail("STARTING_POINT_NOT_EXISTS", "Starting point not found in travel list");

        return Result<SortedDictionary<string, decimal>>.Fail("DESTINATION_NOT_EXISTS", "Destination not found in travel list");
    }

    private (bool, bool) CheckLocations(IEnumerable<Travel> travels, Location startingPoint, Location destination)
    {
        var startPointExists = false;
        var destinationExists = false;

        var exists = travels.Any(x =>
        {
            var existsStartingPoint = x.Connection.StartingPoint == startingPoint;
            var existsDestination = x.Connection.Destination == destination;

            if (existsStartingPoint)
                startPointExists = true;

            if (existsDestination)
                destinationExists = true;

            return startPointExists && destinationExists;
        });

        return (startPointExists, destinationExists);
    }


    private DirectedGraph MakeGraph(List<Travel> travels)
    {
        var result = _graphEngine.BuildGraph(travels);

        if (result.IsFailed)
            return DirectedGraph.Null;

        return result.Value;
    }
}
