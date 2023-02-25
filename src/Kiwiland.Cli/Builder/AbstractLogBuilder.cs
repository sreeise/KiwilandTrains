using System.Text;
using Kiwiland.Cli.RoutesFileConfig;
using Kiwiland.RouteComputation;

namespace Kiwiland.Cli.Builder;

public abstract class AbstractLogBuilder
{
    private readonly StringBuilder _stringBuilder = new();
    private int _output;

    public abstract AbstractLogBuilder FindRouteDistance(IEnumerable<IEnumerable<Route>> routes);
    public abstract AbstractLogBuilder FindRoutesLessThanMaxStops(Route start, Route end, int k);
    public abstract AbstractLogBuilder FindRoutesEqualToMaxStops(Route start, Route end, int k);
    public abstract AbstractLogBuilder ShortestRoute(Route start, Route end);
    public abstract AbstractLogBuilder ShortestRoute(IEnumerable<ShortestRoute> shortestRoutes);
    public abstract AbstractLogBuilder FindRoutesLessThanMaxDistance(Route start, Route end, int maxDistance);

    protected void AppendLog(string output)
    {
        _output++;
        _stringBuilder.Append($"Output #{_output}: {output}\n");
    }

    protected void AppendLog(int output)
    {
        _output++;
        _stringBuilder.Append($"Output #{_output}: {output}\n");
    }

    public void Build() => Console.WriteLine(_stringBuilder.ToString());

    public override string ToString()
    {
        return _stringBuilder.ToString();
    }
}