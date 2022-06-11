using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Infra.Travels.Repositories;

namespace TechTests.BancoMaster.Travels.UnitTests.Infra.Travels.Repositories;

public class TravelRepositoriesTests
{
    [Fact]
    public async Task Adding_New_Travel_Should_Add_Correctly()
    {
        var travel = new Travel("CDG", "BRC", 1000);

        var logger = Substitute.For<ILogger<TravelRepository>>();
        var data = new Dictionary<string, Travel>();
        var sut = new TravelRepository(logger, data);

        var id = await sut.SaveAsync(travel);

        id.Should().Be(travel.Id);
        data.Should().Contain(KeyValuePair.Create(travel.Id, travel));
    }

    [Fact]
    public async Task Updating_Travel_Should_Work()
    {
        var travel = new Travel("CDG", "BRC", 1000);

        var logger = Substitute.For<ILogger<TravelRepository>>();
        var data = new Dictionary<string, Travel>();
        var sut = new TravelRepository(logger, data);

        var id = await sut.SaveAsync(travel);

        var update = new Travel("CDG", "BRC", 5000);

        await sut.UpdateAsync(travel.Id, update);

        var updated = await sut.GetByIdAsync(id);

        updated.Should().Be(update);
        updated.Amount.Should().Be(update.Amount);
    }
}