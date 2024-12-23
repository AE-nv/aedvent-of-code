using System.Runtime.CompilerServices;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
Dictionary<string, HashSet<string>> graph = ReadInput(input);
stats.Report("Init");

var part1 = (
    from a in graph.Keys
    from b in graph[a]
    from c in graph[b]
    where graph[a].Contains(c) && (a[0] == 't' || b[0] == 't' || c[0] == 't')
    select (a.CompareTo(b), b.CompareTo(c), a.CompareTo(c)) switch
    {
        ( < 0, < 0, _) => (a, b, c),
        ( < 0, >= 0, < 0) => (a, c, b),
        ( < 0, >= 0, >= 0) => (c, a, b),
        ( >= 0, < 0, < 0) => (b, a, c),
        ( >= 0, < 0, >= 0) => (b, c, a),
        ( >= 0, >= 0, < 0) => (c, b, a),
        ( >= 0, >= 0, >= 0) => (c, b, a),
    }).Distinct().Count();
stats.Report(1, part1);

var part2 = string.Join(",", FindMaximalCliques().MaxBy(cl => cl.Count)!.Order());
stats.Report(2, part2);

Dictionary<string, HashSet<string>> ReadInput(string[] input)
{
    var graph = new Dictionary<string, HashSet<string>>();
    var edges =
        from line in input
        let split = line.IndexOf('-')
        from edge in new[]
        {
            (src: 0..split, dst: (split + 1)..),
            (src: (split + 1).., dst: 0..split)
        }

        select (line, edge);

    var lookup = graph.GetAlternateLookup<ReadOnlySpan<char>>();
    foreach (var (line, edge) in edges)
    {
        var span = line.AsSpan();
        var key = span[edge.src];
        var value = span[edge.dst];
        if (!lookup.ContainsKey(key))
            lookup[key] = [];
        lookup[key].Add(value.ToString());
    }

    return graph;
}

IEnumerable<HashSet<string>> FindMaximalCliques()
{
    var cliques = new List<HashSet<string>>();
    BronKerbosch([], [.. graph.Keys], [], cliques);
    return cliques;
}

void BronKerbosch(HashSet<string> R, HashSet<string> P, HashSet<string> X, List<HashSet<string>> cliques)
{
    if ((P, X) is ({ Count: 0 }, { Count: 0 }))
    {
        cliques.Add([.. R]);
        return;
    }

    var u = P.Union(X).First();
    var nonNeighbors = P.Except(graph[u]);
    foreach (var v in nonNeighbors)
    {
        var neighbors = graph[v];
        BronKerbosch([.. R, v], [.. P.Intersect(neighbors)], [.. X.Intersect(neighbors)], cliques);
        P.Remove(v);
        X.Add(v);
    }
}
