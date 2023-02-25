using Newtonsoft.Json;

namespace Kiwiland.RouteComputation.core;

public record RouteInformation(List<string> DirectionsRoutes, int Distance, int Stops)
{
    public IEnumerable<Route> IntoRoutes() => DirectionsRoutes.Select(s => char.Parse(s).ToRoute());

    public string IntoString() => DirectionsRoutes.Aggregate((s, s1) => $"{s}{s1}");

    public override string ToString()
    {
        var s = DirectionsRoutes.Aggregate((s, s1) => $"{s} -> {s1} ");
        return $"\nRoute: {s}\nDistance: {Distance}\nStops: {Stops}";
    }
}