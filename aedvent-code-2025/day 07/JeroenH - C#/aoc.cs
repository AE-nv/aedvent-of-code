using System.Diagnostics;
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);
var part1 = Part1();
Report(1, part1, sw, ref bytes);
var part2 = Part2();
Report(2, part2, sw, ref bytes);
int Part1()
{
    var start = input[0].IndexOf('S');
    HashSet<int> beams = [start];
    HashSet<int> newBeams = [];
    int count = 0;
    for (var row = 1; row < input.Length; row++)
    {
        newBeams.Clear();
        foreach (var x in beams)
        {
            if (input[row][x] == '^')
            {
                newBeams.Add(x - 1);
                newBeams.Add(x + 1);
                count++;
            }
            else
            {
                newBeams.Add(x);
            }
        }
        (beams, newBeams) = (newBeams, beams);
    }
    return count;
}
long Part2()
{
    var start = input[0].IndexOf('S');
    Dictionary<int, long> paths = new(input[0].Length)
    {
        [start] = 1
    };
    Dictionary<int, long> newPaths = new(input[0].Length);
    for (var row = 1; row < input.Length; row++)
    {
        newPaths.Clear();
        foreach (var (x, count) in paths)
        {
            if (input[row][x] == '^')
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