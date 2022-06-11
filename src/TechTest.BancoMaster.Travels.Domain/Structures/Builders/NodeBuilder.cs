using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.Structures;
public class NodeBuilder : INodeBuilder
{
    private Node _instance;
    private readonly List<Link> _links = new();

    public INodeBuilder Create(string node)
    {
        Clear();
        _instance = new Node(node, decimal.MaxValue);

        return this;
    }

    public INodeBuilder LinkTo(string other, decimal weight)
    {
        var link = new Link(_instance.Name, other, weight);

        _links.Add(link);

        return this;
    }

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
}

