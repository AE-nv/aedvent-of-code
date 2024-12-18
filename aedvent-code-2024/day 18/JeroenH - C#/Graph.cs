using System.Collections.Immutable;
using System.Text;

namespace Net.Code.Graph;

public interface IGraph<TVertex, TValue> where TVertex : IEquatable<TVertex>
{
    int GetVertexId(TVertex vertex);
    TVertex GetVertex(int id);
    IEnumerable<TVertex> Vertices { get; }
    int VertexCount { get; }
    int EdgeCount { get; }
    IEnumerable<Edge<TVertex, TValue>> Edges { get; }
    Edge<TVertex, TValue>? GetEdge(TVertex source, TVertex to);
    IEnumerable<Edge<TVertex, TValue>> GetEdgesFrom(TVertex source);
    IEnumerable<TVertex> GetAdjacentVertices(TVertex current);
    TValue[,] ToAdjacencyMatrix();
    Dictionary<int, List<(int dest, TValue value)>> ToDictionary();
}


internal class Graph<TVertex, TValue> : IGraph<TVertex, TValue>
    where TVertex : IEquatable<TVertex>
{
    private readonly TVertex[] vertices;
    private readonly IReadOnlyDictionary<TVertex, int> vertexToIndex;
    private readonly ILookup<TVertex, Edge<TVertex, TValue>> edgesBySource;
    private readonly IReadOnlyDictionary<TVertex, string> labels;
    internal Graph(
        ImmutableHashSet<TVertex> vertices,
        ILookup<TVertex, Edge<TVertex, TValue>> edgesBySource,
        IReadOnlyDictionary<TVertex, string>? labels = null
    )
    {
        this.vertices = [.. vertices];
        this.vertexToIndex = vertices.Select((v, i) => (v, i)).ToDictionary(t => t.v, t => t.i);
        this.edgesBySource = edgesBySource;
        this.labels = labels ?? ImmutableDictionary<TVertex, string>.Empty;
    }

    public IEnumerable<TVertex> Vertices => vertices;
    public int VertexCount => vertices.Length;
    public int EdgeCount => edgesBySource.Sum(g => g.Count());
    public IEnumerable<Edge<TVertex, TValue>> Edges => edgesBySource.SelectMany(l => l);
    public Edge<TVertex, TValue>? GetEdge(TVertex source, TVertex destination)
    {
        foreach (var e in edgesBySource[source])
            if (destination.Equals(e.Destination)) return e;
        return null;
    }
    public IEnumerable<Edge<TVertex, TValue>> GetEdgesFrom(TVertex source)
        => edgesBySource[source].OrderBy(e => e.Destination);

    public IEnumerable<TVertex> GetAdjacentVertices(TVertex source)
        => edgesBySource[source].Select(e => e.Destination);

    public TValue[,] ToAdjacencyMatrix()
    {
        TValue[,] adjacencymatrix = new TValue[VertexCount, VertexCount];
        foreach (var e in Edges)
        {
            adjacencymatrix[vertexToIndex[e.Source], vertexToIndex[e.Destination]] = e.Value;
        }
        return adjacencymatrix;
    }

    public Dictionary<int, List<(int, TValue)>> ToDictionary()
    {
        var result = from v in vertices
                     let destinations = from e in GetEdgesFrom(v)
                                        select (vertexToIndex[e.Destination], e.Value)
                     select (source: vertexToIndex[v], destinations);
        return result.ToDictionary(x => x.source, x => x.destinations.ToList());
    }

    public int GetVertexId(TVertex vertex) => vertexToIndex[vertex];

    public TVertex GetVertex(int id) => vertices[id];
}
