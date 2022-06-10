using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Models;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation.Builders;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;
public class TravelNodeBuilder : ITravelNodeBuilder
{
    private LocationNode _instance;
    private readonly List<LocationLink> _links = new();

    public Node Build()
    {
        var node = _instance;

        _instance?.AddLinks(_links);

        Clear();
        return node;
    }

    public INodeBuilder Clear()
    {
        _instance = default;
        _links.Clear();
        return this;
    }

    public INodeBuilder Create(string node)
    {
        Clear();
        _instance = new LocationNode(node);

        return this;
    }

    public ITravelNodeBuilder LinkFromTravel(Travel travel)
    {
        if(_instance.Name != travel.Connection.StartingPoint)
            throw new ArgumentException("Cannot Create Link for node with a different starting point");

        return LinkTo(travel.Connection.Destination, travel.Amount) as ITravelNodeBuilder;
    }

    public INodeBuilder LinkTo(string other, decimal weight)
    {
        var link = new LocationLink(_instance.Name, other, weight);

        _links.Add(link);

        return this;
    }
}

