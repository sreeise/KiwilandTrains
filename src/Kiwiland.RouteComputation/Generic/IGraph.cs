namespace Kiwiland.RouteComputation.Generic;

public interface IGraph
{
    int ShortestRoute(Route sourceRoute, Route destinationRoute);

    (int, bool) RouteDistance(IEnumerable<Route> routes);

    int RoutesWithMaxDistance(Route start, Route end, int maxDistance);

    int FindRoutesGivenK(Route start, Route end, int k);

    int FindRoutesEqualToK(Route start, Route end, int k);
}