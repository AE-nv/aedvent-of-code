var stats = new Stats();

var input = File.ReadAllLines("input.txt");
var antennas = (
    from line in input.Index()
    from value in line.Item.Index()
    where value.Item != '.'
    let c = new Coordinate(value.Index, line.Index)
    group c by value.Item into g
    from p in g.Combinations()
    select p).ToList();
stats.Report("Init");

var part1 = (
    from p in antennas
    from a in GetAntinodes1(p)
    select a).Distinct().Count();
stats.Report(1, part1);

var part2 = (
    from p in antennas
    from a in GetAntinodes2(p)
    select a).Distinct().Count();
stats.Report(2, part2);

bool IsValid(Coordinate p) => p.x >= 0 && p.x < input[0].Length && p.y >= 0 && p.y < input.Length;

IEnumerable<Coordinate> GetAntinodes1((Coordinate c1, Coordinate c2) pair)
{
    var (c1, c2) = pair;
    var delta = c2 - c1;
    if (IsValid(c1 - delta))
        yield return c1 - delta;
    if (IsValid(c2 + delta))
        yield return c2 + delta;
}

IEnumerable<Coordinate> GetAntinodes2((Coordinate c1, Coordinate c2) pair)
{
    var (c1, c2) = pair;
    var delta = c2 - c1;
    return GetEquidistantPoints(c1, -1 * delta).Concat(GetEquidistantPoints(c2, delta));
}

IEnumerable<Coordinate> GetEquidistantPoints(Coordinate c, Delta d)
{
    while (IsValid(c))
    {
        yield return c;
        c += d;
    }
}

readonly record struct Coordinate(int x, int y)
{
    public static Delta operator -(Coordinate left, Coordinate right) => new(left.x - right.x, left.y - right.y);
    public static Coordinate operator +(Coordinate left, Delta p) => new(left.x + p.dx, left.y + p.dy);
    public static Coordinate operator -(Coordinate left, Delta p) => new(left.x - p.dx, left.y - p.dy);
}

readonly record struct Delta(int dx, int dy)
{
    public static Delta operator *(int n, Delta d) => new(d.dx * n, d.dy * n);
}

static class LinqEx
{
    public static IEnumerable<(T first,T second)> Combinations<T>(this IEnumerable<T> items)
    {
        if (items is not IReadOnlyList<T> list) list = items.ToList();
            
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = i + 1; j < list.Count; j++)
            {
                yield return (list[i], list[j]);
            }
        }
    }

}