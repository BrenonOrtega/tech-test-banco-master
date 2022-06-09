using Awarean.Sdk.ValueObjects;
using TechTest.BancoMaster.Travels.Domain.RouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;
using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;

namespace TechTest.BancoMaster.Travels.UnitTests.Application.SearchEngine.CheapestSearchEngineTests;

public class SearchAsyncTests
{
    [Fact]
    public async Task Searching_Between_Locations_Should_Return_Cheapest_Travel_Path()
    {
        // Given
        var desiredTravel = new Connection("GRU", "CDG");

        var travelRoute1 = new TravelConnection(source: "GRU", destination: "BRC", amount: 10);
        var travelRoute2 = new TravelConnection(source: "BRC", destination: "SCL", amount: 5);
        var travelRoute3 = new TravelConnection(source: "GRU", destination: "CDG", amount: 75);
        var travelRoute4 = new TravelConnection(source: "GRU", destination: "SCL", amount: 20);
        var travelRoute5 = new TravelConnection(source: "GRU", destination: "ORL", amount: 56);
        var travelRoute6 = new TravelConnection(source: "ORL", destination: "CDG", amount: 5);
        var travelRoute7 = new TravelConnection(source: "SCL", destination: "ORL", amount: 20);

        var expectedRoute = BuildExpectedRoute((travelRoute1.Connection.StartingPoint, 0),
            (travelRoute1.Connection.Destination, travelRoute1.Amount),
            (travelRoute2.Connection.Destination, travelRoute2.Amount),
            (travelRoute7.Connection.Destination, travelRoute7.Amount),
            (travelRoute6.Connection.Destination, travelRoute6.Amount));

        var expected = Substitute.For<ICheapestTravelResponse>();
        expected.BestTravelRoute.Returns(expectedRoute);
        expected.TotalAmount.Returns((Money)expectedRoute.Sum(x => x.connectionAmount));
        expected.StartingPoint.Returns((Location)"GRU");

        var repo = Substitute.For<ITravelConnectionRepository>();
        var logger = Substitute.For<ILogger<CheapestTravelSearchEngine>>();

        var command = GetCommand(desiredTravel);

        var sut = new CheapestTravelSearchEngine(repo, x => Console.WriteLine(x));

        // When
        var result = await sut.SearchAsync(command);

        // Then
        result.IsSuccess.Should().BeTrue();
        result.Value.StartingPoint.Should().Equals(desiredTravel.StartingPoint);
        result.Value.Destination.Should().Equals(desiredTravel.StartingPoint);
        result.Value.TotalAmount.Should().Be(40);
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
        {
            travelRoute.AddLast(element);
        }

        return travelRoute;
    }
}
