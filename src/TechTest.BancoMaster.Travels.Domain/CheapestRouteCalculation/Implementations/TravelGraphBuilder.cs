using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public class TravelGraphBuilder : ITravelGraphBuilder
{
    private readonly Dictionary<string, Node<Location, decimal>> _nodes = new();

    public IGraphBuilder<Location, decimal> AddLink(Location source, Location destination, decimal weight)
    {
        var exists = _nodes.TryGetValue(source, out var node);
        
        if(exists)
            node.AddLink(new LocationLink(destination, weight));
        
        return this;
    }

    public IGraphBuilder<Location, decimal> AddNode(Node<Location, decimal> node)
    {
        if(_nodes.ContainsKey(node.Id) is false)
        {
            _nodes.Add(node.Id, node);
        }

        return this;
    }

    public DirectedGraph<Location, decimal> Build()
    {
        var graph = new DirectedGraph<Location, decimal>();

        graph.AddNodes(_nodes.Select(x => x.Value));

        Clear();

        return graph;
    }

    public IGraphBuilder<Location, decimal> Clear()
    {
        _nodes.Clear();

        return this;
    }
}
