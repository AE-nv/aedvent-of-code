using System.Diagnostics;

var input = File.ReadAllLines("input.txt");
var split = input.IndexOf("");

var ranges = new Range[split];
for (int i = 0; i < split; i++)
{
    ranges[i] = Range.Parse(input[i]);
}
ranges.Sort();

var nofids = input.Length - split - 1;
var ids = new long[nofids];
for (int i = 0; i < nofids; i++)
{
    ids[i] = long.Parse(input[i + split + 1]);
}
ids.Sort();

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);

var part1 = Part1();
Report(1, part1, sw, ref bytes);

var part2 = Part2();
Report(2, part2, sw, ref bytes);

int Part1()
{
    var count = 0;
    var j = 0;
    for (int i = 0; i < ids.Length; i++)
    {
        // advance j to the first range that could contain ids[i]
        while (j < ranges.Length && ranges[j].end < ids[i])
        {
            j++;
        }

        if (j < ranges.Length && ranges[j].Contains(ids[i]))
        {
            count++;
        }
    }
    return count;
}

long Part2()
{
    var merged = ranges[0];
    long count = 0;
    for (int i = 1; i < ranges.Length; i++)
    {
        if (merged.Overlaps(ranges[i]))
        {
            merged = merged.Merge(ranges[i]);
        }
        else
        {
            count += merged.Count;
            merged = ranges[i];
        }
    }

    // last one is either merged or standalone; has to be added in any case
    count += merged.Count;
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

record struct Range(long start, long end) : IComparable<Range>
{
    public static Range Parse(ReadOnlySpan<char> s)
    {
        var split = s.IndexOf('-');
        var start = long.Parse(s[..split]);
        var end = long.Parse(s[++split..]);
        return new Range(start, end);
    }

    public readonly bool Contains(long value) => start <= value && end >= value;
    public readonly bool Overlaps(Range other) => start <= other.end && end >= other.start;
    public readonly Range Merge(Range other) => new Range(Math.Min(start, other.start), Math.Max(end, other.end));
    public int CompareTo(Range other) => start.CompareTo(other.start) switch
    {
        0 => end.CompareTo(other.end),
        var cmp => cmp
    };

    public long Count = end - start + 1;
}