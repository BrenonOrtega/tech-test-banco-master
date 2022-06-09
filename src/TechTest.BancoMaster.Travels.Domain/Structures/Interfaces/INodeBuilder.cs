namespace TechTest.BancoMaster.Travels.Domain.Structures;

public interface INodeBuilder<TNode, TWeight>
{
    INodeBuilder<TNode, TWeight> Create(TNode node);
    INodeBuilder<TNode, TWeight> LinkTo(TNode other, TWeight weight);
    Node<TNode, TWeight> Build();
    INodeBuilder<TNode, TWeight> Clear();
}
