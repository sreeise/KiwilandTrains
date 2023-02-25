using System.CommandLine;
using Kiwiland.Cli.Builder;
using Kiwiland.Cli.RoutesFileConfig;
using Kiwiland.RouteComputation;
using Newtonsoft.Json;

/*
 // Uncomment to run input based on the trains scenario.
LogBuilder.Input("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7")
    .FindRouteDistance(routes)
    .FindRoutesLessThanMaxStops(Route.C, Route.C, 3)
    .FindRoutesEqualToMaxStops(Route.A, Route.C, 4)
    .ShortestRoute(Route.A, Route.C)
    .ShortestRoute(Route.B, Route.B)
    .FindRoutesLessThanMaxDistance(Route.C, Route.C, 30)
    .Build();
*/

var routes = new List<List<Route>>()
{
    new() {Route.A, Route.B, Route.C},
    new() {Route.A, Route.D},
    new() {Route.A, Route.D, Route.C},
    new() {Route.A, Route.E, Route.B, Route.C, Route.D},
    new() {Route.A, Route.E, Route.D}
};

var rootCommand = new RootCommand("Kiwiland CLI for providing information on railroad routes and distances");

var fileCommand = new Command("file");

var fileOption = new Option<FileInfo?>(
    name: "--file",
    description: "The file to read and display on the console.")
{ IsRequired = true, AllowMultipleArgumentsPerToken = true };

fileCommand.Add(fileOption);

// Handler for file option with chosen input up to the caller.
fileCommand.SetHandler((file) =>
{
    if (file == null) throw new ArgumentNullException(nameof(file));
    var text = File.ReadAllText(file.FullName);
    var fileConfig = JsonConvert.DeserializeObject<FileConfig>(text);

    if (fileConfig?.Routes == null ||
        fileConfig.FindRoutesDistances == null ||
        fileConfig.RoutesLessThanMaxStops == null ||
        fileConfig.RoutesEqualToMaxStops == null ||
        fileConfig.RoutesLessThanMaxDistance == null ||
        fileConfig.ShortestRoutes == null) return;

    var lessThanMax = fileConfig.RoutesLessThanMaxStops;
    var maxStops = fileConfig.RoutesEqualToMaxStops;
    var maxDistance = fileConfig.RoutesLessThanMaxDistance;
    LogBuilder.Input(string.Join(' ', fileConfig.Routes))
        .FindRouteDistance(fileConfig.FindRoutesDistances)
        .FindRoutesLessThanMaxStops(lessThanMax.StartDest, lessThanMax.EndDest, lessThanMax.Num)
        .FindRoutesEqualToMaxStops(maxStops.StartDest, maxStops.EndDest, maxStops.Num)
        .ShortestRoute(fileConfig.ShortestRoutes)
        .FindRoutesLessThanMaxDistance(maxDistance.StartDest, maxDistance.EndDest, maxDistance.Num)
        .Build();
}, fileOption);

rootCommand.AddCommand(fileCommand);

// Provide input for each command individually.
var optionCommand = new Command("routes-options", "Provide list of routes to get exact distance if the routes connect");

var routesOption = new Option<string[]>(
        name: "--routes",
        description: "List of routes in the format: AB5 BC4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };
var distanceOption = new Option<string[]>(
        name: "--distance",
        description: "List of routes in the format: AB5 BC4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };

var maxStopsOption = new Option<string[]>(
        name: "--max-stops",
        description: "Two routes and an int for max stops: A C 4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };

var lessThanMaxStopsOption = new Option<string[]>(
        name: "--max-stops-less-than",
        description: "Two routes and an int for max stops: A C 4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };

var maxDistanceOption = new Option<string[]>(
        name: "--max-distance",
        description: "Two routes and an int for max stops: A C 4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };

optionCommand.AddOption(routesOption);
optionCommand.AddOption(distanceOption);
optionCommand.AddOption(maxStopsOption);
optionCommand.AddOption(lessThanMaxStopsOption);
optionCommand.AddOption(maxDistanceOption);

// Handler for multi command input option.
// Kiwiland.Cli routes-options --routes AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7 --distance A B C --max-stops A C 4 --max-stops-less-than C C 3 --max-distance C C 30
optionCommand.SetHandler((routesList, distance, maxStopsOptions, lessThanMaxStopsOptions, maxDistanceArr) =>
{
    var gateway = Helper.TerminalGateway(string.Join(' ', routesList));
    var (routeDistance, isRoute) = gateway.RouteDistance(distance);
    var maxStops = gateway.FindRoutesEqualToMaxStops(maxStopsOptions[0], maxStopsOptions[1], int.Parse(maxStopsOptions[2])).ToList();
    var maxStopsLessThan = gateway.FindRoutesLessThanMaxStops(lessThanMaxStopsOptions[0], lessThanMaxStopsOptions[1], int.Parse(lessThanMaxStopsOptions[2])).ToList();
    var maxDistance = gateway.FindRoutesLessThanMaxDistance(maxDistanceArr[0], maxDistanceArr[1], int.Parse(maxDistanceArr[2])).ToList();

    var output = $"Output #2: {maxStops.Count}\nOutput #3: {maxStopsLessThan.Count}\nOutput #4: {maxDistance.Count}";
    if (isRoute) Console.WriteLine($"Output #1: {routeDistance}\n{output}");
    else Console.WriteLine($"Output #1: NO SUCH ROUTE\n{output}");

}, routesOption, distanceOption, maxStopsOption, lessThanMaxStopsOption, maxDistanceOption);

rootCommand.AddCommand(optionCommand);

var routesOptionCore = new Option<string[]>(
        name: "--routes",
        description: "List of routes in the format: AB5 BC4")
{ IsRequired = false, AllowMultipleArgumentsPerToken = true };

rootCommand.AddOption(routesOptionCore);

// Handler for list of routes by itself: --routes AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7
rootCommand.SetHandler((routeStr) =>
{
    LogBuilder.Input(string.Join(' ', routeStr))
        .FindRouteDistance(routes)
        .FindRoutesLessThanMaxStops(Route.C, Route.C, 3)
        .FindRoutesEqualToMaxStops(Route.A, Route.C, 4)
        .ShortestRoute(Route.A, Route.C)
        .ShortestRoute(Route.B, Route.B)
        .FindRoutesLessThanMaxDistance(Route.C, Route.C, 30)
        .Build();
}, routesOptionCore);

return await rootCommand.InvokeAsync(args);






