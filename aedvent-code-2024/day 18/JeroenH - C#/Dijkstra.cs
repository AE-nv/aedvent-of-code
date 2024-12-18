
namespace Net.Code.Graph.Algorithms;

public record DijkstraShortestPathResult<TVertex>(
    IReadOnlyDictionary<TVertex, int> PathCosts,
    IReadOnlyDictionary<TVertex, TVertex> Ancestors
    )
{
    public IEnumerable<TVertex> GetPath(TVertex destination)
    {
        var result = new List<TVertex>();
        TVertex? current = destination;
        while (Ancestors.TryGetValue(current, out current))
        {
            result.Add(current);
        }
        return result;
    }
}

public static class Dijkstra
{
    public static DijkstraShortestPathResult<TVertex> ShortestPaths<TVertex>(IGraph<TVertex, int> graph, TVertex start)
        where TVertex : IEquatable<TVertex>
    {
        int[,] adjacencymatrix = graph.ToAdjacencyMatrix();

        var source = graph.GetVertexId(start);

        var (distances, ancestors) = ComputeShortestPaths(adjacencymatrix, source, graph.VertexCount);

        return new(Enumerable.Range(0, distances.Length)
                             .ToDictionary(graph.GetVertex, i => distances[i]),
                   ancestors.ToDictionary(kv => graph.GetVertex(kv.Key),
                                          kv => graph.GetVertex(kv.Value)));

    }

    static (int[] distance, Dictionary<int, int> ancestors) ComputeShortestPaths(int[,] graph, int source, int verticesCount)
    {
        var distance = new int[verticesCount];
        var visited = new bool[verticesCount];
        Dictionary<int, int> ancestors = [];
        for (int i = 0; i < verticesCount; ++i)
        {
            distance[i] = int.MaxValue;
            visited[i] = false;
        }

        distance[source] = 0;

        for (int count = 0; count < verticesCount - 1; ++count)
        {
            int u = MinimumDistance(distance, visited, verticesCount);
            visited[u] = true;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (!visited[v] && graph[u, v] > 0 && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                {
                    distance[v] = distance[u] + graph[u, v];
                    ancestors[v] = u;
                }
            }
        }

        return (distance, ancestors);

        static int MinimumDistance(int[] distance, bool[] visited, int verticesCount) => (
                from v in Enumerable.Range(0, verticesCount)
                where !visited[v]
                select (v, distance: distance[v])
                ).MinBy(p => p.distance).v;
    }
}
