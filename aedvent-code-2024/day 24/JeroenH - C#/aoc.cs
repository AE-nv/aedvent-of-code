var stats = new Stats();

var input = File.ReadAllLines("input.txt");

var wires = input
    .TakeWhile(l => !string.IsNullOrEmpty(l))
    .Select(line => line.Split(": ") switch
        {
            [string label, string n] v => (label, value: int.Parse(n)),
            _ => default
        })
    .ToDictionary(x => x.label, x => x.value);

var gates = input.SkipWhile(l => !string.IsNullOrEmpty(l)).Skip(1).Select(Gate.Parse).ToList();
stats.Report("Init");

var part1 = Part1();
stats.Report(1, part1);

var part2 = Part2();
stats.Report(2, part2);

ulong Part1()
{
    var queue = new Queue<Gate>(gates);
    while (queue.Count > 0)
    {
        var gate = queue.Dequeue();
        if (wires.ContainsKey(gate.left) && wires.ContainsKey(gate.right))
        {
            wires[gate.output] = gate.Process(wires);
        }
        else
        {
            queue.Enqueue(gate);
        }
    }

    return wires.Keys.Where(k => k is ['z', ..]).OrderDescending().Aggregate(0UL, (n, k) => (n << 1) | (uint)wires[k]);
}

string Part2()
{
    var maxZ = wires.Keys.Where(x => x[0] == 'z').MaxBy(x => int.Parse(x[1..]));
    // identifies the wrong gates
    // inspired by https://www.reddit.com/r/adventofcode/comments/1hl698z/comment/m3kt1je/
    var q =
        from g in gates
        where g switch
        {
            (_, not '^', _, ['z', ..] output) => output != maxZ,
            ([not ('x' or 'y' or 'z'), ..], '^', [not ('x' or 'y' or 'z'), ..], [not ('x' or 'y' or 'z'), ..]) => true,
            (not "x00", '&', not "x00", _) => (
                from s in gates
                where s.IsConnectedTo(g) && s.@operator != '|'
                select g).Any(),
            (_, '^', _, _) => (
                from s in gates
                where s.IsConnectedTo(g) && s.@operator == '|'
                select g).Any(),
            _ => false
        }
        select g.output;
    var wrong = q.ToHashSet();
    return string.Join(",", wrong.OrderBy(x => x));
}

record struct Gate(string left, char @operator, string right, string output)
{
    public static Gate Parse(string line) => line.Split(' ') switch
    {
        [string left, string o, string right, _, string output] => new Gate(left, o switch
        {
            "AND" => '&',
            "OR" => '|',
            "XOR" => '^',
            _ => '\0'
        }, right, output),
        _ => default
    };
    public readonly int Process(Dictionary<string, int> wires) => @operator switch
    {
        '&' => wires[left] & wires[right],
        '|' => wires[left] | wires[right],
        '^' => wires[left] ^ wires[right],
        _ => throw new Exception()
    };
    public readonly bool IsConnectedTo(Gate other) => left == other.output || right == other.output;
}