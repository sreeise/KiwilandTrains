using Kiwiland.Cli;
using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Digraph;

namespace RouteComputationTests;

public class RouteDistanceTests
{
    private readonly Graph _graph;

    public RouteDistanceTests()
    {
        var adj = Helper.MapInput("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7");
        _graph = new Graph(adj);
    }

    [Theory]
    [InlineData(Route.A, Route.C, 9)]
    [InlineData(Route.A, Route.D, 5)]
    [InlineData(Route.A, Route.B, 5)]
    [InlineData(Route.C, Route.D, 8)]
    [InlineData(Route.D, Route.D, 16)]
    public void ShortestRouteTest(Route source, Route destination, int weight)
    {
        var routeDistance = _graph.ShortestRoute(source, destination);
        Assert.Equal(weight, routeDistance);
    }

    [Theory]
    [InlineData(Route.A, Route.B, Route.C, 9, true)]
    [InlineData(Route.A, Route.D, Route.C, 13, true)]
    [InlineData(Route.A, Route.E, Route.C, -1, false)]
    public void SpecificRouteTest(Route source, Route middleRoute, Route destination, int weight, bool hasRouteAnswer)
    {
        var routes = new List<Route>() { source, middleRoute, destination };
        var (distance, hasRoute) = _graph.RouteDistance(routes);
        Assert.Equal(hasRoute, hasRouteAnswer);
        Assert.Equal(weight, distance);
    }

    [Fact]
    public void DistanceGivenKTest()
    {
        var totalRoutes = _graph.FindRoutesGivenK(Route.C, Route.C, 3);
        Assert.Equal(2, totalRoutes);
    }

    [Fact]
    public void DistanceEqualToKTest()
    {
        var totalRoutes = _graph.FindRoutesEqualToK(Route.A, Route.C, 4);
        Assert.Equal(3, totalRoutes);
    }

    [Fact]
    public void MaxDistanceTest()
    {
        var totalRoutes = _graph.RoutesWithMaxDistance(Route.C, Route.C, 30);
        Assert.Equal(7, totalRoutes);
    }

    [Fact]
    public void MaxDistanceUsingRoutesTest()
    {
        // CDC, CEBC, CEBCDC, CDCEBC, CDEBC, CEBCEBC, CEBCEBCEBC.
        var routes = new List<List<Route>>()
        {
            new List<Route>() {Route.C, Route.D, Route.C},
            new List<Route>() {Route.C, Route.E, Route.B, Route.C},
            new List<Route>() {Route.C, Route.E, Route.B, Route.C, Route.D, Route.C},
            new List<Route>() {Route.C, Route.D, Route.C, Route.E, Route.B, Route.C},
            new List<Route>() {Route.C, Route.D, Route.E, Route.B, Route.C},
            new List<Route>() {Route.C, Route.E, Route.B, Route.C, Route.E, Route.B, Route.C},
            new List<Route>() {Route.C, Route.E, Route.B, Route.C, Route.E, Route.B, Route.C, Route.E, Route.B, Route.C}
        };

        foreach (var routeList in routes)
        {
            var (distance, hasRoute) = _graph.RouteDistance(routeList);
            Assert.True(distance < 30);
            Assert.True(hasRoute);
        }
    }
}