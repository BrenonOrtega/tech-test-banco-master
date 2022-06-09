using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation
{
    public interface ITravelGraphBuilder : IGraphBuilder<Location, decimal>
    {
    }
}