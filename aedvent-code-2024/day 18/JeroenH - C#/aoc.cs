using Net.Code.Graph;
using Net.Code.Graph.Algorithms;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
var size = 71;
var n = 1024;
var writer = Console.Out;
Coordinate[] coordinates = (
    from line in input
    let comma = line.IndexOf(',')
    let x = int.Parse(line[..comma])
    let y = int.Parse(line[(comma + 1)..])
    select new Coordinate(x, y)).ToArray();
stats.Report("Init");
var part1 = FindShortestPaths(coordinates[..n].ToHashSet()).Count();
stats.Report(1, part1);
var part2 = Part2();
stats.Report(2, part2);
IEnumerable<Coordinate> FindShortestPaths(HashSet<Coordinate> walls)
{
    var g = GraphBuilder.Create<Coordinate, int>().AddEdges(
        from c in Coordinate.Range(size, size)
        where !walls.Contains(c)
        from n in c.Neighbours(size, size)
        where n.In(size, size) && !walls.Contains(n)
        select Edge.Create(c, n, 1)).BuildGraph();
    return Dijkstra.ShortestPaths(g, new(0, 0)).GetPath(new(size - 1, size - 1));
}

string Part2()
{
    var (lower, upper) = (n, coordinates.Length);
    var hashSet = new HashSet<Coordinate>();
    while (lower < upper)
    {
        var n = (lower + upper) / 2;
        hashSet.Clear();
        foreach (var c in coordinates[..n])
            hashSet.Add(c);
        var shortestPath = FindShortestPaths(hashSet);
        if (shortestPath.Any())
            lower = n + 1;
        else
            upper = n;
    }

    return coordinates[lower - 1].ToString();
}

readonly record struct Coordinate(int x, int y)
{
    public static Coordinate Origin = new(0, 0);
    public override string ToString() => $"{x},{y}";
    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
    static readonly (int dx, int dy)[] deltas =
    {
        (0, -1),
        (1, 0),
        (0, 1),
        (-1, 0)
    };
    public IEnumerable<Coordinate> Neighbours(int width, int height)
    {
        var c = this;
        foreach (var d in deltas)
        {
            var n = c + d;
            if (n.In(width, height))
                yield return n;
        }
    }

    public static IEnumerable<Coordinate> Range(int width, int height)
    {
        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                yield return new Coordinate(x, y);
    }

    public bool In(int width, int height) => (x, y, width - x, width - y) is ( >= 0, >= 0, > 0, > 0);
}