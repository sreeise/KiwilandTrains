using Kiwiland.RouteComputation.Core;

namespace Kiwiland.RouteComputation.Generic;

public interface IGatewayGraph<T>
{
    T AddNode(T node);

    void AddRoute(T node1, T node2, int distance);

    T? GetNode(string nodeName);

    bool HasNode(string nodeName);

    bool HasNode(T node);

    IEnumerable<RouteInformation> FindRoutesLessThanMaxDistance(string startDestination, string endDestination,
        int maxDistance);

    IEnumerable<RouteInformation> FindRoutesLessThanMaxStops(string startDestination, string endDestination, int k);

    IEnumerable<RouteInformation> FindRoutesEqualToMaxStops(string startDestination, string endDestination, int maxStops);

    int ShortestRoute(string startDestination, string endDestination);

    (int, bool) RouteDistance(IEnumerable<Route> routes);
}