
namespace TechTest.BancoMaster.Travels.Domain.Structures;

public interface IGraphBuilder
{
    IGraphBuilder AddNode(Node node);
    IGraphBuilder Clear();
    DirectedGraph Build();
}