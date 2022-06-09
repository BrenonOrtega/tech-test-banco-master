using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.UnitTests.Application.SearchEngine.TravelGraphSearchEngineTests;

public class MakeDirectedGraphTests
{
    [Fact]
    public async Task Searching_Between_Locations_Should_Return_Cheapest_Travel_Path()
    {
        // Given
        var desiredTravel = new Connection("GRU", "CDG");
        var expectedRoute = GetExpectedRoute();
        var expected = GetExpectedResponse(expectedRoute);

        var repo = GetRepository();
        var command = GetCommand(desiredTravel);
        var nodeBuilder = Substitute.For<ITravelNodeBuilder>();
        var graphBuilder = Substitute.For<ITravelGraphBuilder>();

        var sut = new TravelGraphSearchEngine(repo, graphBuilder,nodeBuilder, x => Console.WriteLine(x));

        // When
        var result = await sut.MakeDirectedGraphAsync(command);

        // Then
        result.IsSuccess.Should().BeTrue();
    }

    private ITravelConnectionRepository GetRepository()
    {
        var repo = Substitute.For<ITravelConnectionRepository>();

        repo.GetConnectionLocations(Arg.Any<Location>(), Arg.Any<Location>()).Returns(GetConnectionList());
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
        var list = GetConnectionList();

        var (travelRoute1, travelRoute2, travelRoute3, travelRoute4, travelRoute5, travelRoute6, travelRoute7) 
            = (list[0], list[1], list[2], list[3], list[4], list[5], list[6]);

        var expectedRoute = BuildExpectedRoute((travelRoute1.Connection.StartingPoint, 0),
            (travelRoute1.Connection.Destination, travelRoute1.Amount),
            (travelRoute2.Connection.Destination, travelRoute2.Amount),
            (travelRoute7.Connection.Destination, travelRoute7.Amount),
            (travelRoute6.Connection.Destination, travelRoute6.Amount));

        return expectedRoute;
    }

    private List<Travel> GetConnectionList() => new List<Travel>
    {
        new Travel(source: "GRU", destination: "BRC", amount: 10),
        new Travel(source: "BRC", destination: "SCL", amount: 5),
        new Travel(source: "GRU", destination: "CDG", amount: 75),
        new Travel(source: "GRU", destination: "SCL", amount: 20),
        new Travel(source: "GRU", destination: "ORL", amount: 56),
        new Travel(source: "ORL", destination: "CDG", amount: 5),
        new Travel(source: "SCL", destination: "ORL", amount: 20),
    };

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
