using System.CommandLine;
using Kiwiland.Cli.Builder;
using Kiwiland.RouteComputation;

var routes = new List<List<Route>>()
{
    new List<Route>() {Route.A, Route.B, Route.C},
    new List<Route>() {Route.A, Route.D},
    new List<Route>() {Route.A, Route.D, Route.C},
    new List<Route>() {Route.A, Route.E, Route.B, Route.C, Route.D},
    new List<Route>() {Route.A, Route.E, Route.D}
};
/*
 // Uncomment to run input based on the trains scenario.
LogBuilder.Input("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7")
    .FindRouteDistance(routes)
    .FindDistanceGivenK(Route.C, Route.C, 3)
    .FindDistanceEqualToK(Route.A, Route.C, 4)
    .ShortestRoute(Route.A, Route.C)
    .ShortestRoute(Route.B, Route.B)
    .MaxDistance(Route.C, Route.C, 30)
    .Build();
 */

var rootCommand = new RootCommand("Kiwiland CLI for providing information on railroad routes and distances");
var routesOption = new Option<string[]>(
        name: "--routes",
        description: "List of routes in the format: AB5 BC4")
{ IsRequired = true, AllowMultipleArgumentsPerToken = true };

rootCommand.AddOption(routesOption);
rootCommand.SetHandler((routeStr) =>
{
    LogBuilder.Input(string.Join(' ', routeStr))
        .FindRouteDistance(routes)
        .FindDistanceGivenK(Route.C, Route.C, 3)
        .FindDistanceEqualToK(Route.A, Route.C, 4)
        .ShortestRoute(Route.A, Route.C)
        .ShortestRoute(Route.B, Route.B)
        .MaxDistance(Route.C, Route.C, 30)
        .Build();
}, routesOption);

return await rootCommand.InvokeAsync(args);






