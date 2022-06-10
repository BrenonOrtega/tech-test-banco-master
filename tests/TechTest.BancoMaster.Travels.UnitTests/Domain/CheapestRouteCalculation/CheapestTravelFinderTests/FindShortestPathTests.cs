
using System.Diagnostics;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;
using TechTest.BancoMaster.Travels.UnitTests.Fixtures;
using static TechTest.BancoMaster.Travels.UnitTests.Fixtures.FixtureHelper;

namespace TechTest.BancoMaster.Travels.UnitTests.Domain.CheapestRouteCalculation.CheapestTravelFinderTests;

public class FindShortestPathTests
{
    [Fact]
    public void TestName()
    {
        // Given
        var travelList = GetTravelList();
        var startingPoint = "GRU";
        var destination = "CDG";

        var engine = FixtureHelper.GetTravelGraphBuildEngine();
        var sut = new CheapestTravelFinder(engine, x => Trace.WriteLine(x));
        // When
        var result = sut.FindShortestPath(startingPoint, destination, travelList);
        // Then
    }
}
