using Awarean.Sdk.SharedKernel;
using Awarean.Sdk.ValueObjects;

namespace TechTest.BancoMaster.Travels.Domain.Travels;
public class Travel : Entity<string>
{
    public Connection Connection { get; }
    public Money Amount { get; }

    public Travel(string source, string destination, Money amount) : this(new Connection(source, destination), amount)
    {
        Connection = new Connection(source, destination);
        Amount = amount;
    }

    public Travel(Connection connection, Money amount)
    {
        Connection = connection;
        Amount = amount;
        Id = Connection.Id;
    }
}