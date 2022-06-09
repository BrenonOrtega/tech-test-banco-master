
using TechTest.BancoMaster.Travels.Domain.Extensions;

namespace TechTest.BancoMaster.Travels.Domain.Structures;

public record Node<TNode, TWeight>(TNode Source)
{
    public virtual string Id { get; }
    private readonly List<Link<TNode, TWeight>> _links = new();

    public IReadOnlyList<Link<TNode, TWeight>> Links { get => _links; }

    public virtual void AddLink(Link<TNode, TWeight> link)
    {
        CheckExistence(link);
        _links.Add(link);
    }
    public virtual void AddLinks(IEnumerable<Link<TNode, TWeight>> links)
    {
        CheckExistence(links);
        _links.AddRange(links);
    }

    private void CheckExistence(Link<TNode, TWeight> link)
    {
        var exists = _links.Exists(x => x == link);

        if (exists)
            throw new ArgumentException($"The link {link} already exists");
    }

    private void CheckExistence(IEnumerable<Link<TNode, TWeight>> links)
    {
        var haveRepetition = links.GroupBy(x => x.Destination).Any(x => x.Count() > 1);
        if (haveRepetition)
            throw new ArgumentException("Added Links have repetitions of one or more links");

        var exists = _links.Any(link => links.Contains(link));
        if (exists)
            throw new ArgumentException($"An Link already exists - links: {links.ToFormatString()}");
    }
}
