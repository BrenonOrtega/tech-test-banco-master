
using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;

public class CheapestTravelFinder : ICheapestTravelFinder
{
    private readonly ITravelGraphBuildEngine _graphEngine;

    Dictionary<string, Node> _nodeDict = new();
    List<Link> _routes = new();
    HashSet<string> _unvisited = new();

    public CheapestTravelFinder(ITravelGraphBuildEngine graphEngine) => 
        _graphEngine = graphEngine ?? throw new ArgumentNullException(nameof(graphEngine));

    public Result<LinkedList<(string Location, decimal CostFromSource)>> FindShortestPath(Location startingPoint, Location destination, List<Travel> travels)
    {
        var (startPointExists, destinationExists) = CheckLocations(travels, startingPoint, destination);

        if (startPointExists is false || destinationExists is false)
            return LocationNotFound(startPointExists);

        var graph = MakeGraph(travels);
        InitGraph(graph);

        // StartinPoint distance to self.
        _nodeDict[startingPoint].Weight = 0;

        var queue = new NodePriorityQueue();
        queue.AddNodeWithPriority(_nodeDict[startingPoint]);

        CheckNode(queue, destination);

        var path = FindShortestPath(startingPoint, destination);
        return Result<LinkedList<(string Location, decimal CostFromSource)>>.Success(path);
    }

    private LinkedList<(string Location, decimal CostFromSource)> FindShortestPath(string startNode, string destNode)
    {
        var pathList = new LinkedList<(string Location, decimal CostFromSource)>();

        pathList.AddLast((_nodeDict[destNode].Name, _nodeDict[destNode].Weight));

        Node currentNode = _nodeDict[destNode];

        while (currentNode != _nodeDict[startNode])
        {
            pathList.AddFirst((currentNode.Previous.Name, currentNode.Previous.Weight));
            currentNode = currentNode.Previous;
        }

        return pathList;
    }

    private void CheckNode(NodePriorityQueue queue, Location destination)
    {
        if (queue.Count == 0)
            return;

        foreach (var route in _routes.FindAll(r => r.StartingPoint == queue.First.Value.Name))
        {
            if (!_unvisited.Contains(route.Destination))
                continue;

            var travelledDistance = _nodeDict[queue.First.Value.Name].Weight + route.Weight;

            if (travelledDistance < _nodeDict[route.Destination].Weight)
            {
                _nodeDict[route.Destination].Weight = travelledDistance;
                _nodeDict[route.Destination].Previous = _nodeDict[queue.First.Value.Name];
            }

            if (!queue.HasLetter(route.Destination))
                queue.AddNodeWithPriority(_nodeDict[route.Destination]);
        }

        _unvisited.Remove(queue.First.Value.Name);
        queue.RemoveFirst();

        CheckNode(queue, destination);
    }

    private void InitGraph(DirectedGraph graph)
    {
        foreach (var (key, node) in graph.Nodes)
        {
            _nodeDict.Add(node.Name, node);
            _unvisited.Add(node.Name);

            foreach (var link in node.Links)
                _routes.Add(link);
        }
    }

    private static Result<LinkedList<(string, decimal)>> LocationNotFound(bool startPointExists)
    {
        if (startPointExists is false)
            return Result<LinkedList<(string, decimal)>>.Fail("STARTING_POINT_NOT_EXISTS", "Starting point not found in travel list");

        return Result<LinkedList<(string, decimal)>>.Fail("DESTINATION_NOT_EXISTS", "Destination not found in travel list");
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
