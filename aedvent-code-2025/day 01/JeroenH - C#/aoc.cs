var stats = new Stats();
var input = File.ReadAllLines("input.txt");
int[] instructions = [.. input.Select(line => (line[0] == 'L' ? -1 : 1) * int.Parse(line[1..]))];
stats.Report("Init");
var part1 = instructions.Aggregate((value: 50, password: 0), (acc, i) =>
{
    var next = (acc.value + 100 + i) % 100;
    var increment = next == 0 ? 1 : 0;
    return acc with
    {
        value = next,
        password = acc.password + increment
    };
}).password;
stats.Report(1, part1);
var part2 = instructions.Aggregate((value: 50, password: 0), (acc, i) =>
{
    var next = (acc.value + 100 + i) % 100;
    var increment = i >= 0 ? (acc.value + i) / 100 : (-i - acc.value + (acc.value == 0 ? 0 : 100)) / 100;
    return acc with
    {
        value = next,
        password = acc.password + increment
    };
}).password;
stats.Report(2, part2);