using System.Diagnostics;

var input = File.ReadAllLines("input.txt");

int[] instructions = [.. input.Select(line => (line[0] == 'L' ? -1 : 1) * int.Parse(line[1..]))];
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);

var part1 = instructions.Aggregate((value: 50, password: 0), (acc, i) =>
{
    var next = (acc.value + 100 + i) % 100;
    var increment = next == 0 ? 1 : 0;
    return acc with
    {
        value = next,
        password = acc.password + increment
    };
}).password;
Report(1, part1, sw, ref bytes);

var part2 = instructions.Aggregate((value: 50, password: 0), (acc, i) =>
{
    int dir = Math.Sign(i);
    int steps = Math.Abs(i);
    int count = 0;
    for (int k = 1; k <= steps; k++)
    {
        int next = ((acc.value + k * dir) % 100 + 100) % 100;
        if (next == 0)
            count++;
    }

    int final = ((acc.value + steps * dir) % 100 + 100) % 100;
    return (value: final, password: acc.password + count);
}).password;
Report(2, part2, sw, ref bytes);

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