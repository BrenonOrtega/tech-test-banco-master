using Awarean.Sdk.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using TechTest.BancoMaster.Travels.Application.Extensions;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Domain.Travels.Contracts;
using TechTest.BancoMaster.Travels.Domain.Travels.Repositories;
using TechTest.BancoMaster.Travels.Infra.Extensions;
using static TechTest.BancoMaster.Travels.IntegrationTests.Fixtures;

namespace TechTest.BancoMaster.Travels.IntegrationTests.Application.Travels.TravelServiceTests;
public class BehaviorTests
{
    private readonly ITravelService _sut;

    public BehaviorTests() => _sut = new ServiceCollection()
        .AddLogging()
        .AddApplicationServices()
        .AddInfrastructure()
        .BuildServiceProvider()
        .GetRequiredService<ITravelService>();
        
    [Fact]
    public async Task Searching_Valid_Routes_Should_Make_Cheapest_Route()
    {
        var startingPoint = "GRU";
        var destination = "CDG";

        var result = await _sut.GetCheapestPathAsync(startingPoint, destination);
        
        result.IsSuccess.Should().BeTrue();
        result.Value.TotalAmount.Should().Be(40.00);
    }

    [Fact]
    public async Task Getting_AllRoutes_Should_Pass()
    {
        var result = await _sut.GetTravelsAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
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
