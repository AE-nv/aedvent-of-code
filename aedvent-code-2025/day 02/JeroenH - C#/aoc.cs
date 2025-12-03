using System.Diagnostics;

var input = File.ReadAllLines("input.txt");
RepeatedDigits[] ranges = [.. input[0].Split(',').Select(RepeatedDigits.Parse)];
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);

var part1 = ranges.SelectMany(r => r.Get(RepetitionCount.Two)).Sum();
Report(1, part1, sw, ref bytes);

var part2 = ranges.SelectMany(r => r.Get(RepetitionCount.Any)).Distinct().Sum();
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

readonly record struct RepeatedDigits(long start, long end)
{
    public static RepeatedDigits Parse(string s)
    {
        var parts = s.Split('-');
        var start = long.Parse(parts[0]);
        var end = long.Parse(parts[1]);
        return new RepeatedDigits(start, end);
    }

    public IEnumerable<long> Get(RepetitionCount mode)
    {
        var nofDigitsStart = NumDigits(start);
        var nofDigitsEnd = NumDigits(end);
        for (int nofDigits = nofDigitsStart; nofDigits <= nofDigitsEnd; nofDigits++)
        {
            var maxRepetitions = mode == RepetitionCount.Two ? 2 : nofDigits;
            for (int repetitionCount = 2; repetitionCount <= maxRepetitions; repetitionCount++)
            {
                if (nofDigits % repetitionCount != 0)
                    continue; // nofDigits has to be divisible by repetitionCount
                int sequenceLength = nofDigits / repetitionCount;
                // to repeat a base number b with l digits exactly r times to form a number S:
                // S = b * 10^(0*l) + b * 10^(1*l) + ... + b * 10^((r-1)*l)
                // => S = b * (10^(0*l) + b * 10^(1*l) + ... + b * 10^((r-1)*l))
                // => multiplier m = (10^(0*l) + b * 10^(1*l) + ... + b * 10^((r-1)*l))
                // e.g. 1234 repeated 3 times: b = 1234, l = 4, r = 3
                // m:
                //  i = 0: 10^0 
                //  i = 1: 10^4  
                //  i = 2: 10^8 
                // m = 100010001
                // S = b * m = 1234 * 100010001 = 123412341234
                long multiplier = 0;
                for (int i = 0; i < repetitionCount; i++)
                {
                    multiplier += Pow10(i * sequenceLength);
                }

                // Find bounds for the base sequence
                long min = Pow10(sequenceLength - 1); // smallest number with sequenceLength digits (e.g. 1000 for 4 digits)
                long max = Pow10(sequenceLength) - 1; // largest number with sequenceLength digits (e.g. 9999 for 4 digits)
                long bMin = LowerBound(multiplier, min, max);
                long bMax = UpperBound(multiplier, min, max);
                if (bMin > bMax)
                    continue;
                for (long b = bMin; b <= bMax; b++)
                {
                    yield return b * multiplier;
                }
            }
        }
    }

    // Find smallest base such that base * multiplier >= start and min <= base <= max
    private long LowerBound(long multiplier, long min, long max)
    {
        if (min * multiplier >= start) // start of the range is smaller than the minimum value
        {
            return min;
        }

        long b = (start + multiplier - 1) / multiplier; // ceiling division
        if (b < min)
            return min;
        if (b > max)
            return max;
        return b;
    }

    // find largest base such that base * multiplier <= end and min <= base <= max
    private long UpperBound(long multiplier, long min, long max)
    {
        long b = end / multiplier; // floor division
        if (b < min)
            return min - 1;
        if (b > max)
            return max;
        return b;
    }

    private static long Pow10(int exp)
    {
        long result = 1;
        for (int i = 0; i < exp; i++)
        {
            result *= 10;
        }

        return result;
    }

    private int NumDigits(long n)
    {
        if (n == 0)
            return 0;
        var count = 0;
        while (n > 0)
        {
            count++;
            n /= 10;
        }

        return count;
    }
}

enum RepetitionCount
{
    Two,
    Any
}