namespace TechTest.BancoMaster.Travels.Domain.Structures;

public interface IShortestPathGraphAlgorithm
{
    LinkedList<TNode> FindShortestPath<TNode, TWeight>(DirectedGraph<TWeight, TNode> graph, Node<TNode, TWeight> Source, Node<TNode, TWeight> Destination);
}