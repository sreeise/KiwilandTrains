using System.Text;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Digraph;
using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.Cli.Builder;

public class LogBuilder : ILogBuilder
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private readonly IGraph _graph;
    private int _output;

    private LogBuilder(string input)
    {
        var adj = Helper.MapInput(input);
        _graph = new Graph(adj);
        _output = 0;
    }


    public static ILogBuilder Input(string input)
    {
        return new LogBuilder(input);
    }


    public ILogBuilder WithRouteDistance(IEnumerable<IEnumerable<Route>> routes)
    {
        var r = routes.ToList();
        for (var i = 0; i < r.Count; i++)
        {
            _output += 1;
            var (distance, hasRoute) = _graph.RouteDistance(r[i]);
            if (!hasRoute) _stringBuilder.Append($"Output #{_output}: NO SUCH ROUTE\n");
            else _stringBuilder.Append($"Output #{_output}: {distance}\n");
        }
        return this;
    }

    public ILogBuilder WithDistanceGivenK(Route start, Route end, int k)
    {
        _output += 1;
        var totalRoutes = _graph.FindRoutesGivenK(start, end, k);
        _stringBuilder.Append($"Output #{_output}: {totalRoutes}\n");
        return this;
    }

    public ILogBuilder WithDistanceEqualToK(Route start, Route end, int k)
    {
        _output += 1;
        var totalRoutes = _graph.FindRoutesEqualToK(start, end, k);
        _stringBuilder.Append($"Output #{_output}: {totalRoutes}\n");
        return this;
    }

    public ILogBuilder WithShortestRoute(Route start, Route end)
    {
        _output += 1;
        var shortestRoute = _graph.ShortestRoute(start, end);
        _stringBuilder.Append($"Output #{_output}: {shortestRoute}\n");
        return this;
    }

    public ILogBuilder WithMaxDistance(Route start, Route end, int maxDistance)
    {
        _output += 1;
        var totalRoutes = _graph.RoutesWithMaxDistance(start, end, maxDistance);
        _stringBuilder.Append($"Output #{_output}: {totalRoutes}\n");
        return this;
    }

    public string Build()
    {
        return _stringBuilder.ToString();
    }
}