using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.UnitTests.Fixtures;

public class MockNode : Node
{
    public MockNode(Location Location) : base(Location, 0) {  }
}