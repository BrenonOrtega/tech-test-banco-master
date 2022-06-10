
namespace TechTest.BancoMaster.Travels.Domain.Structures;

public interface IGraphBuilder<TNode, TWeight>
{
    IGraphBuilder<TNode, TWeight> AddNode(Node<TNode, TWeight> node);
    IGraphBuilder<TNode, TWeight> Clear();
    DirectedGraph<TNode, TWeight> Build();
}