namespace Net.Code.Graph;

public static class Edge
{
    public static Edge<TVertex, TValue> Create<TVertex, TValue>(TVertex source, TVertex destination, TValue value) => new(source, destination, value);
    public static Edge<TVertex, TValue> Create<TVertex, TValue>((TVertex source, TVertex destination) edge, TValue value) => new(edge.source, edge.destination, value);
}

public record struct Edge<TVertex, TValue>(TVertex Source, TVertex Destination, TValue Value)
{
    public static implicit operator Edge<TVertex, TValue>((TVertex Source, TVertex Destination, TValue Value) tuple) => new(tuple.Source, tuple.Destination, tuple.Value);
    public override string ToString()
    {
        return $"{Source} -({Value})-> {Destination}";
    }
}

public static class Vertex
{
    public static Vertex<TVertex> Create<TVertex>(TVertex value) => new(value);
}

public readonly record struct Vertex<TVertex>(TVertex Value)
{
    public static implicit operator Vertex<TVertex>(TVertex value) => new(value);
}

public static class TupleExtensions
{
    public static (T, T) Reverse<T>(this (T a, T b) tuple) => (tuple.b, tuple.a);
    public static (T, T) Ordered<T>(this (T, T) tuple) where T : IComparable<T>
    => tuple.Item1.CompareTo(tuple.Item2) >= 0
        ? tuple
        : (tuple.Item2, tuple.Item1);
    public static (T, T, T) Ordered<T>(this (T a, T b, T c) tuple) where T : IComparable<T>
    {
        var (a, b, c) = tuple;
        return (a.CompareTo(b), b.CompareTo(c), a.CompareTo(c)) switch
        {
            ( < 0, < 0, < 0) => (a, b, c),
            ( < 0, < 0, >= 0) => (a, b, c),
            ( < 0, >= 0, < 0) => (a, c, b),
            ( < 0, >= 0, >= 0) => (c, a, b),
            ( >= 0, < 0, < 0) => (b, a, c),
            ( >= 0, < 0, >= 0) => (b, c, a),
            ( >= 0, >= 0, < 0) => (c, b, a),
            ( >= 0, >= 0, >= 0) => (c, b, a),
        };
    }
}
