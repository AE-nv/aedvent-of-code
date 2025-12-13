using System.Diagnostics;
using static System.Linq.Enumerable;

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
var iterations = 1000;
var coordinates = input.Select(line => Coordinate.Parse(line)).ToArray();
var pairs = (
    from i in Range(0, coordinates.Length) from j in Range(i + 1, coordinates.Length - i - 1) orderby coordinates[i].DistanceSquared(coordinates[j]) select (coordinates[i], coordinates[j])).ToArray();
var circuitManager = new CircuitManager(coordinates);
Report(0, "", sw, ref bytes);
var part1 = Part1();
Report(1, part1, sw, ref bytes);
var part2 = Part2();
Report(2, part2, sw, ref bytes);
int Part1()
{
    foreach (var (c1, c2) in pairs[..iterations])
    {
        circuitManager.Merge(c1, c2);
    }

    return circuitManager.GetUniqueCircuits().OrderByDescending(c => c.Size).Take(3).Aggregate(1, (acc, circuit) => acc * circuit.Size);
}

long Part2()
{
    (Coordinate c1, Coordinate c2) last = default;
    foreach (var (c1, c2) in pairs[iterations..])
    {
        if (circuitManager.Merge(c1, c2))
        {
            last = (c1, c2);
            var uniqueCircuits = circuitManager.GetUniqueCircuits().Count();
            if (uniqueCircuits == 1)
                break;
        }
    }

    return 1L * last.c1.X * last.c2.X;
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

record struct Coordinate(int X, int Y, int Z)
{
    public static Coordinate Parse(ReadOnlySpan<char> s)
    {
        var parts = new Range[3];
        s.Split(parts, ',');
        return new Coordinate(int.Parse(s[parts[0]]), int.Parse(s[parts[1]]), int.Parse(s[parts[2]]));
    }

    public double DistanceSquared(Coordinate other) => Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2);
}

record Circuit(HashSet<Coordinate> Members)
{
    public int Size => Members.Count;

    public bool Contains(Coordinate coord) => Members.Contains(coord);
    public Circuit MergeWith(Circuit other)
    {
        var merged = new HashSet<Coordinate>(Members);
        merged.UnionWith(other.Members);
        return new Circuit(merged);
    }
}

class CircuitManager
{
    private Dictionary<Coordinate, Circuit> coordinateToCircuit = [];
    public CircuitManager(Coordinate[] coordinates)
    {
        foreach (var coord in coordinates)
        {
            coordinateToCircuit[coord] = new Circuit([coord]);
        }
    }

    public bool Merge(Coordinate c1, Coordinate c2)
    {
        var circuit1 = coordinateToCircuit[c1];
        var circuit2 = coordinateToCircuit[c2];
        if (circuit1 == circuit2)
            return false;
        var merged = circuit1.MergeWith(circuit2);
        foreach (var coord in merged.Members)
        {
            coordinateToCircuit[coord] = merged;
        }

        return true;
    }

    public IEnumerable<Circuit> GetUniqueCircuits() => coordinateToCircuit.Values.Distinct();
}