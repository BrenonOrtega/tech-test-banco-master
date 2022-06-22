using Microsoft.Extensions.Logging;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.Infra;
using TechTest.BancoMaster.Travels.Infra.Travels.Repositories;
using TechTest.BancoMaster.Travels.UnitTests.Fixtures;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TechTest.BancoMaster.Travels.Infra.Context.Models;

namespace TechTests.BancoMaster.Travels.UnitTests.Infra.Travels.Repositories;

public class TravelsRepositoryTests
{
    private readonly TravelsContext _context;
    public TravelsRepositoryTests() => _context = BuildContext();

    ~TravelsRepositoryTests()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task Adding_New_Travel_Should_Add_Correctly()
    {
        var travel = new Travel("CDG", "BRC", 1000);
        var logger = Substitute.For<ILogger<TravelRepository>>();

        var sut = new TravelRepository(logger, _context);

        var id = await sut.SaveAsync(travel);

        var entity = await sut.GetByIdAsync(id);

        id.Should().Be(travel.Id);
        entity.Should().BeEquivalentTo(travel, x => x.ComparingRecordsByValue().ExcludingFields().ExcludingMissingMembers());
    }
    

    [Fact]
    public async Task Updating_Travel_Should_Work()
    {
        var travel = new Travel("CDG", "BRC", 1000);

        var logger = Substitute.For<ILogger<TravelRepository>>();
        
        var sut = new TravelRepository(logger, _context);

        var id = await sut.SaveAsync(travel);

        var update = new Travel("CDG", "BRC", 5000);

        await sut.UpdateAsync(travel.Id, update);

        var updated = await sut.GetByIdAsync(id);

        updated.Id.Should().Be(travel.Id);
        updated.Amount.Should().Be(update.Amount);
    }

    [Fact]
    public async Task Getting_Connections_Should_Get_All_Possible_Routes()
    {
        var startingPoint = "GRU";
        var destination = "CDG";

        var locations = FixtureHelper.GetTravelList();

        var logger = Substitute.For<ILogger<TravelRepository>>();

        _context.Set<TravelModel>().AddRange(locations.Select(x => (TravelModel)x).ToArray());
        _context.SaveChanges();

        var sut = new TravelRepository(logger, _context);

        var travels = (await sut.GetConnectionLocations(startingPoint, destination)).ToList();

        travels.Count.Should().BeGreaterThanOrEqualTo(locations.Count);
    }

    private static TravelsContext BuildContext()
    {
        var connection = new SqliteConnection("DataSource=file::memory:");
        var builder = new DbContextOptionsBuilder();
        connection.Open();
        builder.UseSqlite(connection);
        var data = new TravelsContext(builder.Options);
        data.Database.Migrate();
        data.Database.EnsureCreated();
        return data;
    }
}