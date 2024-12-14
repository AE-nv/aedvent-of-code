using Z3.Linq;

var stats = new Stats();
var input = File.OpenRead("input.txt");
ImmutableArray<Machine> items = ReadInput(input).ToImmutableArray();
stats.Report("Init");

var part1 = items.Select(m => m.Solve(Delta.Zero)).Sum();
stats.Report(1, part1);

var part2 = items.Select(m => m.Solve(Delta.Square(10_000_000_000_000))).Sum();
stats.Report(2, part2);

IEnumerable<Machine> ReadInput(Stream stream)
{
    using var sr = new StreamReader(stream);
    while (!sr.EndOfStream)
    {
        var buttonA = Regexes.Button().Match(sr.ReadLine()!);
        var buttonB = Regexes.Button().Match(sr.ReadLine()!);
        var prize = Regexes.Prize().Match(sr.ReadLine()!);
        
        yield return new Machine(
                new Delta(int.Parse(buttonA.Groups["dx"].Value), int.Parse(buttonA.Groups["dy"].Value)),
                new Delta(int.Parse(buttonB.Groups["dx"].Value), int.Parse(buttonB.Groups["dy"].Value)),
                new Coordinate(int.Parse(prize.Groups["x"].Value), int.Parse(prize.Groups["y"].Value)));
        sr.ReadLine();
    }
}

readonly record struct Coordinate(long x, long y)
{
    public static Coordinate operator +(Coordinate left, Delta d) => new(left.x + d.dx, left.y + d.dy);
}

readonly record struct Delta(long dx, long dy)
{
    public readonly static Delta Zero = default;
    public static Delta Square(long value) => new(value, value);
}

readonly record struct Machine(Delta A, Delta B, Coordinate Prize)
{
    public long Solve(Delta delta)
    {
        var ((dxa, dya), (dxb, dyb), (x, y)) = (A, B, Prize + delta);
        using var z3 = new Z3Context();
        var theorem =
            from _ in z3.NewTheorem<(long a, long b)>()
            where _.a * dxa + _.b * dxb == x
            where _.a * dya + _.b * dyb == y
            orderby 3 * _.a + _.b descending
            select _;
        (long a, long b) = theorem.Solve();
        return 3 * a + b;
    }
}

static partial class Regexes
{
    [GeneratedRegex(@"Button [AB]: X(?<dx>[+\-\d]+), Y(?<dy>[+\-\d]+)")]
    public static partial Regex Button();
    [GeneratedRegex(@"Prize: X=(?<x>[\d]+), Y=(?<y>[\d]+)")]
    public static partial Regex Prize();
}