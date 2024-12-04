Console.WriteLine(DayOne.PartTwo(File.ReadAllLines("inputs/1.txt")));
Console.WriteLine(DayOne.PartTwo(File.ReadAllLines("inputs/1.txt")));

public static class DayOne
{
    public static int PartOne(string[] lines)
    {
        var (left, right) = SplitLists(lines);
        return left.Order().Zip(right.Order()).Sum(x => Math.Abs(x.First - x.Second));
    }

    public static int PartTwo(string[] lines)
    {
        var (left, right) = SplitLists(lines);
        return left.Sum(l => l * right.Count(r => r == l));
    }

    private static (List<int>, List<int>) SplitLists(string[] lines) => lines.Aggregate((new List<int>(), new List<int>()), (acc, line) =>
    {
        var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        acc.Item1.Add(int.Parse(parts[0]));
        acc.Item2.Add(int.Parse(parts[1]));
        return acc;
    });
}