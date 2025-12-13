#: package Microsoft.Z3@4.12.2
using Microsoft.Z3;
using System.Numerics;
using static System.Linq.Enumerable;
using System.Diagnostics;

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
Machine[] machines = input.Select(s => Machine.Parse(s)).ToArray();
Report(0, "", sw, ref bytes);
var part1 = machines.Sum(m => m.Start());
Report(1, part1, sw, ref bytes);
var part2 = machines.Sum(m => m.SolveJoltage());
Report(2, part2, sw, ref bytes);
void Report<T>(int part, T value, Stopwatch sw, ref long bytes)
{
    var label = part switch
    {
        1 => $"Part 1: [{value}]",
        2 => $"Part 2: [{value}]",
        _ => "Init"
    };
    var time = sw.Elapsed switch
    {
        { TotalMicroseconds: < 1 } => $"{sw.Elapsed.TotalNanoseconds:N0} ns",
        { TotalMilliseconds: < 1 } => $"{sw.Elapsed.TotalMicroseconds:N0} µs",
        { TotalSeconds: < 1 } => $"{sw.Elapsed.TotalMilliseconds:N0} ms",
        _ => $"{sw.Elapsed.TotalSeconds:N2} s"
    };
    var newbytes = GC.GetTotalAllocatedBytes(false);
    var memory = (newbytes - bytes) switch
    {
        < 1024 => $"{newbytes - bytes} B",
        < 1024 * 1024 => $"{(newbytes - bytes) / 1024:N0} KB",
        _ => $"{(newbytes - bytes) / (1024 * 1024):N0} MB"
    };
    Console.WriteLine($"{label} ({time} - {memory})");
    bytes = newbytes;
}

// [.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
// represent target state as an integer, with bits for each position
// represent buttons as arrays of integers, each with bits for positions they toggle
record Machine(int nofIndicators, int targetState, int[] buttons, int[] joltage)
{
    static Range FindRange(ReadOnlySpan<char> input, char start, char end)
    {
        int startIndex = input.IndexOf(start);
        int endIndex = input.LastIndexOf(end) + 1;
        return startIndex..endIndex;
    }

    public int Start()
    {
        var combinations = 1 << buttons.Length;
        var minimum = int.MaxValue;
        for (int mask = 0; mask < combinations; mask++)
        {
            int presses = BitOperations.PopCount((uint)mask);
            if (presses >= minimum)
                continue;
            var state = 0;
            for (int b = 0; b < buttons.Length; b++)
            {
                if ((mask & (1 << b)) == 0)
                    continue;
                state ^= buttons[b];
            }

            if (state == targetState)
            {
                minimum = presses;
            }
        }

        return minimum == int.MaxValue ? -1 : minimum;
    }

    /*
     * the "joltage" requirement can be seen as a system of linear equations:
     *   - each button contributes +1 to every indicator it toggles every time it is pressed
     *   - we can press a button any number of times
     *   - for indicator i we want sum(presses[b]) == target[i] over all buttons b that light up i
     *   - the objective is to minimise the total number of button presses subject to those equalities
     */
    public int SolveJoltage()
    {
        using var ctx = new Context();
        var opt = ctx.MkOptimize();
        var pressVars = Range(0, buttons.Length).Select(i => ctx.MkIntConst($"p{i}")).ToArray();
        // Define each button's number of presses as a positive integer
        var zero = ctx.MkInt(0);
        foreach (var v in pressVars)
        {
            opt.Add(ctx.MkGe(v, zero));
        }

        for (int indicator = 0; indicator < joltage.Length; indicator++)
        {
            // build the LHS of the equality for this indicator:
            // 
            //                    nofb
            //                    ----
            //           t_i  =   \      press[b] * affects(b, i)
            //                    /
            //                    ----
            //                    b=1
            // each contributing button press adds +1 to the indicator's joltage
            List<IntExpr> terms = [];
            for (int b = 0; b < buttons.Length; b++)
            {
                if ((buttons[b] & (1 << indicator)) == 0)
                    continue; // only consider buttons that toggle this indicator
                terms.Add(pressVars[b]);
            }

            var sum = Sum(ctx, terms);
            // tie the linear combination to the required joltage target for this indicator
            opt.Add(ctx.MkEq(sum, ctx.MkInt(joltage[indicator])));
        }

        var totalPresses = Sum(ctx, pressVars);
        // objective is to minimize the total number of button presses
        opt.MkMinimize(totalPresses);
        return opt.Check() == Status.SATISFIABLE ? Evaluate(opt.Model, totalPresses) : -1;
    }

    static IntExpr Sum(Context ctx, IReadOnlyList<IntExpr> terms) => terms.Count switch
    {
        0 => ctx.MkInt(0),
        1 => terms[0],
        _ => (IntExpr)ctx.MkAdd([.. terms.Cast<ArithExpr>()])
    };
    static int Evaluate(Model model, IntExpr expr) => ((IntNum)model.Evaluate(expr)).Int;
    public static Machine Parse(ReadOnlySpan<char> input)
    {
        T[] parseBetween<T>(ReadOnlySpan<char> input, char start, char end, char separator, Func<ReadOnlySpan<char>, T> convert)
        {
            var startIndex = input.IndexOf(start);
            var endIndex = input.LastIndexOf(end) + 1;
            var span = input[startIndex..endIndex][1..^1];
            var count = span.Count(separator) + 1;
            var ranges = new Range[count];
            span.Split(ranges, separator);
            T[] result = new T[count];
            for (int i = 0; i < count; i++)
            {
                var part = span[ranges[i]];
                result[i] = convert(part);
            }

            return result;
        }

        var startStateEnd = input.IndexOf(']');
        var stateSpan = input[FindRange(input, '[', ']')][1..^1];
        var nofIndicators = stateSpan.Length;
        var targetState = 0;
        for (int i = 0; i < stateSpan.Length; i++)
        {
            if (stateSpan[i] == '#')
            {
                targetState |= (1 << i);
            }
        }

        var buttonsSpan = input[FindRange(input, '(', ')')];
        var numberOfButtons = buttonsSpan.Count(' ') + 1;
        int[] buttons = new int[numberOfButtons];
        Range[] buttonsRanges = new Range[numberOfButtons];
        buttonsSpan.Split(buttonsRanges, ' ');
        for (var i = 0; i < numberOfButtons; i++)
        {
            int button = 0;
            var digitsSeparatedByComma = buttonsSpan[buttonsRanges[i]][1..^1];
            for (int j = 0; j < digitsSeparatedByComma.Length; j += 2)
            {
                var digit = digitsSeparatedByComma[j];
                button |= (1 << (digit - '0'));
            }

            buttons[i] = button;
        }

        int[] joltage = parseBetween<int>(input, '{', '}', ',', span => int.Parse(span));
        return new Machine(nofIndicators, targetState, buttons, joltage);
    }
}