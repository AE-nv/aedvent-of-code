Console.WriteLine(DayFour.PartOne(File.ReadAllLines("inputs/4.txt")));
Console.WriteLine(DayFour.PartTwo(File.ReadAllLines("inputs/4.txt")));

public static class DayFour
{
    public static int PartOne(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();

        return cells
            .Where(c => c.Value == 'X')
            .SelectMany(x => x.GetNeighbors(cells).WithValue('M'))
            .SelectMany(m => m.Cell.GetNeighbors(cells).WithValue('A').WithDirection(m.Direction))
            .SelectMany(a => a.Cell.GetNeighbors(cells).WithValue('S').WithDirection(a.Direction))
            .Count();
    }

    public static int PartTwo(string[] lines)
    {
        var cells = Cells.FromLines(lines).ToArray();

        return cells
            .Where(c => c.Value == 'A')
            .Count(a =>
            {
                var m = a.GetNeighbors(cells).WithValue('M').OnDiagonal();

                // Check if two S neighbors of A are on the opposite diagonal of an M
                return a.GetNeighbors(cells).WithValue('S').Count(x => m.Any(mm => mm.Direction.Flip() == x.Direction)) == 2;
            });
    }
}

public record Cell(char Value, int X, int Y)
{
    public bool Neighbors(Cell other) => this != other && Math.Abs(X - other.X) <= 1 && Math.Abs(Y - other.Y) <= 1;

    public IEnumerable<Neighbor> GetNeighbors(IEnumerable<Cell> cells) =>
        cells.Where(Neighbors).Select(c => new Neighbor(c, new Vector(c.X - X, c.Y - Y)));
}

public static class Cells
{
    public static IEnumerable<Cell> FromLines(string[] lines) =>
        lines.SelectMany((line, y) => line.Select((c, x) => new Cell(c, x, y)));
}

public record Vector(int X, int Y)
{
    public bool IsDiagonal() => this is not (0, 0) && Math.Abs(X) == Math.Abs(Y);
    public Vector Flip() => new(-X, -Y);
}

public record Neighbor(Cell Cell, Vector Direction);

public static class Neighbors
{
    public static IEnumerable<Neighbor> WithValue(this IEnumerable<Neighbor> neighbors, char value) =>
        neighbors.Where(n => n.Cell.Value == value);

    public static IEnumerable<Neighbor> WithDirection(this IEnumerable<Neighbor> neighbors, Vector direction) =>
        neighbors.Where(n => n.Direction == direction);

    public static IEnumerable<Neighbor> OnDiagonal(this IEnumerable<Neighbor> neighbors) =>
        neighbors.Where(n => n.Direction.IsDiagonal());
}