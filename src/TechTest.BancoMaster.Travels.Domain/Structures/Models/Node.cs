
using TechTest.BancoMaster.Travels.Domain.Extensions;

namespace TechTest.BancoMaster.Travels.Domain.Structures;

public class Node
{
    private readonly List<Link> _links = new();
    
    public Node Previous { get; set; }
    public string Name { get; }
    public decimal Weight { get; set; } = decimal.MaxValue;
    public IReadOnlyList<Link> Links { get => _links; }

    public Node(string name, decimal weight)
    {
        Name = name;
        Weight = weight;
    }

    public virtual void AddLink(Link link)
    {
        CheckExistence(link);
        _links.Add(link);
    }
    public virtual void AddLinks(IEnumerable<Link> links)
    {
        CheckExistence(links);
        _links.AddRange(links);
    }

    private void CheckExistence(Link link)
    {
        var exists = _links.Exists(x => x == link);

        if (exists)
            throw new ArgumentException($"The link {link} already exists");
    }

    private void CheckExistence(IEnumerable<Link> links)
    {
        var haveRepetition = links.GroupBy(x => x.Destination).Any(x => x.Count() > 1);
        if (haveRepetition)
            throw new ArgumentException("Added Links have repetitions of one or more links");

        var exists = _links.Any(link => links.Contains(link));
        if (exists)
            throw new ArgumentException($"An Link already exists - links: {links.ToFormatString()}");
    }
}
