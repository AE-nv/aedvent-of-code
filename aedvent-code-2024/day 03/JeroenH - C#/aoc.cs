var input = File.ReadAllText("input.txt");
List<Instruction> tokens = Parse(input);
var sw = Stopwatch.StartNew();
var part1 = tokens.OfType<@mul>().Select(m => m.result).Sum();
var part2 = GetActiveTokens(tokens).Select(m => m.result).Sum();
Output.WriteResult(part1, part2, sw.Elapsed);
List<Instruction> Parse(ReadOnlySpan<char> span)
{
    List<Instruction> tokens = [];
    foreach (var m in Regexes.AoC202403Regex().EnumerateMatches(span))
    {
        var slice = span.Slice(m.Index, m.Length);
        tokens.Add(slice switch
        {
            "do()" => new @do(),
            "don't()" => new @dont(),
            _ => @mul.Parse(slice)
        });
    }

    return tokens;
}

IEnumerable<@mul> GetActiveTokens(IList<Instruction> tokens)
{
    bool active = true;
    foreach (var token in tokens)
    {
        active = token switch
        {
            @do _ => true,
            @dont _ => false,
            _ => active
        };
        if (active && token is mul m)
        {
            yield return m;
        }
    }
}

interface Instruction
{
}

readonly struct @mul(int l, int r) : Instruction
{
    public int result => l * r;

    public static Instruction Parse(ReadOnlySpan<char> slice)
    {
        var mul = slice[4..^1];
        var separator = mul.IndexOf(',');
        var l = int.Parse(mul[0..separator]);
        var r = int.Parse(mul[(separator + 1)..]);
        return new mul(l, r);
    }
}

readonly struct @do : Instruction;
readonly struct @dont : Instruction;
static partial class Regexes
{
    [GeneratedRegex(@"(mul\((?<left>\d+),(?<right>\d+)\)|do\(\)|don't\(\))")]
    public static partial Regex AoC202403Regex();
}