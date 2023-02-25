using System.Text;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.core;
using Kiwiland.RouteComputation.Generic;
using Newtonsoft.Json;

namespace Kiwiland.Cli.Builder;

public class LogBuilder : AbstractLogBuilder
{
    private TerminalGateway Gateway { get; set; }

    private LogBuilder(string input)
    {
        Gateway = Helper.TerminalGateway(input);
    }

    public static AbstractLogBuilder Input(string input)
    {
        return new LogBuilder(input);
    }

    public override AbstractLogBuilder FindRouteDistance(IEnumerable<IEnumerable<Route>> routes)
    {
        var r = routes.ToList();
        for (var i = 0; i < r.Count; i++)
        {
            var (distance, hasRoute) = Gateway.RouteDistance(r[i]);
            if (!hasRoute) AppendLog("NO SUCH ROUTE");
            else AppendLog(distance);
        }
        return this;
    }

    public override AbstractLogBuilder FindDistanceGivenK(Route start, Route end, int k)
    {
        var totalRoutes = Gateway.FindRoutesLessThanMaxStops(start, end, k);
        AppendLog(totalRoutes.Count());
        return this;
    }

    public override AbstractLogBuilder FindDistanceEqualToK(Route start, Route end, int k)
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

    public override AbstractLogBuilder MaxDistance(Route start, Route end, int maxDistance)
    {
        var totalRoutes = Gateway.FindRoutesLessThanMaxDistance(start, end, maxDistance).ToList();
        AppendLog(totalRoutes.Count);
        return this;
    }
}