
using System.Collections.Immutable;

namespace TechTest.BancoMaster.Travels.Domain.Structures
{
    public class DirectedGraph
    { 
        private Dictionary<string, Node> _nodes { get; } = new();
        public ImmutableDictionary<string, Node> Nodes => _nodes.ToImmutableDictionary();

         public void AddNodes(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
                AddNode(node);
        }
        
        public void AddNode(Node node) 
        {
            if (this == Null)
                return;

            if (_nodes.ContainsKey(node.Name))
                throw new ArgumentException("This Node Already exists in the graph");
            
            _nodes.Add(node.Name, node);
        } 

        public static readonly DirectedGraph Null = new();
    }
}