using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Core;

namespace RouteComputationTests;

public class RouteDistanceTests
{
    private TerminalGateway Gateway { get; }

    public RouteDistanceTests()
    {
        Gateway = Helper.TerminalGateway("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
    }

    [Theory]
    [InlineData(Route.A, Route.C, 9)]
    [InlineData(Route.A, Route.D, 5)]
    [InlineData(Route.A, Route.B, 5)]
    [InlineData(Route.C, Route.D, 8)]
    [InlineData(Route.D, Route.D, 16)]
    public void ShortestRouteTest(Route source, Route destination, int weight)
    {
        var routeDistance = Gateway.ShortestRoute(source, destination);
        Assert.Equal(weight, routeDistance);
    }

    [Fact]
    public void DistanceGivenKTest()
    {
        // trips: C-D-C (2 stops). and C-E-B-C
        var routes = Gateway.FindRoutesLessThanMaxStops(Route.C, Route.C, 3).ToList();
        Assert.Equal(2, routes.Count);

        var routeStrings = routes.Select(route => route.IntoString()).ToList();
        Assert.Contains(routeStrings, r => r == "CDC");
        Assert.Contains(routeStrings, r => r == "CEBC");
    }

    [Fact]
    public void DistanceEqualToKTest()
    {
        var routes = Gateway.FindRoutesEqualToMaxStops(Route.A, Route.C, 4).ToList();
        Assert.Equal(3, routes.Count);

        var routeStrings = routes.Select(route => route.IntoString()).ToList();
        Assert.Contains(routeStrings, r => r == "ABCDC");
        Assert.Contains(routeStrings, r => r == "ADCDC");
        Assert.Contains(routeStrings, r => r == "ADEBC");
    }
}