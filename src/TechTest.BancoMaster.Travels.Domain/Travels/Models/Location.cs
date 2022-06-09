namespace TechTest.BancoMaster.Travels.Domain.Travels;

public record Location(string Name)
{
    private Location() : this("Empty Location Object") { }
    public static readonly Location Null = new Location();

    public static implicit operator Location(string name)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidOperationException("Location name should not be null or empty");
        }

        return new Location(name);
    }

    public static implicit operator string(Location location) => location.ToString();
    public override string ToString() => Name;
}