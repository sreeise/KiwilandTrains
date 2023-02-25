using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Core;

namespace RouteComputationTests;

public class ExactRouteDistanceTests
{
    private TerminalGateway Gateway { get; }

    public ExactRouteDistanceTests()
    {
        Gateway = Helper.TerminalGateway("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
    }

    [Theory]
    [InlineData(Route.A, Route.B, Route.C, 9, true)]
    [InlineData(Route.A, Route.D, Route.C, 13, true)]
    [InlineData(Route.A, Route.E, Route.C, -1, false)]
    public void SpecificRouteTest(Route source, Route middleRoute, Route destination, int weight, bool hasRouteAnswer)
    {
        var routes = new List<Route>() { source, middleRoute, destination };
        var (distance, hasRoute) = Gateway.RouteDistance(routes);
        Assert.Equal(hasRoute, hasRouteAnswer);
        Assert.Equal(weight, distance);
    }
}