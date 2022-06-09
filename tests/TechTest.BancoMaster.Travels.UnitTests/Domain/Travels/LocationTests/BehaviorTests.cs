using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.UnitTests.Domain.Travels.LocationTests;

public class BehaviorTests
{
    [Fact]
    public void string_Should_Be_Convertible_To_Location()
    {
        // Given
        var validName = "CBG";
        // When

        Location sut = validName;
        // Then

        sut.Name.Should().Be(validName);
    }

    [Fact]
    public void Location_Should_Be_Convertible_To_String()
    {
        // Given
        var validName = "CBG";
        var sut = new Location(validName);

        // When
        var location = (string)sut;
        // Then

        location.Should().Be(validName);
    }

    [Fact]
    public void Location_ToString_Should_Be_Its_Name()
    {
        // Given
        var validName = "CBG";
        var sut = new Location(validName);

        // When
        var locationName = sut.ToString();
        // Then

        locationName.Should().Be(validName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Invalid_Location_Name_Should_Throw(string invalidName)
    {
        var invalidAction = new Action(() =>
        {
            Location sut = invalidName;
        });

        // Then
        invalidAction.Invoking(x => x.Invoke()).Should().Throw<InvalidOperationException>();
    }
}