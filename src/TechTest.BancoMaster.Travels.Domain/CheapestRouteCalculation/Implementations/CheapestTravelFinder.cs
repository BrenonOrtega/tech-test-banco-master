
using Awarean.Sdk.Result;
using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.Extensions;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public class CheapestTravelFinder
{
    private readonly Dictionary<Location, Dictionary<Location, LocationVertex>> _visited = new();
    private readonly List<LocationVertex> _unvisited = new();

    private ITravelGraphBuildEngine _graphEngine;
    private readonly Action<string> _log = (string msg) => Console.WriteLine(msg);

    public CheapestTravelFinder(ITravelGraphBuildEngine graphEngine, Action<string> log = null)
    {
        _graphEngine = graphEngine ?? throw new ArgumentNullException(nameof(graphEngine));
        _log = log ?? new Action<string>((msg) => Console.WriteLine(msg));
    }

    public object FindShortestPath(LocationNode source, LocationNode destination, DirectedGraph<LocationNode, decimal> graph)
    {
        CheckNulls(source, destination, graph);

        var path = new LinkedList<(LocationNode vertex, decimal cost)>();
        path.AddFirst((source, 0));

        foreach (var vertex in graph.Nodes)
        {
            var queue = new PriorityQueue<Link<LocationNode, decimal>, Money>();

            foreach (var link in vertex.Links)
            {
                queue.Enqueue(link, link.Weight);
            }

            var lessExpensiveLink = queue.Dequeue();

            var actual = path.AddLast((lessExpensiveLink.Destination, lessExpensiveLink.Weight));

        }

        throw new NotImplementedException();
    }

    public Result FindShortestPath(Location startingPoint, Location destination, List<Travel> travels)
    {
        var graph = MakeGraph(travels);

        if (graph.Nodes.Any(x => x.Source == startingPoint) is false)
            return Result.Fail("STARTING_POINT_NOT_EXISTS", "Starting Point not found in travel list");

        if(graph.Nodes.Any(node => node.Links.Any(link => link.Destination == destination)) is false)
            return Result.Fail("DESTINATION_NOT_EXISTS", "Destination not found in travel list");

        var source = new LocationVertex(startingPoint);

        _visited.Add(startingPoint, new() { { startingPoint, source } });


        foreach (var node in graph.Nodes)
        {
            InitializeNode(node);
            AddLinks(node);
        }
        
        // NEED TO FIX SUMMING TOTAL WEIGHTS WHEN ADDING TO DICTIONARY.
        LET IT BREAK HERE.
        

        return Result.Fail("FAILED", "FAILED, STILL NOT IMPLEMENTED CORRECTLY");
    }

    private void AddLinks(Node<Location, decimal> node)
    {
        foreach(var link in node.Links)
        {
            _visited[node.Source].Add(link.Destination, new LocationVertex(node.Source, link.Destination, link.Weight));
        }
    }

    private void InitializeNode(Node<Location, decimal> node)
    {
        if (_visited.ContainsKey(node.Source))
            return;

        _visited.Add(node.Source, new());
    }

    private DirectedGraph<Location, decimal> MakeGraph(List<Travel> travels)
    {
        var result = _graphEngine.BuildGraph(travels);

        if (result.IsFailed)
            return DirectedGraph<Location, decimal>.Null;

        return result.Value;
    }

    private void CheckNulls(params object[] args)
    {
        foreach (var arg in args)
            ArgumentNullException.ThrowIfNull(arg);
    }

    public class LocationVertex : Vertex<Location>
    {
        public LocationVertex(Location Actual, Location Predecessor, decimal totalCost) : base(Actual, Predecessor, totalCost) { }
        public LocationVertex(Location actual) : base(actual) { }
    }

    public class Vertex<TNode>
    {
        public Vertex(TNode actual) : this (actual, default, 0) { }
        public Vertex(TNode actual, TNode predecessor, decimal totalCost)
        {
            Actual = actual;
            Predecessor = predecessor;
            TotalCost = totalCost;
        }

        public TNode Actual { get; }
        public TNode Predecessor { get; private set; }
        public decimal TotalCost { get; }

        public bool IsSource() => TotalCost == 0;
    }
}
