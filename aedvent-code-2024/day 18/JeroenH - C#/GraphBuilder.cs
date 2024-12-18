using System.Collections.Immutable;

namespace Net.Code.Graph;
public static class GraphBuilder
{
    public static IGraphBuilder<TVertex, TValue> Create<TVertex, TValue>() where TVertex : IEquatable<TVertex>
        => new GraphBuilderImpl<TVertex, TValue>();

    class GraphBuilderImpl<TVertex, TValue> : IGraphBuilder<TVertex, TValue> where TVertex : IEquatable<TVertex>
    {
        private readonly HashSet<TVertex> vertices = [];
        private readonly HashSet<Edge<TVertex, TValue>> edges = [];
        private IReadOnlyDictionary<TVertex, string> labels = ImmutableDictionary<TVertex, string>.Empty;
        public IGraphBuilder<TVertex, TValue> AddVertex(TVertex source)
        {
            vertices.Add(source);
            return this;
        }
        public IGraphBuilder<TVertex, TValue> AddVertices(IEnumerable<TVertex> vertices)
        {
            foreach (var v in vertices) this.vertices.Add(v);
            return this;
        }

        public IGraphBuilder<TVertex, TValue> AddEdge(TVertex source, TVertex destination, TValue value) 
            => AddEdge(Edge.Create(source, destination, value));
        public IGraphBuilder<TVertex, TValue> AddEdge(Edge<TVertex, TValue> edge)
        {
            edges.Add(edge);
            AddVertices([edge.Source, edge.Destination]);
            return this;
        }
        public IGraphBuilder<TVertex, TValue> AddEdges(IEnumerable<Edge<TVertex, TValue>> edges)
        {
            foreach (var e in edges) AddEdge(e);
            return this;
        }

        public IGraphBuilder<TVertex, TValue> WithLabels(IReadOnlyDictionary<TVertex, string> labels)
        {
            this.labels = labels;
            return this;
        }

        public IGraph<TVertex, TValue> BuildGraph() => new Graph<TVertex, TValue>(
                vertices.ToImmutableHashSet(),
                edges.ToLookup(e => e.Source),
                labels
            );
    }

}
public interface IGraphBuilder<TVertex, TValue> where TVertex : IEquatable<TVertex>
{
    IGraphBuilder<TVertex, TValue> AddEdge(Edge<TVertex, TValue> edge);
    IGraphBuilder<TVertex, TValue> AddEdge(TVertex source, TVertex destination, TValue value);
    IGraphBuilder<TVertex, TValue> AddEdges(IEnumerable<Edge<TVertex, TValue>> edges);
    IGraphBuilder<TVertex, TValue> AddVertex(TVertex source);
    IGraphBuilder<TVertex, TValue> AddVertices(IEnumerable<TVertex> vertices);
    IGraph<TVertex, TValue> BuildGraph();
    IGraphBuilder<TVertex, TValue> WithLabels(IReadOnlyDictionary<TVertex, string> labels);
}

public interface IGraphBuilder<TVertex> : IGraphBuilder<TVertex, int> where TVertex : IEquatable<TVertex>
{ }
