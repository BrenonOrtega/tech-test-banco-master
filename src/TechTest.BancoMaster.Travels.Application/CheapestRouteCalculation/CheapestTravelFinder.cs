
using Awarean.Sdk.Result;
using Awarean.Sdk.ValueObjects;
using System.Collections.Immutable;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Models;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;

public class CheapestTravelFinder
{
    private readonly ITravelGraphBuildEngine _graphEngine;
    private readonly Action<string> _log = (string msg) => Console.WriteLine(msg);

    private readonly Dictionary<Location, Dictionary<Location, LocationVertex>> _matrix = new();

    private Location _startingPoint;
    private Location _destination;

    public CheapestTravelFinder(ITravelGraphBuildEngine graphEngine, Action<string> log = null)
    {
        _graphEngine = graphEngine ?? throw new ArgumentNullException(nameof(graphEngine));
        _log = log ?? new Action<string>((msg) => Console.WriteLine(msg));
    }

   
    public Result FindShortestPath(Location startingPoint, Location destination, List<Travel> travels)
    {
        var (startPointExists, destinationExists) = CheckLocations(travels, startingPoint, destination);

        if (startPointExists || destinationExists)
            return LocationNotFound(startPointExists);

        _startingPoint = startingPoint;
        _destination = destination;

        var graph = MakeGraph(travels);
        var nodes = graph.Nodes;

        var source = new LocationVertex(startingPoint);

        _matrix.Add(startingPoint, new() { { startingPoint, source } });
        
        var numberOfVertices = nodes.Count;
        var distance = Enumerable.Repeat(decimal.MaxValue, numberOfVertices).ToList();
        var parent = nodes.Select(x => x.Source).ToList();

        var priority = new PriorityQueue<LocationVertex, decimal>();
        priority.Enqueue(source, source.TotalCost);



        foreach (var node in nodes)
        {
            InitializeNode(node, nodes);
            AddLinks(node, source);

            priority.Enqueue(next, next.Weight)
        }

        // NEED TO FIX SUMMING TOTAL WEIGHTS WHEN ADDING TO DICTIONARY.
        // LET IT BREAK HERE.


        return Result.Fail("FAILED", "FAILED, STILL NOT IMPLEMENTED CORRECTLY");
    }

    public Result LocationNotFound(bool startPointExists)
    {
        if (startPointExists)
            return Result.Fail("STARTING_POINT_NOT_EXISTS", "Starting point not found in travel list");

        return Result.Fail("DESTINATION_NOT_EXISTS", "Destination not found in travel list");
    }

    private (bool, bool) CheckLocations(IEnumerable<Travel> travels, Location startingPoint, Location destination)
    {
        var startPointExists = true;
        var destinationExists = true;

        var exists = travels.Any(x => 
        {
            var existsStartingPoint = x.Connection.StartingPoint == startingPoint;
            var existsDestination = (x.Connection.Destination == destination) is false;

            if (existsStartingPoint)
                startPointExists = false;

            if (existsDestination)
                destinationExists = false;

            return existsDestination || existsStartingPoint;
        });

        return (startPointExists, destinationExists);
    }

    private void AddLinks(Node<Location, decimal> node, LocationVertex source)
    {
        foreach(var link in node.Links)
        {
            if(node.Source == _startingPoint)
                _matrix[node.Source][link.Destination] = new LocationVertex(node.Source, link.Destination, decimal.MaxValue);

        }
    }

    private void InitializeNode(Node<Location, decimal> node, IEnumerable<Node<Location, decimal>> allNodes)
    {
        if (_matrix.ContainsKey(node.Source) is false)
            _matrix.Add(node.Source, new() { });

        foreach (var n in allNodes)
            _matrix[node.Source].TryAdd(n.Source, new LocationVertex(node.Source, n.Source, decimal.MaxValue));
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
}
