Console.WriteLine(DayTwo.PartOne(File.ReadAllLines("inputs/2.txt")));
Console.WriteLine(DayTwo.PartTwo(File.ReadAllLines("inputs/2.txt")));

public static class DayTwo
{
    public static int PartOne(string[] lines)
    {
        return lines
            .Select(l => l.Split(' ').Select(int.Parse).ToList())
            .Count(IsSafe);

        static bool IsSafe(List<int> numbers)
        {
            // take pairs (a,b), (b, c), (c, d) and calculate their distances
            var trends = numbers.Zip(numbers.Skip(1), (a, b) => b - a).ToArray();
            var allSameSign = trends.GroupBy(Math.Sign).Count() == 1;
            var allInRange = trends.Select(Math.Abs).All(t => t is not 0 and <= 3);
            return allSameSign && allInRange;
        }
    }

    public static int PartTwo(string[] lines)
    {
        return lines
            .Select(l => l.Split(' ').Select(int.Parse).ToList())
            .Count(x => IsSafe(x));

        static bool IsSafe(List<int> numbers, bool secondPass = false)
        {
            // take pairs (a,b), (b, c), (c, d) and calculate their distances
            var trends = numbers.Zip(numbers.Skip(1), (a, b) => b - a).ToArray();
            var allSameSign = trends.GroupBy(Math.Sign).Count() == 1;
            var allInRange = trends.Select(Math.Abs).All(t => t is not 0 and <= 3);
            if (allSameSign && allInRange) return true;

            // if not safe, try checking if it's safe when removing one number at a time
            if (secondPass) return false;
            return numbers
                .Select((_, i) => new List<int>([..numbers[..i], ..numbers[(i + 1)..]]))
                .Any(newList => IsSafe(newList, secondPass: true));
        }
    }
}