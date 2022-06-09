namespace TechTest.BancoMaster.Travels.Domain.Structures
{
    public class PathFinder<TNode, TWeight>
    {
        private readonly DirectedGraph<TNode, TWeight> _graph;

        public PathFinder(DirectedGraph<TNode, TWeight> graph)
        {
            if (graph == null)
                throw new ArgumentException("Cannot create path finder with null graph.");

            _graph = graph;
        }

        public Path FindShortestPath(Node<TNode,TWeight> origin, Node<TNode,TWeight> destination)
        {
            if (origin == null)
                throw new ArgumentException("Cannot path from null node.");

            if (destination == null)
                throw new ArgumentException("Cannot path to null node.");

            if (origin == destination)
                throw new ArgumentException("Cannot find path to self.");

            return Build(origin, destination, Process(origin, destination, _graph));
        }

        private static List<Record> Process(Node<TNode,TWeight> origin, Node<TNode,TWeight> destination, DirectedGraph<TNode, TWeight> graph)
        {
            var visited = new List<Node<TNode,TWeight>>();

            var records = CreateInitialRecords(origin, graph);

            var currentRecord = NextRecordToProcess(records, visited, destination);

            do
            {
                if (IsPathingNotPossible(currentRecord))
                    throw new KeyNotFoundException("No paths have been found.");

                foreach (var link in currentRecord.Vertex.Links)
                {
                    var weight = currentRecord.Weight + Convert.ToDouble(link.Weight);
                    var nextRecord = NextRecord(link, records);

                    if (weight < nextRecord.Weight)
                        nextRecord.Update(weight, currentRecord.Vertex);
                }

                visited.Add(currentRecord.Vertex);

                currentRecord = NextRecordToProcess(records, visited, destination);
            } while (currentRecord != null);

            return records;
        }

        private static List<Record> CreateInitialRecords(Node<TNode,TWeight> node, DirectedGraph<TNode, TWeight> graph)
        {
            var records = graph.Node<TNode,TWeight>s
                .Select(Record.Create)
                .ToList();

            const double initalWeight = 0;
            const Node<TNode,TWeight> initialNode<TNode,TWeight> = null;

            records
                .Single(r => r.Vertex == node)
                .Update(initalWeight, initialNode<TNode,TWeight>);

            return records;
        }

        private static Record NextRecordToProcess(IEnumerable<Record> records, ICollection<Node<TNode,TWeight>> visited, Node<TNode,TWeight> destination)
        {
            if (visited.Contains(destination))
                return null;

            return records
                .Where(record => !visited.Contains(record.Vertex))
                .OrderBy(r => r.Weight)
                .FirstOrDefault();
        }

        private static bool IsPathingNotPossible(Record currentRecord)
        {
            return Math.Abs(currentRecord.Weight - double.MaxValue) < double.Epsilon;
        }

        private static Record NextRecord(Link<TNode, TWeight> link, IEnumerable<Record> records)
        {
            return records.Single(record => record.Vertex.Source.Equals(link.Destination));
        }

        private static Path Build(Node<TNode,TWeight> origin, Node<TNode,TWeight> destination, List<Record> records)
        {
            var path = Path.Create(origin);

            foreach (var segment in PathSegments(destination, records))
                path.AddSegment(segment);

            return path;
        }

        private static IEnumerable<PathSegment> PathSegments(Node<TNode,TWeight> destination, List<Record> records)
        {
            var segments = new List<PathSegment>();

            var currentRecord = Destination(destination, records);

            do
            {
                segments.Add(CreateSegment(currentRecord));
                currentRecord = PreviousRecord(currentRecord, records);
            } while (!IsOriginRecord(currentRecord));

            return segments.AsEnumerable().Reverse();
        }

        private static Record Destination(Node<TNode,TWeight> destination, IEnumerable<Record> records)
        {
            return records.Single(record => record.Vertex == destination);
        }

        private static PathSegment<TNode, TWeight> CreateSegment(Record currentRecord)
        {
            return PathSegment<TNode, TWeight>.Create(currentRecord.PreviousVertex, currentRecord.Vertex,
                currentRecord.PreviousVertex.Links.Single(link => link.Destination == currentRecord.Vertex).Weight);
        }

        private static Record PreviousRecord(Record currentRecord, IEnumerable<Record> records)
        {
            return records.Single(record => record.Vertex == currentRecord.PreviousVertex);
        }

        private static bool IsOriginRecord(Record currentRecord)
        {
            return currentRecord.PreviousVertex == null;
        }

        private class Record
        {
            private Record(Node<TNode,TWeight> vertex)
            {
                Vertex = vertex;
                Weight = double.MaxValue;
                PreviousVertex = null;
            }

            internal Node<TNode,TWeight> Vertex { get; }
            internal double Weight { get; private set; }
            internal Node<TNode,TWeight> PreviousVertex { get; private set; }

            internal static Record Create(Node<TNode,TWeight> vertex)
            {
                return new Record(vertex);
            }

            internal void Update(double weight, Node<TNode,TWeight> previousVertex)
            {
                Weight = weight;
                PreviousVertex = previousVertex;
            }
        }
    }
    public class Path<TNode, TWeight>
    {
        private readonly List<PathSegment<TNode, TWeight>> _segments;

        private Path(Node<TNode, TWeight> origin)
        {
            Origin = origin;
            _segments = new List<PathSegment<TNode, TWeight>>();
        }

        public Node<TNode, TWeight> Origin { get; }
        public Node<TNode, TWeight> Destination { get; private set; }
        public IReadOnlyList<PathSegment<TNode, TWeight>> Segments => _segments;

        internal static Path<TNode, TWeight> Create(Node<TNode, TWeight> origin)
        {
            return new Path<TNode, TWeight>(origin);
        }

        internal void AddSegment(PathSegment<TNode, TWeight> segment)
        {
            Destination = segment.Destination;
            _segments.Add(segment);
        }
    }

    public class PathSegment<TNode, TWeight>
    {
        private PathSegment(Node<TNode, TWeight> origin, Node<TNode, TWeight> destination, double weight)
        {
            Weight = weight;
            Origin = origin;
            Destination = destination;
        }

        public Node<TNode, TWeight> Origin { get; }
        public Node<TNode, TWeight> Destination { get; }
        public double Weight { get; }

        internal static PathSegment<TNode, TWeight> Create(Node<TNode, TWeight> origin, Node<TNode, TWeight> destination, double weigth)
        {
            return new PathSegment<TNode, TWeight>(origin, destination, weigth);
        }
    }
}