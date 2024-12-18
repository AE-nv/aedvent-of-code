var stats = new Stats();
var input = File.ReadAllLines("input.txt");
stats.Report("Init");
var part1 = Part1();
stats.Report(1, part1);
var part2 = Part2();
stats.Report(2, part2);
(int a, int b, int c, long[] program) ReadInput(string[] input)
{
    var (a, b, c) = (Parse(input[0]), Parse(input[1]), Parse(input[2]));
    var program = input[4][9..].Split(",").Select(long.Parse).ToArray();
    var intcode = new IntCode(program);
    return (a, b, c, program);
    static int Parse(string register) => int.Parse(Regexes.Register().Match(register).Groups[1].Value);
}

string Part1()
{
    var (a, b, c, program) = ReadInput(input);
    var intcode = new IntCode(program);
    return string.Join(",", intcode.Run(a, b, c));
}

long Part2()
{
    var (_, _, _, program) = ReadInput(input);
    var intcode = new IntCode(program);
    long current = 0;
    for (int digit = program.Length - 1; digit >= 0; digit -= 1)
    {
        for (int i = 0; i < int.MaxValue; i++)
        {
            var candidate = current + (1L << (digit * 3)) * i;
            var output = intcode.Run(candidate, 0, 0);
            if (output.Skip(digit).SequenceEqual(program.Skip(digit)))
            {
                current = candidate;
                break;
            }
        }
    }

    return current;
}

class IntCode(long[] program, TextWriter? writer = null)
{
    public IEnumerable<long> Run(long a, long b, long c)
    {
        var i = 0L;
        while (i < program.Length - 1)
        {
            var opcode = program[i];
            var operand = program[i + 1];
            var combo = operand switch
            {
                >= 0 and <= 3 => operand,
                4 => a,
                5 => b,
                6 => c
            };
            (i, a, b, c, long output) = opcode switch
            {
                0 => (i + 2, a >>= (int)combo, b, c, -1),
                1 => (i + 2, a, b ^ operand, c, -1),
                2 => (i + 2, a, (int)(combo % 8 + 8) % 8, c, -1),
                3 => (a != 0 ? operand : i + 2, a, b, c, -1),
                4 => (i + 2, a, b ^ c, c, -1),
                5 => (i + 2, a, b, c, (combo % 8 + 8) % 8),
                6 => (i + 2, a, a >>= (int)combo, c, -1),
                7 => (i + 2, a, b, a >>= (int)combo, -1),
            };
            writer?.WriteLine((i, a, b, c, output));
            if (opcode == 5)
                yield return output;
        }
    }
}

static partial class Regexes
{
    [GeneratedRegex(@"Register [ABC]: (\d+)")]
    public static partial Regex Register();
}