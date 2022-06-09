
using System.Collections.Immutable;

namespace TechTest.BancoMaster.Travels.Domain.Structures
{
    public class DirectedGraph<TNode, TWeight>
    {
        private List<Node<TNode, TWeight>> _nodes { get; } = new List<Node<TNode, TWeight>>();
        public ImmutableList<Node<TNode, TWeight>> Nodes => _nodes.ToImmutableList();

        public void AddNodes(IEnumerable<Node<TNode, TWeight>> nodes)
        {
            foreach (var node in nodes)
                AddNode(node);
        }
        
        public void AddNode(Node<TNode, TWeight> node) 
        {
            if (this == Null)
                return;

            if (_nodes.Exists(x => x == node) is true)
                throw new ArgumentException("This Node Already exists in the graph");
            
            _nodes.Add(node);
        }

        public static readonly DirectedGraph<TNode, TWeight> Null = new();
    }
}