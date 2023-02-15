namespace Kiwiland.RouteComputation.Generic;

public interface IEdge
{
    int Vertex { get; set; }

    int Weight { get; set; }
}