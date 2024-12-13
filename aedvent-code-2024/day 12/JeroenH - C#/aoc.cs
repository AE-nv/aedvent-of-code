using Set = System.Collections.Generic.HashSet<Coordinate>;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
List<Set> islands = GetIslands(input);
stats.Report("Init");

var part1 = (
    from island in islands
    let area = island.Count
    let perimeter = (
        from c in island
        select 4 - c.Neighbours().Count(island.Contains)).Sum()
    select area * perimeter).Sum();
stats.Report(1, part1);

var part2 = (
    from island in islands
    let area = island.Count
    let perimeter = (
        from c in island
        select GetCorners(island, c)).Sum()
    select area * perimeter).Sum();
stats.Report(2, part2);

List<Set> GetIslands(string[] input)
{
    List<Set> islands = [];
    var visited = new Set(input.Length * input[0].Length);
    for (int y = 0; y < input.Length; y++)
    {
        for (int x = 0; x < input[0].Length; x++)
        {
            var c = new Coordinate(x, y);
            if (visited.Contains(c)) continue;
            var island = new Set();
            Flood(input, c, visited, island);
            islands.Add(island);
        }
    }

    return islands;
}

static void Flood(string[] input, Coordinate c, Set visited, Set island)
{
    if (visited.Contains(c)) return;
    visited.Add(c);
    island.Add(c);
    var neighbours = 
        from n in c.Neighbours()
        where c.x >= 0 && c.y >= 0 && c.x < input[0].Length && c.y < input.Length && input[n.y][n.x] == input[c.y][c.x]
        select n;
    foreach (var n in neighbours) Flood(input, n, visited, island);
}

int GetCorners(Set island, Coordinate c)
{
    var (dnw, dn, dw) = (Dir.NW, Dir.N, Dir.W);
    int corners = 0;
    for (int i = 0; i < 4; i++)
    {
        var (nw, n, w) = (island.Contains(c + dnw), island.Contains(c + dn), island.Contains(c + dw));
        /*
        * 
        *     no n or w
        *     
        *             x
        *      CC    x^C  
        *      CC     CC
        *      
        *     n & w but no nw
        *     
        *       C     xC
        *      CC     C^
        *      CC     CC
        *
        */
        if (!n && !w || n && w && !nw) corners++;
        (dnw, dn, dw) = (dnw.Rotate90(), dn.Rotate90(), dw.Rotate90());
    }

    return corners;
}

readonly record struct Coordinate(int x, int y)
{
    public static Coordinate operator +(Coordinate left, Dir d) => new(left.x + d.dx, left.y + d.dy);
    readonly static Dir[] dirs = [Dir.N, Dir.E, Dir.S, Dir.W];
    public IEnumerable<Coordinate> Neighbours()
    {
        var p = this;
        return from d in dirs select p + d;
    }
}

record struct Dir(int dx, int dy)
{
    public static readonly Dir N = new(0, -1);
    public static readonly Dir NE = new(1, -1);
    public static readonly Dir E = new(1, 0);
    public static readonly Dir SE = new(1, 1);
    public static readonly Dir S = new(0, 1);
    public static readonly Dir SW = new(-1, 1);
    public static readonly Dir W = new(-1, 0);
    public static readonly Dir NW = new(-1, -1);
    public Dir Rotate90() => new(dy, -dx);
}
