using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.RouteComputation.Digraph;

public class Edge : IComparable<Edge>, IEdge
{
    public int Vertex { get; set; }
    public int Weight { get; set; }

    public Edge(int vertex, int weight)
    {
        Vertex = vertex;
        Weight = weight;
    }

    public int CompareTo(Edge? other)
    {
        if (other != null) return Weight - other.Weight;
        throw new ArgumentNullException(nameof(other));
    }

    public override string ToString()
    {
        return $"Vertex: {Vertex}, Weight: {Weight}";
    }
}