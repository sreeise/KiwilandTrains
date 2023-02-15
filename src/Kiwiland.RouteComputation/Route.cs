namespace Kiwiland.RouteComputation;

public enum Route
{
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
}

public static class RouteExtensionMethods
{
    public static Route ToRoute(this char c)
    {
        return c switch
        {
            'A' => Route.A,
            'B' => Route.B,
            'C' => Route.C,
            'D' => Route.D,
            'E' => Route.E,
            _ => throw new ArgumentNullException(nameof(c))
        };
    }
}