using Operator = System.Func<long, long, long>;

var stats = new Stats();
var input = File.OpenRead("input.txt");

Equation[] equations = ParseInput(input).ToArray();
stats.Report("Init");

Operator[] operators1 = [Add, Multiply];
var part1 = equations.Where(e => e.IsValid(operators1)).Sum(e => e.target);
stats.Report(1, part1);

Operator[] operators2 = [Add, Multiply, Concatenate];
var part2 = equations.AsParallel().Where(e => e.IsValid(operators2)).Sum(e => e.target);
stats.Report(2, part2);

IEnumerable<Equation> ParseInput(Stream input)
{
    var sr = new StreamReader(input);
    while (sr.ReadLine() is string line)
    {
        var span = line.AsSpan();
        var separator = span.IndexOf(':');
        var result = long.Parse(span[..separator]);
        var numbers = span[(separator + 2)..];
        var list = new List<long>(numbers.Count(' ') + 1);
        foreach (var range in numbers.Split(" "))
        {
            list.Add(long.Parse(numbers[range]));
        }

        yield return new Equation(result, list);
    }
}

long Add(long left, long right) => left + right;
long Multiply(long left, long right) => left * right;
long Concatenate(long left, long right)
{
    long factor = 1;
    while (factor <= right)
    {
        factor *= 10;
    }

    return left * factor + right;
}

readonly record struct Equation(long target, List<long> numbers)
{
    public bool IsValid(Operator[] operators)
    {
        int n = operators.Length;
        int combinations = (int)Pow(n, numbers.Count - 1);
        for (int i = 0; i < combinations; i++)
        {
            var result = numbers[0];
            var mask = i;
            for (int j = 1; j < numbers.Count; j++)
            {
                int index = mask % n;
                mask /= n;
                result = operators[index](result, numbers[j]);
                if (result > target)
                    break;
            }

            if (result == target)
            {
                return true;
            }
        }

        return false;
    }
}