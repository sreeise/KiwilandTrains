using Kiwiland.RouteComputation;

namespace Kiwiland.Cli.RoutesFileConfig;

public class FileConfig
{
    public IEnumerable<string>? Routes { get; set; }
    public IEnumerable<IEnumerable<Route>>? FindRoutesDistances { get; set; }
    public RoutesWithNum? RoutesLessThanMaxStops { get; set; }
    public RoutesWithNum? RoutesEqualToMaxStops { get; set; }
    public RoutesWithNum? RoutesLessThanMaxDistance { get; set; }
    public IEnumerable<ShortestRoute>? ShortestRoutes { get; set; }
}