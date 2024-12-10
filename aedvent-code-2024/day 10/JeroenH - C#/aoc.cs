using Maze = System.Collections.Generic.IReadOnlyDictionary<Coordinate, int>;

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
Maze maze = (
    from il in input.Index()
    let y = il.Index
    let line = il.Item
    from ic in line.Index()
    let x = ic.Index
    let c = ic.Item
    where c != '.'
    select KeyValuePair.Create(new Coordinate(x, y), c - '0')).ToDictionary();
stats.Report("Init");
var part1 = (
    from c in maze.Keys
    where maze[c] == 0
    select GetScore1(maze, c)).Sum();
stats.Report(1, part1);
var part2 = (
    from c in maze.Keys
    where maze[c] == 0
    select GetScore2(maze, c)).Sum();
stats.Report(2, part2);
int GetScore1(Maze maze, Coordinate start)
{
    HashSet<Coordinate> peaks = [];
    Queue<Coordinate> queue = [];
    HashSet<Coordinate> visited = [start];
    queue.Enqueue(start);
    while (queue.Any())
    {
        var current = queue.Dequeue();
        var height = maze[current];
        if (height == 9)
        {
            peaks.Add(current);
            continue;
        }

        foreach (var n in current.Neighbours().Where(c => maze.TryGetValue(c, out var value) && value == height + 1))
        {
            if (!visited.Contains(n))
            {
                visited.Add(n);
                queue.Enqueue(n);
            }
        }
    }

    return peaks.Count;
}

int GetScore2(Maze maze, Coordinate start)
{
    var stack = new Stack<Coordinate>();
    stack.Push(start);
    var visited = new HashSet<Coordinate>();
    var count = 0;
    while (stack.Any())
    {
        var current = stack.Pop();
        var height = maze[current];
        if (height == 9)
        {
            count++;
            continue;
        }

        visited.Clear();
        foreach (var n in current.Neighbours().Where(c => maze.TryGetValue(c, out var value) && value == height + 1))
        {
            if (!visited.Contains(n))
            {
                visited.Add(n);
                stack.Push(n);
            }
        }
    }

    return count;
}

readonly record struct Coordinate(int x, int y)
{
    public override string ToString() => $"({x},{y})";
    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
    static readonly (int dx, int dy)[] deltas = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    public IEnumerable<Coordinate> Neighbours()
    {
        var c = this;
        return
            from d in deltas
            select c + d;
    }
}