using TechTest.BancoMaster.Travels.Application.CheapestRouteCalculation;
using TechTest.BancoMaster.Travels.Domain.Structures;
using TechTest.BancoMaster.Travels.Domain.Travels;

namespace TechTest.BancoMaster.Travels.UnitTests.Application.CheapestRouteCalculation.TravelGraphBuilderTests;
public class BuildingGraphTests
{
    [Fact]
    public void Adding_Distinct_Nodes_Should_Work()
    {
        var nodeQuantity = 10;
        var sut = new GraphBuilder();

        var nodes = GetNodes(10);

        foreach (var node in nodes)
            sut.AddNode(node);

        var graph = sut.Build();

        graph.Nodes.Count.Should().Be(nodeQuantity);
    }

    public static IEnumerable<Node> GetNodes(int quantity)
    {
        var nodeBuilder = new NodeBuilder();
        var random = new Random();
        var nodes = Enumerable.Range(0, quantity)
            .Select(x => nodeBuilder.Create($"node {x}")
                .LinkTo($"node {(x > 1 ? x - 1 : x >= 9 ? x - 2 : x + 1)}", random.NextInt64(0, 99))
                .LinkTo($"node {(x + 3 > quantity ? quantity - (3 + x) : x + 3)}", random.NextInt64(0, 99))
                .Build());

        return nodes;
    }
}
