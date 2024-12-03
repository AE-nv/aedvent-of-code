using System.Text.RegularExpressions;

Console.WriteLine(DayThree.PartOne(File.ReadAllText("inputs/3.txt")));
Console.WriteLine(DayThree.PartTwo(File.ReadAllText("inputs/3.txt")));

public static class DayThree
{
    private static readonly Regex Regex = new(@"mul\((\d+),(\d+)\)", RegexOptions.Compiled);

    public static int PartOne(string input) =>
        Regex.Matches(input).Sum(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value));

    public static int PartTwo(string input) =>
        PartOne(string.Join("", input.Split("do()").Select(x => x.Split("don't()").First())));
}