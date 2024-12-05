var stats = new Stats();

var lines = File.ReadAllLines("input.txt");
stats.Report("Read input");

var rules = (
    from line in lines.TakeWhile(l => l != "") 
    let parts = line.Split('|') 
    select (left: int.Parse(parts[0]), right: int.Parse(parts[1]))
).ToArray();

var updates = (
    from line in lines.SkipWhile(l => l != "").Skip(1) 
    select line.Split(',').Select(int.Parse).ToArray()
).ToArray();

stats.Report("Parse");

var part1 = (
    from update in updates
    where !InvalidRules(update).Any()
    select update[update.Length / 2]
).Sum();

stats.Report(1, part1);

var comparer = new CustomComparer(rules);
var part2 = (
    from update in updates
    where InvalidRules(update).Any()
    select update.Order(comparer).Skip(update.Length / 2).First()
).Sum();

stats.Report(2, part2);

IEnumerable<(int left, int right)> InvalidRules(IList<int> update) =>
    from rule in rules
    where update.Contains(rule.left) && update.Contains(rule.right) && update.IndexOf(rule.left) > update.IndexOf(rule.right)
    select rule;

struct CustomComparer((int left, int right)[] rules) : IComparer<int>
{
    public readonly int Compare(int x, int y)
    {
        foreach (var (left, right) in rules)
        {
            if (left == x && right == y)
                return -1;
            if (left == y && right == x)
                return 1;
        }

        return x.CompareTo(y);
    }
}
