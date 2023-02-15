using Kiwiland.RouteComputation;

namespace Kiwiland.Cli.Builder;

public interface ILogBuilder
{
    ILogBuilder WithRouteDistance(IEnumerable<IEnumerable<Route>> routes);
    ILogBuilder WithDistanceGivenK(Route start, Route end, int k);
    ILogBuilder WithDistanceEqualToK(Route start, Route end, int k);
    ILogBuilder WithShortestRoute(Route start, Route end);
    ILogBuilder WithMaxDistance(Route start, Route end, int maxDistance);
    string Build();
}