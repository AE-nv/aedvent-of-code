using System.Diagnostics;
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
TachyonManifold manifold = new (input);

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Logger.Report(0, "", sw, ref bytes);

var part1 = BeamSplitCounter.CountSplits(manifold);
Logger.Report(1, part1, sw, ref bytes);

var part2 = QuantumTimelineCounter.CountTimelines(manifold);
Logger.Report(2, part2, sw, ref bytes);


record struct Beam(int Position)
{
    public static Beam At(Coordinate c) => new(c.x);
    public Coordinate At(int row) => new(Position, row);
    public (Beam Left, Beam Right) Split() => (Left, Right);
    Beam Left => this with { Position = Position - 1 };
    Beam Right => this with { Position = Position + 1 };
}
record struct Coordinate(int x, int y)
{
    public static Coordinate At(int x, int y) => new(x, y);
}

static class Extensions
{
    extension<T>(HashSet<T> set)
    {
        public void Add((T left, T right) pair)
        {
            set.Add(pair.left);
            set.Add(pair.right);
        }
    }
}

class TachyonManifold(string[] lines)
{
    private const char SplitChar = '^';
    public int Width => lines[0].Length;
    public int Height => lines.Length;
    public Coordinate Origin => new(lines[0].IndexOf('S'), 0);
    public bool IsSplit(Coordinate c) => lines[c.y][c.x] == SplitChar;
}

static class BeamSplitCounter
{
    internal static long CountSplits(TachyonManifold manifold)
    {
        HashSet<Beam> beams = [Beam.At(manifold.Origin)];
        HashSet<Beam> newBeams = [];
        long splits = 0;
        for (int row = 1; row < manifold.Height; row++)
        {
            newBeams.Clear();
            foreach (var beam in beams)
            {
                if (manifold.IsSplit(beam.At(row)))
                {
                    newBeams.Add(beam.Split());
                    splits++;
                }
                else
                {
                    newBeams.Add(beam);
                }
            }
            (beams, newBeams) = (newBeams, beams);
        }
        return splits;
    }
}

static class QuantumTimelineCounter
{
    internal static long CountTimelines(TachyonManifold manifold)
    {
        var start = manifold.Origin.x;
        Dictionary<int, long> paths = new(manifold.Width)
        {
            [start] = 1
        };
        Dictionary<int, long> newPaths = new(manifold.Width);
        for (var row = 1; row < manifold.Height; row++)
        {
            newPaths.Clear();
            foreach (var (x, count) in paths)
            {
                if (manifold.IsSplit(Coordinate.At(x, row)))
                {
                    // split: each path branches into two
                    newPaths[x - 1] = (newPaths.TryGetValue(x - 1, out var left) ? left : 0) + count;
                    newPaths[x + 1] = (newPaths.TryGetValue(x + 1, out var right) ? right : 0) + count;
                }
                else
                {
                    newPaths[x] = (newPaths.TryGetValue(x, out var value) ? value : 0) + count;
                }
            }

            (paths, newPaths) = (newPaths, paths);
        }

        return paths.Values.Sum();
    }
}

static class Logger
{
    public static void Report<T>(int part, T value, Stopwatch sw, ref long bytes)
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
}
