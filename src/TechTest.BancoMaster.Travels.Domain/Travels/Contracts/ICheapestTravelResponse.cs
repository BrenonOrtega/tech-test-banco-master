
using Awarean.Sdk.ValueObjects;

namespace TechTest.BancoMaster.Travels.Domain.Travels.Contracts;

public interface ICheapestTravelResponse
{
    public Location StartingPoint { get; }
    public Location Destination { get; }
    public Money TotalAmount { get; }
    public LinkedList<(string Location, decimal Amount)> BestTravelRoute { get; }
    public string DescribeCheapestTravel();
}
