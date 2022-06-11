using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation
{
    public interface ICheapestTravelFinder
    {
        public Result<SortedDictionary<string, decimal>> FindShortestPath(Location startingPoint, Location destination, List<Travel> travels);
    }
}
