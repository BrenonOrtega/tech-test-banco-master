using Awarean.Sdk.SharedKernel;
using Awarean.Sdk.ValueObjects;

namespace TechTest.BancoMaster.Travels.Domain.Travels;
public class TravelConnection : Entity<string>
{
    public Connection Connection { get; }
    public Money Amount { get; }


    public TravelConnection(string source, string destination, Money amount) : this(new Connection(source, destination), amount)
    {
        Connection = new Connection(source, destination);
        Amount = amount;
    }

    public TravelConnection(Connection connection, Money amount)
    {
        Connection = connection;
        Amount = amount;
        Id = Connection.Id;
    }
}