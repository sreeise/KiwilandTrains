using System.Text;
using Kiwiland.RouteComputation;

namespace Kiwiland.Cli.Builder;

public abstract class AbstractLogBuilder
{
    private readonly StringBuilder _stringBuilder = new StringBuilder();
    private int _output = 0;

    public abstract AbstractLogBuilder FindRouteDistance(IEnumerable<IEnumerable<Route>> routes);
    public abstract AbstractLogBuilder FindDistanceGivenK(Route start, Route end, int k);
    public abstract AbstractLogBuilder FindDistanceEqualToK(Route start, Route end, int k);
    public abstract AbstractLogBuilder ShortestRoute(Route start, Route end);
    public abstract AbstractLogBuilder MaxDistance(Route start, Route end, int maxDistance);

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