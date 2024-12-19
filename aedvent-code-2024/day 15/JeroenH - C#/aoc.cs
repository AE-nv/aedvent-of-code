using System.Diagnostics.CodeAnalysis;
using System.Collections;
using FastConsole;
FConsole.Initialize("Advent of Code - 2024 day 15");

var stats = new Stats();
var input = File.ReadAllLines("input.txt");
var lines = input.TakeWhile(l => !string.IsNullOrEmpty(l)).ToArray();
var moves = (
    from line in input.SkipWhile(l => !string.IsNullOrEmpty(l)).Skip(1)
    from c in line
    select (Direction)c).ToImmutableList();
stats.Report("Init");

var part1 = Part1();
stats.Report(1, part1);

var part2 = Part2();
stats.Report(2, part2);

int Part1()
{
    var grid = new Grid(lines);
    var robot = grid.Find('@');
    foreach (var move in moves)
    {
        (robot, grid) = Move(robot, grid, move);
    }

    return (
        from kv in grid
        where kv.Value == 'O'
        select kv.Key.x + kv.Key.y * 100).Sum();
}

(Coordinate, Grid) Move(Coordinate robot, Grid grid, Direction move)
{
    
    var next = robot + move;
    return grid[next] switch
    {
        '.' => (next, grid.Swap(robot, next)),
        '#' => (robot, grid),
        'O' => grid.Items(next, move).First(x => x.value is '#' or '.') switch
        {
            (_, '#') => (robot, grid),
            (Coordinate c, '.') => (next, grid.With(g =>
            {
                g[robot] = '.';
                g[next] = '@';
                g[c] = 'O';
            })),
            _ => throw new InvalidOperationException()
        },
        _ => (robot, grid)
    };
}

int Part2()
{
    var grid = new Grid(Expanded().ToArray());
    var robot = grid.Find('@');
    int n = 0;

    FConsole.Clear();
    Draw(grid, robot);

    foreach (var move in moves)
    {
        n++;
        (robot, grid) = Move2(robot, grid, move);
        Draw(grid, robot);
    }

    return (
        from kv in grid
        where kv.Value == '['
        select kv.Key.x + kv.Key.y * 100).Sum();
}

void Draw(Grid grid, Coordinate robot)
{
    if (!args.Contains("draw")) return;
    FConsole.SetChars(grid.ToString());
    FConsole.SetChar(robot.x, robot.y, '▓', ConsoleColor.Gray, ConsoleColor.Black);
    FConsole.DrawBuffer();
}

IEnumerable<string> Expanded()
{
    var sb = new StringBuilder();
    foreach (var line in lines)
    {
        foreach (var c in line)
        {
            sb.Append(c switch
            {
                '@' => "@.",
                'O' => "[]",
                '#' => "##",
                _ => ".."
            });
        }

        yield return sb.ToString();
        sb.Clear();
    }
}

(Coordinate, Grid) Move2(Coordinate robot, Grid grid, Direction move)
{
    var next = robot + move;
    return (grid[next], move) switch
    {
        ('.', _) => (next, grid.Swap(robot, next)),
        ('#', _) => (robot, grid),
        ('[' or ']', Direction.E or Direction.W) => grid.Items(next, move).TakeWhile(x => x.value is not ('#' or '.')).ToArray() switch
        {
            var items => grid[items[^1].c + move] switch
            {
                not '.' => (robot, grid),
                _ => (next, grid.With(b =>
                {
                    // shift all boxes
                    foreach (var item in items.Reverse())
                    {
                        (b[item.c], b[item.c + move]) = ('.', item.value);
                    }

                    (b[robot], b[next]) = ('.', '@');
                }))
            }
        },
        ('[' or ']', Direction.N or Direction.S) => GetBoxesToMoveVertical([], grid, next, move, out var blocked) switch
        {
            var boxes when !blocked => MoveVertical(grid, robot, move, boxes),
            _ => (robot, grid)
        }
    };
}

(Coordinate, Grid) MoveVertical(Grid grid, Coordinate robot, Direction move, HashSet<(Coordinate left, Coordinate right)> boxes)
{
    var next = robot + move;
    return (next, grid.With(b =>
    {
        foreach (var (left, right) in move switch
        {
            Direction.S => boxes.OrderByDescending(x => x.left.y),
            Direction.N => boxes.OrderBy(x => x.left.y),
        })
        {
            b[left] = b[right] = '.';
            (b[left + move], b[right + move]) = ('[', ']');
        }

        (b[robot], b[next]) = ('.', '@');
    }));
}

HashSet<(Coordinate left, Coordinate right)> GetBoxesToMoveVertical(HashSet<(Coordinate left, Coordinate right)> boxes, Grid grid, Coordinate current, Direction move, out bool blocked)
{
    var (left, right) = grid[current] switch
    {
        '[' => (current, current + Direction.E),
        ']' => (current + Direction.W, current),
        _ => throw new InvalidOperationException()
    };
    boxes.Add((left, right));
    if (grid[left + move] == '.' && grid[right + move] == '.')
    {
        blocked = false;
        return boxes;
    }

    if (grid[left + move] is '[' or ']')
    {
        GetBoxesToMoveVertical(boxes, grid, left + move, move, out blocked);
        if (blocked)
            return boxes;
    }

    if (grid[right + move] is '[' or ']')
    {
        GetBoxesToMoveVertical(boxes, grid, right + move, move, out blocked);
        if (blocked)
            return boxes;
    }

    if (grid[left + move] == '#' || grid[right + move] == '#')
    {
        blocked = true;
        boxes.Clear();
        return boxes;
    }

    blocked = false;
    return boxes;
}

readonly record struct Coordinate(int x, int y)
{
    public static Coordinate Origin = new(0, 0);
    public override string ToString() => $"({x},{y})";
    public Coordinate N => new(x, y - 1);
    public Coordinate E => new(x + 1, y);
    public Coordinate S => new(x, y + 1);
    public Coordinate W => new(x - 1, y);

    public static Coordinate operator +(Coordinate left, (int dx, int dy) p) => new(left.x + p.dx, left.y + p.dy);
    public static Coordinate operator +(Coordinate c, Direction d) => d switch
    {
        Direction.N => c.N,
        Direction.E => c.E,
        Direction.S => c.S,
        Direction.W => c.W,
    };
    public static Coordinate operator -(Coordinate c, Direction d) => d switch
    {
        Direction.N => c.S,
        Direction.E => c.W,
        Direction.S => c.N,
        Direction.W => c.E,
    };
}

class Grid : IReadOnlyDictionary<Coordinate, char>
{
    readonly ImmutableDictionary<Coordinate, char> items;
    readonly Coordinate origin = new(0, 0);
    readonly Coordinate endmarker;
    readonly char empty;
    public int Height => endmarker.y;
    public int Width => endmarker.x;

    public IEnumerable<Coordinate> Keys
    {
        get
        {
            for (int y = origin.y; y < Height; y++)
                for (int x = origin.x; x < Width; x++)
                    yield return new Coordinate(x, y);
        }
    }

    public IEnumerable<char> Values => Keys.Select(k => this[k]);
    public int Count => Width * Height;

    public Grid(string[] input, char empty = '.') : this(ToDictionary(input, empty), empty, new(input[0].Length, input.Length))
    {
    }

    static ImmutableDictionary<Coordinate, char> ToDictionary(string[] input, char empty) => (
        from y in Range(0, input.Length)
        from x in Range(0, input[y].Length)
        where input[y][x] != empty
        select (x, y, c: input[y][x])).ToImmutableDictionary(t => new Coordinate(t.x, t.y), t => t.c);
    internal Grid(ImmutableDictionary<Coordinate, char> items, char empty, Coordinate endmarker)
    {
        this.items = items;
        this.empty = empty;
        this.endmarker = endmarker;
    }

    public Grid With(Action<ImmutableDictionary<Coordinate, char>.Builder> action)
    {
        var builder = items.ToBuilder();
        action(builder);
        return new Grid(builder.ToImmutable(), empty, endmarker);
    }

    public Grid Swap(Coordinate left, Coordinate right)
    {
        var l = this[left];
        var r = this[right];
        return this.With(b =>
        {
            b[left] = r;
            b[right] = l;
        });
    }

    public Coordinate Find(char c) => items.Where(i => i.Value == c).First().Key;
    public char this[Coordinate p] => items.TryGetValue(p, out var c) ? c : empty;
    public char this[(int x, int y) p] => this[new Coordinate(p.x, p.y)];
    public char this[int x, int y] => this[new Coordinate(x, y)];
    public IEnumerable<(Coordinate c, char value)> Items(Coordinate start, Direction d)
    {
        var delta = d switch
        {
            Direction.N => (0, -1),
            Direction.E => (1, 0),
            Direction.S => (0, 1),
            Direction.W => (-1, 0)
        };
        var p = start;
        while (ContainsKey(p))
        {
            yield return (p, this[p]);
            p += delta;
        }
    }

    bool IsValid(Coordinate p) => p.x >= 0 && p.y >= 0 && p.x < Width && p.y < Height;
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int y = origin.y; y < Height; y++)
        {
            for (int x = origin.x; x < Width; x++)
                sb.Append(this[x, y]);
            sb.AppendLine();
        }

        return sb.ToString();
    }

    public bool ContainsKey(Coordinate key) => IsValid(key);
    public bool TryGetValue(Coordinate key, [MaybeNullWhen(false)] out char value)
    {
        if (IsValid(key))
        {
            value = this[key];
            return true;
        }

        value = empty;
        return true;
    }

    public IEnumerator<KeyValuePair<Coordinate, char>> GetEnumerator() => Keys.Select(k => new KeyValuePair<Coordinate, char>(k, this[k])).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

enum Direction
{
    N = '^',
    E = '>',
    S = 'v',
    W = '<'
}