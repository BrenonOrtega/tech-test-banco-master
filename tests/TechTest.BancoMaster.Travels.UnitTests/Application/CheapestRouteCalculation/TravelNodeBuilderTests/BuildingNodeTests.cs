using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.UnitTests.Application.CheapestRouteCalculation.TravelNodeBuilderTests;

public class BuildingNodeTests
{
    [Fact]
    public void Valid_Links_Should_Build_Correctly()
    {
        var sut = new TravelNodeBuilder();

        var id = "CBG";
        sut.Create(id)
            .LinkTo("CBR", 5)
            .LinkTo("DMG", 10)
            .LinkTo("BFF", 13)
            .LinkTo("DMM", 17)
            ;

        var node = sut.Build();

        node.Links.Count.Should().Be(4);
        node.Name.Should().Be((Location)id);
    }

    [Fact]
    public void Building_Node_With_Repeated_Links_Should_Throw()
    {
        var sut = new TravelNodeBuilder();

        var id = "CBG";
        var act = () => sut.Create(id)
            .LinkTo("CBR", 5)
            .LinkTo("CBR", 5)
            .Build()
            ;

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Building_Node_After_Clearing_Should_Return_Null_Object()
    {
        var sut = new TravelNodeBuilder();

        var id = "CBG";
        var node = sut.Create(id)
            .LinkTo("CBR", 5)
            .LinkTo("CBR", 5)
            .Clear()
            .Build()
            ;

        node.Should().BeNull();
    }
}
