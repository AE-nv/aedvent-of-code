var (left, right) = ReadInput();

var sw = Stopwatch.StartNew();
var part1 = Part1();
var part2 = Part2();
Console.WriteLine((part1, part2, sw.Elapsed, GC.GetTotalAllocatedBytes()));

(List<int> left, List<int> right) ReadInput()
{
    var l = new List<int>(1000);
    var r = new List<int>(1000);

    var s = File.OpenRead("input.txt");
    var sr = new StreamReader(s);
    var buffer = new char[14];

    // file contains lines of 5 digits, 3 spaces, 5 digits and \n (= 14 characters)
    // the first 5 digits are the left number, the second 5 digits are the right number
    while (!sr.EndOfStream)
    {
        sr.ReadBlock(buffer);
        l.Add(int.Parse(new string(buffer, 0, 5)));
        r.Add(int.Parse(new string(buffer, 8, 5)));
    }
    return (l, r);
}

int Part1() => left.Order().Zip(right.Order(), (l, r) => Abs(r - l)).Sum();

long Part2()
{
        var counts = (
            from r in right
            group r by r into g 
            select (g.Key, Count: g.Count())
         ).ToDictionary(x => x.Key, x => x.Count);

        return (from l in left
                select counts.TryGetValue(l, out int value) ? l * value : 0).Sum();
}