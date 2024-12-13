var stats = new Stats();
var input = File.ReadAllLines("input.txt").First();
ImmutableList<long> list = ReadInput(input);
stats.Report("Init");

var part1 = Solve(25);
stats.Report(1, part1);

var part2 = Solve(75);
stats.Report(2, part2);

ImmutableList<long> ReadInput(string input)
{
    var span = input.AsSpan();
    var ll = new List<long>();
    foreach (var s in span.Split(' '))
    {
        ll.Add(long.Parse(span[s]));
    }

    return [.. ll];
}

long Solve(int iterations)
{
    var current = list.ToDictionary(x => x, _ => 1L);
    for (int i = 0; i < iterations; i++)
    {
        var next = new Dictionary<long, long>(current.Count);
        foreach (var (k, v) in current)
        {
            if (k == 0)
            {
                next[1] = next.GetValueOrDefault(1) + v;
            }
            else
            {
                var digits = GetDigits(k);
                if (digits % 2 == 0)
                {
                    var factor = (long)Pow(10, digits / 2);
                    var (first, second) = (k / factor, k % factor);
                    next[first] = next.GetValueOrDefault(first) + v;
                    next[second] = next.GetValueOrDefault(second) + v;
                }
                else
                {
                    next[k * 2024] = next.GetValueOrDefault(k * 2024) + v;
                }
            }
        }

        current = next;
    }

    return current.Values.Sum();
}

int GetDigits(long n)
{
    int count = 0;
    while (n != 0)
    {
        n /= 10;
        ++count;
    }

    return count;
}