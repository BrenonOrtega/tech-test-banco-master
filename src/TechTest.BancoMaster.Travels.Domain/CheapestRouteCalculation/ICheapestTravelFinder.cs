using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation
{
    public interface ICheapestTravelFinder
    {
        public Result<LinkedList<(string Location, decimal CostFromSource)>> FindShortestPath(Location startingPoint, Location destination, List<Travel> travels);
    }
}
