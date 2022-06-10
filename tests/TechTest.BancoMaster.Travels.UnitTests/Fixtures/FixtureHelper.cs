using System.Diagnostics;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.UnitTests.Fixtures;
public static class FixtureHelper
{
    public static List<Travel> GetTravelList() => new List<Travel>
    {
        new Travel(source: "GRU", destination: "BRC", amount: 10),
        new Travel(source: "BRC", destination: "SCL", amount: 5),
        new Travel(source: "GRU", destination: "CDG", amount: 75),
        new Travel(source: "GRU", destination: "SCL", amount: 20),
        new Travel(source: "GRU", destination: "ORL", amount: 56),
        new Travel(source: "ORL", destination: "CDG", amount: 5),
        new Travel(source: "SCL", destination: "ORL", amount: 20),
    };

    internal static ITravelGraphBuildEngine GetTravelGraphBuildEngine()
    {
        var nodeBuilder = new TravelNodeBuilder();
        var graphBuilder = new TravelGraphBuilder();

        return new TravelGraphBuildEngine(graphBuilder, nodeBuilder, x => Trace.WriteLine(x));
    }
}