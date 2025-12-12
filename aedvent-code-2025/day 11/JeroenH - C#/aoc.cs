using System.Collections.Generic;
using System.Diagnostics;
using Graph = System.Collections.Generic.Dictionary<string, string[]>;

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
Graph graph = input.Select(l => l.Split(": ")).ToDictionary(p => p[0], p => p[1].Split(' '));
Report(0, "", sw, ref bytes);
var part1 = Count("you", graph, [], 0);
Report(1, part1, sw, ref bytes);
var part2 = Count("svr", graph, [], 0b100);
Report(2, part2, sw, ref bytes);
long Count(string node, Graph graph, Dictionary<(string, int), long> cache, int state)
{
    state = (state & 0b100, node) switch
    {
        (0, _) => 0,
        (_, "dac") => state | 0b01,
        (_, "fft") => state | 0b10,
        _ => state
    };
    var value = node switch
    {
        "out" => state == 0 || (state & 0b111) == 0b111 ? 1 : 0,
        _ when cache.TryGetValue((node, state), out var cached) => cached,
        _ => graph[node].Select(n => Count(n, graph, cache, state)).Sum()
    };
    cache[(node, state)] = value;
    return value;
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