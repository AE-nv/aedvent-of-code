using System.Diagnostics;
var input = File.ReadAllLines("input.txt");
var (sw, bytes) = (Stopwatch.StartNew(), 0L);
Report(0, "", sw, ref bytes);

var part1 = input.Sum(l => GetLargestNDigitNumber(l, 2));
Report(1, part1, sw, ref bytes);

var part2 = input.Sum(l => GetLargestNDigitNumber(l, 12));
Report(2, part2, sw, ref bytes);

long GetLargestNDigitNumber(ReadOnlySpan<char> input, int n)
{
    // Construct the largest possible n-digit number from the digits in the input string.
    // For each position in the result, select largest available digit that leaves
    // enough remaining digits for the subsequent positions.
    long result = 0; // Accumulates the resulting n-digit number
    int start = 0; // Starting index for the current search range in the input
    for (int i = 0; i < n; i++) // Loop for each digit position in the result
    {
        // Calculate the end index: we can search up to input.Length - (n - i) to ensure
        // there are at least (n - i - 1) digits left after this position for the remaining positions
        int end = input.Length - (n - i);
        int max = -1; // Tracks the maximum digit found in the current search range
        for (int j = start; j <= end; j++) // Search from start to end for the largest digit
        {
            int digit = input[j] - '0'; // Convert char digit to int
            if (digit > max)
            {
                max = digit;
                start = j + 1; // Start search for next digit after this one
                if (digit == 9) // If we found a '9', we can't do better, so break early
                    break;
            }
        }

        // Append the max digit to the result
        result = result * 10 + max;
    }

    return result;
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