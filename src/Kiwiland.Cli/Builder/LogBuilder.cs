using Kiwiland.Cli.RoutesFileConfig;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Core;

namespace Kiwiland.Cli.Builder;

public class LogBuilder : AbstractLogBuilder
{
    private TerminalGateway Gateway { get; set; }

    private LogBuilder(string input)
    {
        Gateway = Helper.TerminalGateway(input);
    }

    public static AbstractLogBuilder Input(string input) => new LogBuilder(input);

    public override AbstractLogBuilder FindRouteDistance(IEnumerable<IEnumerable<Route>> routes)
    {
        var r = routes.Select(r => r.ToList()).ToList();
        for (var i = 0; i < r.Count; i++)
        {
            var (distance, hasRoute) = Gateway.RouteDistance(r[i]);
            if (!hasRoute) AppendLog("NO SUCH ROUTE");
            else AppendLog(distance);
        }
        return this;
    }

    public override AbstractLogBuilder FindRoutesLessThanMaxStops(Route start, Route end, int k)
    {
        var totalRoutes = Gateway.FindRoutesLessThanMaxStops(start, end, k);
        AppendLog(totalRoutes.Count());
        return this;
    }

    public override AbstractLogBuilder FindRoutesEqualToMaxStops(Route start, Route end, int k)
    {
        var totalRoutes = Gateway.FindRoutesEqualToMaxStops(start, end, k);
        AppendLog(totalRoutes.Count());
        return this;
    }

    public override AbstractLogBuilder ShortestRoute(Route start, Route end)
    {
        var shortestRoute = Gateway.ShortestRoute(start, end);
        AppendLog(shortestRoute);
        return this;
    }

    public override AbstractLogBuilder ShortestRoute(IEnumerable<ShortestRoute> shortestRoutes)
    {
        foreach (var shortestRoute in shortestRoutes)
        {
            var ans = Gateway.ShortestRoute(shortestRoute.StartDest, shortestRoute.EndDest);
            AppendLog(ans);
        }

        return this;
    }

    public override AbstractLogBuilder FindRoutesLessThanMaxDistance(Route start, Route end, int maxDistance)
    {
        var totalRoutes = Gateway.FindRoutesLessThanMaxDistance(start, end, maxDistance).ToList();
        AppendLog(totalRoutes.Count);
        return this;
    }
}