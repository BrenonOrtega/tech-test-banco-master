using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
public class TravelNodeBuilder : ITravelNodeBuilder
{
    private LocationNode _instance;
    private readonly List<LocationLink> _links = new();

    public Node<Location, decimal> Build()
    {
        var node = _instance;

        _instance?.AddLinks(_links);

        Clear();
        return node;
    }

    public INodeBuilder<Location, decimal> Clear()
    {
        _instance = default;
        _links.Clear();
        return this;
    }

    public INodeBuilder<Location, decimal> Create(Location node)
    {
        Clear();
        _instance = new LocationNode(node);

        return this;
    }

    public ITravelNodeBuilder LinkFromTravel(Travel travel)
    {
        if(_instance.Id != travel.Connection.StartingPoint)
            throw new ArgumentException("Cannot Create Link for node with a different starting point");

        return LinkTo(travel.Connection.Destination, travel.Amount) as ITravelNodeBuilder;
    }

    public INodeBuilder<Location, decimal> LinkTo(Location other, decimal weight)
    {
        var link = new LocationLink(other, weight);

        _links.Add(link);

        return this;
    }
}

