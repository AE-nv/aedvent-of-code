var input = File.ReadAllText("input.txt");
var sw = Stopwatch.StartNew();
var part1 = GetResult(input, false);
var part2 = GetResult(input, true);
Output.WriteResult(part1, part2, sw.Elapsed);
int GetResult(ReadOnlySpan<char> span, bool activeOnly)
{
    int result = 0;
    bool shouldreturn = true;
    foreach (var m in Regexes.AoC202403Regex().EnumerateMatches(span))
    {
        var slice = span.Slice(m.Index, m.Length);
        shouldreturn = !activeOnly || slice switch
        {
            "do()" => true,
            "don't()" => false,
            _ => shouldreturn
        };
        if (shouldreturn && slice.StartsWith("mul("))
        {
            var openbracket = slice.IndexOf('(');
            var comma = slice.IndexOf(',');
            var closebracket = slice.IndexOf(')');
            var left = int.Parse(slice[(openbracket + 1)..comma]);
            var right = int.Parse(slice[(comma + 1)..closebracket]);
            result += left * right;
        }
    }

    return result;
}

static partial class Regexes
{
    [GeneratedRegex(@"(mul\((?<left>\d+),(?<right>\d+)\)|do\(\)|don't\(\))")]
    public static partial Regex AoC202403Regex();
}
static class Output
{
    internal static void WriteResult<T1, T2>(T1 part1, T2 part2, TimeSpan time)
    {
        Console.WriteLine($"+".PadRight(39, '-') + "+");
        Console.WriteLine($"| Part 1    | {part1}".PadRight(39) + "|");
        Console.WriteLine($"| Part 2    | {part2}".PadRight(39) + "|");
        Console.WriteLine($"| Time      | {time.FormatTime()}".PadRight(39) + "|");
        Console.WriteLine($"| Allocated | {GC.GetTotalAllocatedBytes().FormatBytes()}".PadRight(39) + "|");
        Console.WriteLine($"+".PadRight(39, '-') + "+");
    }
    static string FormatBytes(this long b)
    {
        double bytes = b;
        string[] sizes = ["B", "KB", "MB", "GB", "TB"];
        int n = 0;
        while (bytes >= 1024 && n < sizes.Length - 1)
        {
            n++;
            bytes /= 1024;
        }
        return $"{bytes:0.00} {sizes[n]}";
    }
    static string FormatTime(this TimeSpan timespan) => timespan switch
    {
        { TotalHours: > 1 } ts => $@"{ts:hh\:mm\:ss}",
        { TotalMinutes: > 1 } ts => $@"{ts:mm\:ss}",
        { TotalSeconds: > 10 } ts => $"{ts.TotalSeconds} s",
        { TotalSeconds: > 1 } ts => $@"{ts:ss\.fff} s",
        { TotalMilliseconds: > 10 } ts => $"{ts.TotalMilliseconds:0.0} ms",
        { TotalMilliseconds: > 1 } ts => $"{ts.TotalMicroseconds:0.0} μs",
        TimeSpan ts => $"{ts.TotalNanoseconds} ns"
    };
}