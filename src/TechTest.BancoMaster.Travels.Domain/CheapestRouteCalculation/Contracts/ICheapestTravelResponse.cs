
using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public interface ICheapestTravelResponse
{
    public Location StartingPoint { get; }
    public Location Destination { get; }
    public Money TotalAmount { get; }
    public LinkedList<(Location Location, Money Amount)> BestTravelRoute { get; }
    public string DescribeCheapestTravel();
}
