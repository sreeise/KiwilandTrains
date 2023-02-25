using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.core;

namespace RouteComputationTests;

public class MaxDistanceTests
{
    private TerminalGateway Gateway { get; }

    public MaxDistanceTests()
    {
        Gateway = Helper.TerminalGateway("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
    }

    [Fact]
    public void MaxDistanceTest()
    {
        var routes = Gateway.FindRoutesLessThanMaxDistance(Route.C, Route.C, 30).ToList();
        Assert.Equal(7, routes.Count);
    }

    [Fact]
    public void FindRoutesLessThanMaxDistance()
    {
        var routeAns = new List<string>() { "CDC", "CEBC", "CEBCDC", "CDCEBC", "CDEBC", "CEBCEBC", "CEBCEBCEBC" };
        routeAns.Sort();

        var routeList = Gateway.FindRoutesLessThanMaxDistance("C", "C", 30).ToList();

        Assert.Equal(7, routeList.Count);

        var distances = routeList.Select(x => x.Distance);
        var routeStrings = routeList.Select(r => r.IntoString()).ToList();
        routeStrings.Sort();

        // XUnit will do a deep equal of the values.
        Assert.Equal(routeAns, routeStrings);
        Assert.All(distances, distance => Assert.True(distance < 30));
    }
}