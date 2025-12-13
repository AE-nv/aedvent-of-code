using System.Runtime.CompilerServices;
using static System.Math;
using System.Diagnostics;
using static System.Linq.Enumerable;

var (sw, bytes) = (Stopwatch.StartNew(), 0L);
var filename = args switch
{
    ["sample"] => "sample.txt",
    _ => "input.txt"
};
var input = File.ReadAllLines(filename);
Coordinate[] coordinates = [..
    from line in input
    select Coordinate.Parse(line.AsSpan())];
Report(0, "", sw, ref bytes);
var part1 = Part1();
Report(1, part1, sw, ref bytes);
var part2 = Part2();
Report(2, part2, sw, ref bytes);
long Part1()
{
    var rectangles = (
        from i in Range(0, coordinates.Length)
        from j in Range(i + 1, coordinates.Length - i - 1)
        let rectangle = Rectangle.FromCoordinates(coordinates[i], coordinates[j])
        orderby rectangle.Area descending
        select rectangle);
    return rectangles.First().Area;
}

long Part2()
{
    var polygon = new Polygon(coordinates);
    var rectangles = (
        from i in Range(0, coordinates.Length)
        from j in Range(i + 1, coordinates.Length - i - 1)
        let rectangle = Rectangle.FromCoordinates(coordinates[i], coordinates[j])
        orderby rectangle.Area descending
        select rectangle).ToArray();
    return (
        from rectangle in rectangles
        where polygon.IsValid(rectangle)
        select rectangle).First().Area;
}

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

record struct Coordinate(int x, int y)
{
    public static Coordinate Parse(ReadOnlySpan<char> s)
    {
        Range[] ranges = new Range[2];
        s.Split(ranges, ',');
        return new Coordinate(int.Parse(s[ranges[0]]), int.Parse(s[ranges[1]]));
    }

    public static Coordinate operator +(Coordinate c, (int dx, int dy) p) => new(c.x + p.dx, c.y + p.dy);
    public override string ToString()
    {
        return $"({x},{y})";
    }
}

record struct Rectangle(Coordinate TopLeft, Coordinate BottomRight)
{
    public long Height => BottomRight.y - TopLeft.y + 1;
    public long Width => BottomRight.x - TopLeft.x + 1;
    public long Area => Width * Height;

    public static Rectangle FromCoordinates(Coordinate a, Coordinate b)
    {
        var topLeft = new Coordinate(Min(a.x, b.x), Min(a.y, b.y));
        var bottomRight = new Coordinate(Max(a.x, b.x), Max(a.y, b.y));
        return new Rectangle(topLeft, bottomRight);
    }

    public Coordinate BottomLeft => TopLeft with
    {
        y = BottomRight.y
    };
    public Coordinate TopRight => BottomRight with
    {
        y = TopLeft.y
    };

    // check if this rectangle is crossed by the given edge
    public bool Intersects(Edge edge) => Edges.Any(edge.Crosses);
    public Edge[] Edges
    {
        get
        {
            return field ??= [new Edge(TopLeft, TopRight), new Edge(TopRight, BottomRight), new Edge(BottomRight, BottomLeft), new Edge(BottomLeft, TopLeft)];
        }
    }
}

record struct Edge(Coordinate Start, Coordinate End)
{
    public readonly bool IsVertical => Start.x == End.x;
    public readonly bool IsHorizontal => Start.y == End.y;
    // Manhattan distance between Start & End
    public readonly int Length => Abs(Start.x - End.x) + Abs(Start.y - End.y);

    public IEnumerable<Coordinate> Points
    {
        get
        {
            if (IsVertical)
            {
                for (int y = Min(Start.y, End.y); y <= Max(Start.y, End.y); y++)
                {
                    yield return new Coordinate(Start.x, y);
                }
            }
            else if (IsHorizontal)
            {
                for (int x = Min(Start.x, End.x); x <= Max(Start.x, End.x); x++)
                {
                    yield return new Coordinate(x, Start.y);
                }
            }
        }
    }

    public readonly bool Contains(Coordinate c) => (IsVertical && c.x == Start.x && Min(Start.y, End.y) <= c.y && c.y <= Max(Start.y, End.y)) || (IsHorizontal && c.y == Start.y && Min(Start.x, End.x) <= c.x && c.x <= Max(Start.x, End.x));
    public readonly bool Crosses(Edge other) => (IsVertical && other.IsHorizontal && Min(other.Start.x, other.End.x) < Start.x && Start.x < Max(other.Start.x, other.End.x) && Min(Start.y, End.y) < other.Start.y && other.Start.y < Max(Start.y, End.y)) || (IsHorizontal && other.IsVertical && Min(other.Start.y, other.End.y) < Start.y && Start.y < Max(other.Start.y, other.End.y) && Min(Start.x, End.x) < other.Start.x && other.Start.x < Max(Start.x, End.x));
    public override string ToString()
    {
        return $"{Start} -> {End}";
    }
}

record struct Polygon(Coordinate[] Corners)
{
    Rectangle? _field;
    public Rectangle BoundingBox
    {
        get
        {
            return _field ??= new(new(Corners.Min(c => c.x), Corners.Min(c => c.y)), new(Corners.Max(c => c.x), Corners.Max(c => c.y)));
        }
    }

    public HashSet<Edge> Edges
    {
        get
        {
            return field ??= [.. GetEdges().OrderByDescending(e => e.Length)];
        }
    }

    private IEnumerable<Edge> GetEdges()
    {
        for (int i = 0; i < Corners.Length; i++)
        {
            var start = Corners[i];
            var end = Corners[(i + 1) % Corners.Length];
            yield return new Edge(start, end);
        }
    }

    // a rectangle is valid if
    // * all corners are 'inside'
    // * all corners of a rectangle reduced in size by 1 are 'inside'
    // * no edges of the polygon cross into the rectangle
    public bool IsValid(Rectangle rectangle)
    {
        var p = this;
        if (!p.IsInside(rectangle.TopLeft))
            return false;
        if (!p.IsInside(rectangle.BottomRight))
            return false;
        if (!p.IsInside(rectangle.TopRight))
            return false;
        if (!p.IsInside(rectangle.BottomLeft))
            return false;
        if (rectangle.Height > 1 && rectangle.Width > 1)
        {
            if (!p.IsInside(rectangle.TopLeft + (1, 1)))
                return false;
            if (!p.IsInside(rectangle.TopRight + (-1, 1)))
                return false;
            if (!p.IsInside(rectangle.BottomRight + (-1, -1)))
                return false;
            if (!p.IsInside(rectangle.BottomLeft + (1, -1)))
                return false;
        }

        foreach (var edge in p.Edges)
        {
            if (rectangle.Intersects(edge))
                return false;
        }

        return true;
    }

    private bool IsInside(Coordinate p)
    {
        // check if on boundary
        foreach (var edge in Edges)
        {
            if (edge.Contains(p))
                return true;
        }

        // ray-casting to the right of p; 
        // count how many edges it intersects
        // if odd, point is inside; if even, point is outside
        int count = 0;
        // if it is a vertical edge & the point is to the left of it
        foreach (var e in Edges.Where(e => e.IsVertical && p.x < e.Start.x))
        {
            var point = new Coordinate(e.Start.x, p.y); // point on the line of the edge
            if (e.Contains(point))
            {
                count++;
            }
        }

        if (count % 2 != 1)
            return false;
        return true;
    }
}