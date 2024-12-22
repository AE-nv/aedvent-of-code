using System.Diagnostics.CodeAnalysis;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
var codes = input;
var numeric = new Grid(["789", "456", "123", " 0A"]);
var directional = new Grid([" ^A", "<v>"]);
Dictionary<(Coordinate, Coordinate, int depth), long> cache = [];
stats.Report("Init");

var part1 = MinimumButtonPresses(GetKeypads(2));
stats.Report(1, part1);

var part2 = MinimumButtonPresses(GetKeypads(25));
stats.Report(2, part2);

long MinimumButtonPresses(Grid[] keypads) => codes.Select(code => int.Parse(code[..^1]) * FindDirectionKeySequence(code, keypads, cache)).Sum();

Grid[] GetKeypads(int n) => Repeat(directional, n).Prepend(numeric).ToArray();

long FindDirectionKeySequence(ReadOnlySpan<char> code, ReadOnlySpan<Grid> keypads, Dictionary<(Coordinate, Coordinate, int), long> cache)
{
    if (keypads.Length == 0)
    {
        return code.Length;
    }
    else
    {
        var current = 'A';
        var length = 0L;
        foreach (var next in code)
        {
            length += FindSequence(keypads[0].Find(current), keypads[0].Find(next), keypads, cache);
            current = next;
        }

        return length;
    }
}

long FindSequence(Coordinate current, Coordinate next, ReadOnlySpan<Grid> keypads, Dictionary<(Coordinate, Coordinate, int), long> cache)
{
    if (cache.TryGetValue((current, next, keypads.Length), out var cost))
        return cost;
    var keypad = keypads[0];
    var (dx, dy) = next - current;
    var v = new string(dy > 0 ? 'v' : '^', Abs(dy));
    var h = new string(dx < 0 ? '<' : '>', Abs(dx));
    cost = long.MaxValue;
    if (keypad[current + (0, dy)] != ' ')
    {
        cost = Min(cost, FindDirectionKeySequence($"{v}{h}A", keypads[1..], cache));
    }

    if (keypad[current + (dx, 0)] != ' ')
    {
        cost = Min(cost, FindDirectionKeySequence($"{h}{v}A", keypads[1..], cache));
    }

    cache[(current, next, keypads.Length)] = cost;
    return cost;
}

record struct Coordinate(int x, int y)
{
    public static (int dx, int dy) operator -(Coordinate a, Coordinate b) => (a.x - b.x, a.y - b.y);
    public static Coordinate operator +(Coordinate a, (int dx, int dy) b) => new(a.x + b.dx, a.y + b.dy);
}

class Grid : IReadOnlyDictionary<Coordinate, char>
{
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
    bool IsValid(Coordinate p) => p.x >= 0 && p.y >= 0 && p.x < Width && p.y < Height;
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