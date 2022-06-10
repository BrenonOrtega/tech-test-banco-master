using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Models;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation.Builders;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;

public class TravelGraphBuilder : ITravelGraphBuilder
{
    private readonly Dictionary<string, Node> _nodes = new();

    public IGraphBuilder AddLink(Location source, Location destination, decimal weight)
    {
        var exists = _nodes.TryGetValue(source, out var node);

        if (exists)
            node.AddLink(new LocationLink(node.Name, destination, weight));

        return this;
    }

    public IGraphBuilder AddNode(Node node)
    {
        if (_nodes.ContainsKey(node.Name) is false)
        {
            _nodes.Add(node.Name, node);
        }

        return this;
    }

    public DirectedGraph Build()
    {
        var graph = new DirectedGraph();

        graph.AddNodes(_nodes.Select(x => x.Value));

        Clear();

        return graph;
    }

    public IGraphBuilder Clear()
    {
        _nodes.Clear();

        return this;
    }
}
