using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;

namespace RouteComputationTests;

public class LogBuilderTests
{
    [Fact]
    public void OutputEqualsTest()
    {
        const string s = @"Output #1: 9\nOutput #2: 5\nOutput #3: 13\nOutput #4: 22\nOutput #5: NO SUCH ROUTE\nOutput #6: 2\nOutput #7: 3\nOutput #8: 9\nOutput #9: 9\nOutput #10: 7";

        var routes = new List<List<Route>>()
        {
            new List<Route>() {Route.A, Route.B, Route.C},
            new List<Route>() {Route.A, Route.D},
            new List<Route>() {Route.A, Route.D, Route.C},
            new List<Route>() {Route.A, Route.E, Route.B, Route.C, Route.D},
            new List<Route>() {Route.A, Route.E, Route.D}
        };

        var output = LogBuilder.Input("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7")
            .FindRouteDistance(routes)
            .FindDistanceGivenK(Route.C, Route.C, 3)
            .FindDistanceEqualToK(Route.A, Route.C, 4)
            .ShortestRoute(Route.A, Route.C)
            .ShortestRoute(Route.B, Route.B)
            .MaxDistance(Route.C, Route.C, 30)
            .ToString();
        Assert.Equal(output.Trim().Remove(' ').Remove('\n'), s.Trim().Remove(' ').Remove('\n'));
    }
}