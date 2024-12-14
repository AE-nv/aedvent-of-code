var stats = new Stats();

var input = File.ReadAllLines("input.txt");
var (width, height) = (101, 103);
var robots = (from t in input.Index() select Robot.Parse(t.Index, t.Item)).ToImmutableArray();
stats.Report("Init");

var part1 = (
    from r in robots
    let m = r.Move(100, width, height)
    group m by m.p.GetQuadrant(width, height) into g
    where g.Key != Quadrant.None
    select g.Count()).Aggregate(1, (i, c) => i * c);
stats.Report(1, part1);

var part2 = Part2();
stats.Report(2, part2);

var set = robots.Select(r => r.Move(part2, width, height)).Select(r => r.p).ToHashSet();
var bounds = GetBounds(set, 30);
DrawImage(bounds, set);

int Part2()
{
    Span<Robot> span = robots.ToArray();
    var set = new HashSet<Coordinate>();
    int n = 0;
    (int top, int left, int bottom, int right) bounds;
    do
    {
        n++;
        set.Clear();
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = span[i].Move(1, width, height);
            set.Add(span[i].p);
        }
    }
    while (!IsChristmasTree(set, out bounds));

    return n;
}

bool IsChristmasTree(HashSet<Coordinate> set, out (int top, int left, int bottom, int right) bounds)
{
    bounds = (-1, -1, -1, -1);
    // optimization (cf. subreddit)
    if (set.Count != robots.Length)
        return false;
    bounds = GetBounds(set, 30); // 30 is arbitrary, but turns out to be ok
    return bounds is ( > 0, > 0, > 0, > 0);
}

(int top, int left, int bottom, int right) GetBounds(HashSet<Coordinate> set, int size)
{
    var (top, left, bottom, right) = (-1, -1, -1, -1);
    for (int y = 0; y < height && bottom < 0; y++)
    {
        for (int x = 0; x < width - size; x++)
        {
            if (Range(x, size).All(i => set.Contains(new Coordinate(i, y))))
            {
                (top, bottom) = (top, bottom) switch
                {
                    ( < 0, _) => (y, bottom),
                    (_, < 0) => (top, y),
                };
                break;
            }
        }
    }

    for (int x = 0; x < width && right < 0; x++)
    {
        for (int y = 0; y < height - size; y++)
        {
            if (Range(y, size).All(i => set.Contains(new Coordinate(x, i))))
            {
                (left, right) = (left, right) switch
                {
                    ( < 0, _) => (x, right),
                    (_, < 0) => (left, x),
                };
                break;
            }
        }
    }

    return (top, left, bottom, right);
}

void DrawImage((int top, int left, int bottom, int right) bounds, HashSet<Coordinate> set)
{
    var (top, left, bottom, right) = bounds;
    var r = new Random();
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var c = new Coordinate(x, y);
            if (set.Contains(c))
            {
                if (x < left || x > right || y < top || y > bottom)
                {
                    Span<ConsoleColor> colors = [ConsoleColor.Gray, ConsoleColor.Yellow, ConsoleColor.DarkGray, ConsoleColor.White, ConsoleColor.DarkYellow];
                    Write('*', colors[r.Next(0, colors.Length - 1)]);
                }
                else if ((x, y) == (left, top) || ((x, y) == (left, bottom)) || ((x, y) == (right, top) || (x, y) == (right, bottom)))
                {
                    Write('+', ConsoleColor.DarkGray);
                }
                else if (x == left || x == right)
                {
                    Write('|', ConsoleColor.DarkGray);
                }
                else if (y == top || y == bottom)
                {
                    Write('-', ConsoleColor.DarkGray);
                }
                else
                {
                    Write('#', ConsoleColor.Green);
                }
            }
            else
            {
                Write(' ', Console.ForegroundColor);
            }
        }

        Console.WriteLine();
    }

    void Write(char c, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(c);
        Console.ResetColor();
    }
}


record struct Delta(int x, int y);
record struct Coordinate(int x, int y)
{
    public static Coordinate operator +(Coordinate c, Delta d) => new(c.x + d.x, c.y + d.y);
    internal Quadrant GetQuadrant(int width, int height) => (x - width / 2, y - height / 2) switch
    {
        ( > 0, < 0) => Quadrant.NE,
        ( > 0, > 0) => Quadrant.SE,
        ( < 0, > 0) => Quadrant.SW,
        ( < 0, < 0) => Quadrant.NW,
        (0, _) or (_, 0) => Quadrant.None
    };
}

record struct Velocity(int dx, int dy)
{
    public static Delta operator *(Velocity v, int t) => new(v.dx * t, v.dy * t);
}

record struct Robot(int id, Coordinate p, Velocity v)
{
    public static Robot Parse(int id, string s)
    {
        var m = Regexes.Robot().Match(s);
        return new Robot(id, new(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value)), new(int.Parse(m.Groups["dx"].Value), int.Parse(m.Groups["dy"].Value)));
    }

    public Robot Move(int t, int w, int h)
    {
        var pos = p + (v * t);
        pos = pos with
        {
            x = (pos.x + t * w) % w,
            y = (pos.y + t * h) % h
        };
        return this with
        {
            p = pos
        };
    }
}

static partial class Regexes
{
    [GeneratedRegex(@"p=(?<x>[\d]+),(?<y>[\d]+) v=(?<dx>[-+\d]+),(?<dy>[-+\d]+)")]
    public static partial Regex Robot();
}

public enum Quadrant
{
    NW,
    NE,
    SE,
    SW,
    None
}