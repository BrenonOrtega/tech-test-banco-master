using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation.Contracts;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;
using static TechTest.BancoMaster.Travels.UnitTests.Fixtures.FixtureHelper;

namespace TechTest.BancoMaster.Travels.UnitTests.Application.SearchEngine.TravelGraphBuildEngineTests;

public class MakeDirectedGraphTests
{
    private readonly INodeBuilder _nodeBuilder;
    private readonly IGraphBuilder _graphBuilder;
    public MakeDirectedGraphTests() => (_nodeBuilder, _graphBuilder) = (new NodeBuilder(), new GraphBuilder());

    [Fact]
    public void Valid_Travel_List_Should_Build_Graph()
    {
        // Given
        var travelList = GetTravelList();
        var sut = new TravelGraphBuildEngine(_graphBuilder, _nodeBuilder, (x, y) => Console.WriteLine(x, y));

        // When
        var result = sut.BuildGraph(travelList);
        var graph = result.Value;

        var expectedCount = GetPlaces().Count();
        // Then
        result.IsSuccess.Should().BeTrue();
        graph.Nodes.Should().HaveCount(expectedCount);

    }

    private ITravelRepository GetRepository()
    {
        var repo = Substitute.For<ITravelRepository>();

        repo.GetConnectionLocations(Arg.Any<Location>(), Arg.Any<Location>()).Returns(GetTravelList());
        return repo;
    }

    private ICheapestTravelResponse GetExpectedResponse(LinkedList<(Location location, Money connectionAmount)> expectedRoute)
    {
        var expected = Substitute.For<ICheapestTravelResponse>();
        expected.BestTravelRoute.Returns(expectedRoute);
        expected.TotalAmount.Returns((Money)expectedRoute.Sum(x => x.connectionAmount));
        expected.StartingPoint.Returns((Location)"GRU");

        return expected;
    }

    private LinkedList<(Location location, Money connectionAmount)> GetExpectedRoute()
    {
        var list = GetTravelList();

        var (travelRoute1, travelRoute2, travelRoute3, travelRoute4, travelRoute5, travelRoute6, travelRoute7)
            = (list[0], list[1], list[2], list[3], list[4], list[5], list[6]);

        var expectedRoute = BuildExpectedRoute((travelRoute1.Connection.StartingPoint, 0),
            (travelRoute1.Connection.Destination, travelRoute1.Amount),
            (travelRoute2.Connection.Destination, travelRoute2.Amount),
            (travelRoute7.Connection.Destination, travelRoute7.Amount),
            (travelRoute6.Connection.Destination, travelRoute6.Amount));

        return expectedRoute;
    }


    private ISearchTravelCommand GetCommand(Connection connection)
    {
        var command = Substitute.For<ISearchTravelCommand>();
        command.From.Returns(connection.StartingPoint);
        command.To.Returns(connection.Destination);

        return command;
    }

    private LinkedList<(Location location, Money connectionAmount)> BuildExpectedRoute(params (Location, Money)[] data)
    {
        var travelRoute = new LinkedList<(Location, Money)>();
        foreach (var element in data)
            travelRoute.AddLast(element);

        return travelRoute;
    }
}
