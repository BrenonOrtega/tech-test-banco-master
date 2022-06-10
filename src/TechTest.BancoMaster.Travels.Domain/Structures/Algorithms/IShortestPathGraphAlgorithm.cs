namespace TechTest.BancoMaster.Travels.Domain.Structures;

public interface IShortestPathGraphAlgorithm
{
    LinkedList<Node> FindShortestPath(DirectedGraph graph, Node Source, Node Destination);
}