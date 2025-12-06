using System.Diagnostics;

var input = File.ReadAllLines("input.txt");
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);

var part1 = Part1();
Report(1, part1, sw, ref bytes);

var part2 = Part2();
Report(2, part2, sw, ref bytes);

long Part1()
{
    ReadOnlySpan<char> operations = input[^1];
    long total = 0;
    List<long> numbers = [];
    for (var column = 0; column < operations.Length;)
    {
        var next = column + 1;
        while (next < operations.Length && operations[next] == ' ')
            next++;
        var operation = operations[column];
        for (int i = 0; i < input.Length - 1; i++)
        {
            ReadOnlySpan<char> line = input[i];
            numbers.Add(long.Parse(line[column..next]));
        }

        var result = operation switch
        {
            '+' => numbers.Sum(),
            '*' => numbers.Aggregate(1L, (a, b) => a * b),
            _ => throw new InvalidOperationException()
        };
        numbers.Clear();
        total += result;
        column = next;
    }

    return total;
}

long Part2()
{
    ReadOnlySpan<char> operations = input[^1];
    long total = 0;
    List<long> numbers = [];
    for (var column = 0; column < operations.Length;)
    {
        var next = column + 1;
        while (next < operations.Length && operations[next] == ' ')
            next++;
        if (next == operations.Length)
            next++; // there is no space at the end
        var operation = operations[column];
        for (int i = next - 2; i >= column; i--)
        {
            var number = 0L;
            var multiplier = 1;
            for (int j = input.Length - 2; j >= 0; j--)
            {
                var digit = input[j][i];
                if (digit == ' ')
                    continue;
                number += (digit - '0') * multiplier;
                multiplier *= 10;
            }

            numbers.Add(number);
        }

        var result = operation switch
        {
            '+' => numbers.Sum(),
            '*' => numbers.Aggregate(1L, (a, b) => a * b),
            _ => throw new InvalidOperationException()
        };
        numbers.Clear();
        total += result;
        column = next;
    }

    return total;
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