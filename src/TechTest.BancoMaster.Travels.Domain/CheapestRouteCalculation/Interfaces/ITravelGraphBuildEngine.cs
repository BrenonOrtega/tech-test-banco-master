using Awarean.Sdk.Result;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;

public interface ITravelGraphBuildEngine
{
    Result<DirectedGraph<Location, decimal>> BuildGraph(IEnumerable<Travel> travelList);
}
