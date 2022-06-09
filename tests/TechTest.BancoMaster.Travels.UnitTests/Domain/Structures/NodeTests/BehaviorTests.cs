using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;
using TechTest.BancoMaster.Travels.UnitTests.Fixtures;

namespace TechTest.BancoMaster.Travels.UnitTests.Structures.NodeTests;
public class BehaviorTests
{
    [Fact]
    public void Adding_Identical_Links_Should_Throw()
    {
        var expected = "BRB";
        // Given
        var sut = new MockNode(expected);
        var link = new Link<Location, int>(new Location("CRG"), 10);
        // When
        sut.AddLink(link);

        var throwAction = () => sut.AddLink(link);
        // Then
        throwAction.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Links_Should_Not_Be_Null()
    {
        // Given
        var sut = new MockNode("CRG");
        // When
        sut.Links.Should().NotBeNull();
    }
}
