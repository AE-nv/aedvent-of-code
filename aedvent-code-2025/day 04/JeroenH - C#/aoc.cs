using System.Diagnostics;
var input = File.ReadAllLines("input.txt");
var writer = Console.Out;
Grid grid = new Grid(input);
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);
var part1 = grid.Keys.Count(CanBeRemoved);
Report(1, part1, sw, ref bytes);
var part2 = Part2();
Report(2, part2, sw, ref bytes);
bool CanBeRemoved(Coordinate pos) => grid[pos] == '@' && grid.CountNeighbours(pos) < 4;
int Part2()
{
    var count = 0;
    List<Coordinate> toRemove;
    while ((toRemove = grid.FindAll(CanBeRemoved)).Any())
    {
        foreach (var p in toRemove)
        {
            grid[p] = '.';
            count++;
        }
    }

    return count;
}

void Report<T>(int part, T value, Stopwatch sw, ref long bytes)
{
    var label = part switch
    {
        1 => $"Part 1: [{value}]",
        2 => $"Part 2: [{value}]",
        _ => "Init"
    };
    var time = sw.Elapsed switch
    {
        { TotalMicroseconds: < 1 } => $"{sw.Elapsed.TotalNanoseconds:N0} ns",
        { TotalMilliseconds: < 1 } => $"{sw.Elapsed.TotalMicroseconds:N0} µs",
        { TotalSeconds: < 1 } => $"{sw.Elapsed.TotalMilliseconds:N0} ms",
        _ => $"{sw.Elapsed.TotalSeconds:N2} s"
    };
    var newbytes = GC.GetTotalAllocatedBytes(false);
    var memory = (newbytes - bytes) switch
    {
        < 1024 => $"{newbytes - bytes} B",
        < 1024 * 1024 => $"{(newbytes - bytes) / 1024:N0} KB",
        _ => $"{(newbytes - bytes) / (1024 * 1024):N0} MB"
    };
    Console.WriteLine($"{label} ({time} - {memory})");
    bytes = newbytes;
}

readonly record struct Coordinate(int x, int y)
{
    public static Coordinate Origin = new(0, 0);
    public override string ToString() => $"({x},{y})";
    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
}

class Grid
{
    readonly char[,] items;
    readonly char empty = '.';
    public readonly int Height;
    public readonly int Width;
    public IEnumerable<Coordinate> Keys
    {
        get
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yield return new Coordinate(x, y);
        }
    }

    public Grid(string[] input, char empty = '.')
    {
        this.empty = empty;
        Height = input.Length;
        Width = input[0].Length;
        items = new char[Height, Width];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                items[y, x] = input[y][x];
            }
        }
    }

    public char this[Coordinate p] { get => items[p.y, p.x]; set => items[p.y, p.x] = value; }

    public int CountNeighbours(Coordinate p)
    {
        Span<(int dx, int dy)> deltas = [(0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1)];
        var (x, y) = p;
        int count = 0;
        foreach (var (dx, dy) in deltas)
        {
            var (nx, ny) = (x + dx, y + dy);
            if (nx >= 0 && ny >= 0 && nx < Width && ny < Height && this[new Coordinate(nx, ny)] != empty)
                count++;
        }

        return count;
    }

    public List<Coordinate> FindAll(Func<Coordinate, bool> predicate)
    {
        var result = new List<Coordinate>();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var coord = new Coordinate(x, y);
                if (predicate(coord))
                    result.Add(coord);
            }
        }

        return result;
    }
}