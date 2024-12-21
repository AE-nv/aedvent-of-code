using System.Diagnostics.CodeAnalysis;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
var writer = Console.Out;
var grid = new Grid(input);
var path = PathFromEnd().ToDictionary(x => x.c, x => x.d);
stats.Report("Init");

var part1 = FindCheats(2, path, 100).Sum(x => x.count);
stats.Report(1, part1);

var part2 = FindCheats(20, path, 100).Sum(x => x.count);
stats.Report(2, part2);

IEnumerable<(Coordinate c, int d)> PathFromEnd()
{
    var distance = 0;
    var current = grid.Find('E');
    var target = grid.Find('S');
    yield return (current, distance);
    var previous = current;
    while (current != target)
    {
        (current, previous) = (grid.Neighbours(current).Single(next => next != previous && grid[next] != '#'), current);
        distance++;
        yield return (current, distance);
    }
}

IEnumerable<(int saved, int count)> FindCheats(int cheats, IReadOnlyDictionary<Coordinate, int> path, int min) =>
    from item in path
    let @from = item.Key
    let time = item.Value
    from to in grid.RangeAround(@from, cheats)
    where path.ContainsKey(to)
    let saved = time - path[to] - to.Manhattan(@from)
    where saved >= min
    group (path[@from], path[to]) by saved into g
    select (g.Key, count: g.Count());

readonly record struct Coordinate(int x, int y)
{
    public override string ToString() => $"({x},{y})";
    public int Manhattan(Coordinate other) => Abs(x - other.x) + Abs(y - other.y);
    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
}

class Grid : IReadOnlyDictionary<Coordinate, char>
{
    //        x
    //   +---->
    //   |
    //   |
    // y v
    readonly ImmutableDictionary<Coordinate, char> items;
    readonly Coordinate origin = new(0, 0);
    readonly Coordinate endmarker;
    readonly char empty;
    public int Height => endmarker.y;
    public int Width => endmarker.x;

    public IEnumerable<Coordinate> Keys
    {
        get
        {
            for (int y = origin.y; y < Height; y++)
                for (int x = origin.x; x < endmarker.x; x++)
                    yield return new Coordinate(x, y);
        }
    }

    public IEnumerable<char> Values => Keys.Select(k => this[k]);
    public int Count => Width * Height;

    public Grid(string[] input, char empty = '.') : this(ToDictionary(input, empty), empty, new(input[0].Length, input.Length))
    {
    }

    static ImmutableDictionary<Coordinate, char> ToDictionary(string[] input, char empty) => (
        from y in Range(0, input.Length)
        from x in Range(0, input[y].Length)
        where input[y][x] != empty
        select (x, y, c: input[y][x])).ToImmutableDictionary(t => new Coordinate(t.x, t.y), t => t.c);
    internal Grid(ImmutableDictionary<Coordinate, char> items, char empty, Coordinate endmarker)
    {
        this.items = items;
        this.empty = empty;
        this.endmarker = endmarker;
    }

    public Coordinate Find(char c) => items.Where(i => i.Value == c).First().Key;
    public char this[Coordinate p] => items.TryGetValue(p, out var c) ? c : empty;
    public char this[(int x, int y) p] => this[new Coordinate(p.x, p.y)];
    public char this[int x, int y] => this[new Coordinate(x, y)];
    static (int, int)[] deltas = [(0, 1), (1, 0), (0, -1), (-1, 0)];
    public IEnumerable<Coordinate> Neighbours(Coordinate p) =>
        from d in deltas
        where ContainsKey(p + d)
        select p + d;
    bool IsValid(Coordinate p) => p.x >= 0 && p.y >= 0 && p.x < Width && p.y < Height;
    public IEnumerable<Coordinate> RangeAround(Coordinate p, int range) =>
        from y in Range(-range, 2 * range + 1)
        from x in Range(-range, 2 * range + 1)
        let c = p + (x, y)
        where IsValid(c) && c.Manhattan(p) <= range
        select c;
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
                sb.Append(this[x, y]);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public bool ContainsKey(Coordinate key) => IsValid(key);
    public bool TryGetValue(Coordinate key, [MaybeNullWhen(false)] out char value)
    {
        if (IsValid(key))
        {
            value = this[key];
            return true;
        }

        value = empty;
        return true;
    }

    public IEnumerator<KeyValuePair<Coordinate, char>> GetEnumerator() => Keys.Select(k => new KeyValuePair<Coordinate, char>(k, this[k])).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}