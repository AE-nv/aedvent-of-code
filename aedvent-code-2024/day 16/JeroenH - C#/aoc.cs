using System.Diagnostics.CodeAnalysis;
using System.Collections;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");

Grid grid = new Grid(input);
stats.Report("Init");

var (part1, part2) = Solve(grid);
stats.Report($"part1: {part1}, part2: {part2}");

(int bestScore, int tileCount) Solve(Grid grid)
{
    var start = grid.Find('S');
    var end = grid.Find('E');
    var priorityQueue = new PriorityQueue<(State state, int score), int>();
    var visited = new Dictionary<State, int>();
    var path = new Dictionary<State, List<State>>();
    int bestScore = int.MaxValue;
    priorityQueue.Enqueue((new State(start, Direction.E), 0), 0);
    while (priorityQueue.Count > 0)
    {
        var (state, score) = priorityQueue.Dequeue();
        if (visited.TryGetValue(state, out var prevScore) && prevScore <= score)
        {
            continue;
        }

        visited[state] = score;
        if (state.c == end)
        {
            bestScore = Min(bestScore, score);
            continue;
        }

        foreach (var (s, q) in Next(state))
        {
            priorityQueue.Enqueue((s, score + q), score + q);
            if (!path.TryGetValue(s, out var value))
            {
                value = [];
                path[s] = value;
            }

            value.Add(state);
        }
    }

    return (bestScore, GetTiles(path, visited, bestScore, end).Count);
}

IEnumerable<(State state, int score)> Next(State state)
{
    var (coordinate, heading) = state;
    var next = coordinate + heading;
    if (grid.ContainsKey(next) && grid[next] != '#')
    {
        yield return (new State(next, heading), 1);
        next += heading;
    }

    yield return (state.TurnLeft(), 1000);
    yield return (state.TurnRight(), 1000);
}

HashSet<Coordinate> GetTiles(Dictionary<State, List<State>> path, Dictionary<State, int> visited, int bestScore, Coordinate end)
{
    var result = new HashSet<Coordinate>();
    var stack = new Stack<State>();
    var seen = new HashSet<State>();
    foreach (var (state, score) in visited)
    {
        if (state.c == end && score == bestScore)
        {
            stack.Push(state);
        }
    }

    while (stack.Count > 0)
    {
        var current = stack.Pop();
        if (seen.Contains(current))
        {
            continue;
        }

        seen.Add(current);
        result.Add(current.c);
        foreach (var parent in path[current])
        {
            var parentScore = visited[parent];
            if (parentScore + 1 == visited[current] || parentScore + 1000 == visited[current])
            {
                stack.Push(parent);
            }
        }
    }

    return result;
}

record struct State(Coordinate c, Direction facing)
{
    public State TurnRight() => this with
    {
        facing = Turn(1)
    };
    public State TurnLeft() => this with
    {
        facing = Turn(-1)
    };
    private Direction Turn(int n) => (Direction)(((int)facing + n + 4) % 4);
}

readonly record struct Coordinate(int x, int y)
{
    public static Coordinate Origin = new(0, 0);
    public override string ToString() => $"{x},{y}";
    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
    public static Coordinate operator +(Coordinate left, Direction d) => d switch
    {
        Direction.N => left + (0, -1),
        Direction.E => left + (1, 0),
        Direction.S => left + (0, 1),
        Direction.W => left + (-1, 0)
    };
    public static Coordinate operator -(Coordinate left, Direction d) => left + (d switch
    {
        Direction.N => (0, 1),
        Direction.E => (-1, 0),
        Direction.S => (0, -1),
        Direction.W => (1, 0)
    });
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
                for (int x = origin.x; x < Width; x++)
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
        for (int y = origin.y; y < Height; y++)
        {
            for (int x = origin.x; x < Width; x++)
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

enum Direction
{
    N,
    E,
    S,
    W
}