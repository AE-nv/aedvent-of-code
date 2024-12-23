using System.Runtime.CompilerServices;
using Sequence = (byte, byte, byte, byte);

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
long[] numbers = input.Select(long.Parse).ToArray();
stats.Report("Init");

var part1 = numbers.Select(n => GetSecret(n, 2000)).Sum();
stats.Report(1, part1);

var part2 = Part2();
stats.Report(2, part2);

long GetSecret(long secret, int n)
{
    for (int i = 0; i < n; i++)
    {
        secret = Next(secret);
    }

    return secret;
}

[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
static long Next(long secret)
{
    secret = ((secret <<  6) ^ secret) & (0x1000000 - 1);
    secret = ((secret >>  5) ^ secret) & (0x1000000 - 1);
    secret = ((secret << 11) ^ secret) & (0x1000000 - 1);
    return secret;
}

int Part2()
{
    var totals = new Dictionary<Sequence, int>();
    Span<byte> prices = new byte[2000];
    var seen = new HashSet<Sequence>();
    foreach (var n in numbers)
    {
        seen.Clear();
        var next = n;
        prices[0] = (byte)(next % 10);
        for (int i = 1; i < prices.Length; i++)
        {
            next = Next(next);
            prices[i] = (byte)(next % 10);
        }

        for (int i = 4; i < prices.Length; i++)
        {
            var span = prices[(i - 4)..(i + 1)];
            var diff = ((byte)(span[1] - span[0]), (byte)(span[2] - span[1]), (byte)(span[3] - span[2]), (byte)(span[4] - span[3]));
            if (seen.Add(diff))
            {
                if (!totals.ContainsKey(diff))
                    totals[diff] = 0;
                totals[diff] += prices[i];
            }
        }
    }

    return totals.Values.Max();
}