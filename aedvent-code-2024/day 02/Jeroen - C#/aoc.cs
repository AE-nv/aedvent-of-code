var stream = File.OpenRead("input.txt");
IReadOnlyList<int>[] input = ReadInput(stream).ToArray();
var sw = Stopwatch.StartNew();
var part1 = input.Where(IsSafe).Count();
var part2 = input.Where(IsSafe2).Count();
Output.WriteResult(part1, part2, sw.Elapsed);
IEnumerable<IReadOnlyList<int>> ReadInput(Stream stream)
{
    using var r = new StreamReader(stream);
    while (!r.EndOfStream)
    {
        var span = r.ReadLine().AsSpan();
        List<int> ints = [];
        foreach (var range in span.Split(' '))
        {
            ints.Add(int.Parse(span[range]));
        }

        yield return ints;
    }
}

bool IsSafe(IReadOnlyList<int> list)
{
    var ascending = list[1] > list[0];
    for (int i = 0; i < list.Count - 1; i++)
    {
        var delta = list[i + 1] - list[i];
        if (delta is 0)
            return false;
        if (delta > 0 != ascending)
            return false;
        if (Abs(delta) is < 1 or > 3)
            return false;
    }

    return true;
}

bool IsSafe2(IReadOnlyList<int> list)
{
    var buffer = new List<int>(list.Count - 1);
    for (var exclude = 0; exclude < list.Count; exclude++)
    {
        buffer.AddRange(Except(list, exclude));
        if (IsSafe(buffer))
            return true;
        buffer.Clear();
    }

    return false;
}

IEnumerable<int> Except(IReadOnlyList<int> list, int item)
{
    for (int i = 0; i < list.Count; i++)
    {
        if (i == item)
            continue;
        yield return list[i];
    }
}
