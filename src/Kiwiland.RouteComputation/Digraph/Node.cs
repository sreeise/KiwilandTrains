using System.Text;
using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.RouteComputation.Digraph;

public class Node
{
    public int Vertex { get; }

    public readonly List<IEdge> Edges = new();

    public Node() { }

    public Node(int vertex)
    {
        Vertex = vertex;
        Edges = new List<IEdge>();
    }

    public Node(int vertex, List<IEdge> edges)
    {
        Vertex = vertex;
        Edges = edges;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append($"Vertex: {Vertex}");
        sb.Append(" [ ");
        foreach (var edge in Edges)
        {
            sb.Append($"{edge.Vertex} ");
        }

        sb.Append(" ]");
        return sb.ToString();
    }
}