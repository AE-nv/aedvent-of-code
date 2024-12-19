var stats = new Stats();
var input = File.ReadAllLines("input.txt");
var patterns = input[0].Split(", ");
var designs = input[2..];
var cache = new long[designs.Max(d => d.Length)];
var trie = new Trie(patterns);
stats.Report("Init");
var part1 = Counts().Where(c => c > 0).Count();
stats.Report(1, part1);
var part2 = Counts().Sum();
stats.Report(2, part2);
IEnumerable<long> Counts()
{
    foreach (var design in designs)
    {
        Array.Clear(cache);
        var count = PatternMatch(design);
        yield return count;
    }
}

long PatternMatch(ReadOnlySpan<char> word)
{
    if (word.Length == 0)
        return 1;
    int index = word.Length - 1;
    if (cache[index] > 0)
        return cache[index];
    long result = 0;
    var node = trie.Root;
    for (int i = 0; i < word.Length && node[word[i]] is not null; i++)
    {
        node = node[word[i]];
        if (node.IsEndOfWord)
        {
            result += PatternMatch(word[(i + 1)..]);
        }
    }

    cache[index] = result;
    return result;
}

// https://en.wikipedia.org/wiki/Trie
class Trie
{
    private readonly Node root = new();
    public Node Root => root;

    public Trie(IEnumerable<string> patterns)
    {
        foreach (var pattern in patterns)
        {
            var node = root;
            foreach (var c in pattern)
            {
                node = node[c] ??= new();
            }

            node.IsEndOfWord = true;
        }
    }

    public class Node
    {
        public Node this[char c] { get => Children[c - 'a']; set => Children[c - 'a'] = value; }

        public Node[] Children { get; } = new Node[26];
        public bool IsEndOfWord { get; set; }
    }
}

public class Node
{
    public Node this[char c] { get => Children[c - 'a']; set => Children[c - 'a'] = value; }

    public Node[] Children { get; } = new Node[26];
    public bool IsEndOfWord { get; set; }
}