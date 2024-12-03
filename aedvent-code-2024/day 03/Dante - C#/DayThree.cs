using System.Text.RegularExpressions;

Console.WriteLine(DayThree.PartOne(File.ReadAllText("inputs/3.txt")));
Console.WriteLine(DayThree.PartTwo(File.ReadAllText("inputs/3.txt")));

public static class DayThree
{
    private static readonly Regex DoRegex = new(@"do\(\)", RegexOptions.Compiled);
    private static readonly Regex DontRegex = new(@"don\'t\(\)", RegexOptions.Compiled);
    private static readonly Regex MulRegex = new(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);

    public static int PartOne(string input)
    {
        return MulRegex.Matches(input).Sum(x => Mul(x.Groups[1].Value, x.Groups[2].Value));
    }

    public static int PartTwo(string input)
    {
        var doPositions = DoRegex.Matches(input).Select(x => x.Index);
        var dontPositions = DontRegex.Matches(input).Select(x => x.Index);

        var merged = doPositions
            .Prepend(0) // implicit do at start
            .Select(x => new { StartIndex = x, IsDo = true })
            .Concat(dontPositions.Select(x => new { StartIndex = x, IsDo = false }))
            .OrderBy(x => x.StartIndex)
            .Append(new { StartIndex = int.MaxValue, IsDo = false }) // add int maxvalue end to make ranges later
            .ToList();

        // make ranges [a..b], [b..c], [c..d] etc and take only the "do" ranges
        var doRanges = merged
            .Zip(merged.Skip(1), (a, b) => new { Range = new Range(a.StartIndex, b.StartIndex), IsDo = a.IsDo })
            .Where(x => x.IsDo)
            .Select(x => x.Range)
            .ToList();

        return MulRegex.Matches(input)
            .Where(m => doRanges.Any(r => m.Index.IsInRange(r))) // take out the matches that fall in "do" ranges
            .Sum(x => Mul(x.Groups[1].Value, x.Groups[2].Value));
    }

    private static int Mul(string l, string r) => int.Parse(l) * int.Parse(r);
    private static bool IsInRange(this int value, Range range) => value >= range.Start.Value && value < range.End.Value;
}