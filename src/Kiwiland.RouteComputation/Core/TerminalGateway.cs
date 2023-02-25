using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.RouteComputation.Core;

/// <summary>
/// TerminalGateway is a central hub for computing train route distances, total maxStops,
/// and shortest distance routes.
/// </summary>
public class TerminalGateway : IGatewayGraph<Terminal>
{
    /// <summary>
    /// Used to find routes given max distance and max maxStops.
    /// </summary>
    private readonly ITerminalQueue _terminalQueue;
    private readonly IDictionary<int, Terminal> _terminals;

    public TerminalGateway()
    {
        _terminalQueue = new TerminalQueue();
        _terminals = new Dictionary<int, Terminal>();
    }

    public TerminalGateway(ITerminalQueue terminalQueue)
    {
        _terminalQueue = terminalQueue;
        _terminals = new Dictionary<int, Terminal>();
    }

    public TerminalGateway(IDictionary<int, Terminal> terminals)
    {
        _terminalQueue = new TerminalQueue();
        _terminals = terminals;
    }

    public Terminal AddNode(string stationName)
    {
        var terminal = new Terminal(stationName);
        if (!_terminals.ContainsKey(terminal.StationId)) _terminals.Add(terminal.StationId, terminal);
        return _terminals[terminal.StationId];
    }

    public Terminal AddNode(Terminal terminal)
    {
        if (!_terminals.ContainsKey(terminal.StationId)) _terminals.Add(terminal.StationId, terminal);
        return _terminals[terminal.StationId];
    }

    public Terminal? GetNode(string stationName)
    {
        var terminal = new Terminal(stationName);
        return !_terminals.ContainsKey(terminal.StationId) ? null : _terminals[terminal.StationId];
    }

    public bool HasNode(string stationName) => _terminals.ContainsKey(new Terminal(stationName).StationId);

    public bool HasNode(Terminal terminal) => _terminals.ContainsKey(terminal.StationId);

    public void AddRoute(string startStationName, string endStationName, int distance)
    {
        var t1 = AddNode(startStationName);
        var t2 = AddNode(endStationName);
        t1.AddRoute(t2, distance);
    }

    public void AddRoute(Terminal t1, Terminal t2, int distance) => t1.AddRoute(t2, distance);

    /// <summary>
    /// Find all routes from start destination to end destination with a route distance less than
    /// the maxDistance given.
    /// </summary>
    /// <param name="startDestination">The destination to start at for the finding routes</param>
    /// <param name="endDestination">
    /// The destination to find all paths to the destination with a maxDistance less than 30
    /// </param>
    /// <param name="maxDistance">The maximum distance that a set of routes can add up to</param>
    /// <returns>
    /// All routes that can be made from startDestination to endDestination with a distance less than max distance
    /// </returns>
    private IEnumerable<RouteInformation> RoutesLessThanMaxDistance(Route startDestination, Route endDestination, int maxDistance)
    {
        var startStationId = (int)startDestination;
        var end = endDestination.ToString();
        _terminalQueue.Enqueue(_terminals[startStationId], TrainRoute.None);

        while (!_terminalQueue.IsEmpty())
        {
            var (currentTerminal, currentTrainRoute) = _terminalQueue.Dequeue();

            foreach (var (terminal, distance) in currentTerminal.RoutesDictionary)
            {
                // Skip iteration if current distance of traversed routes is greater than max distance.
                if ((currentTrainRoute.Distance + distance) >= maxDistance) continue;

                // Enqueue the next route onto the queue.
                _terminalQueue.Enqueue(terminal, currentTrainRoute, distance);
                // If we reached the destination station then we yield the current train route which is a record
                // of the exact routes taken, the distance of that exact route, and the total maxStops of the route.
                if (terminal.StationName == end) yield return currentTrainRoute.BuildRouteInformation(end, distance);
            }
        }
    }

    public IEnumerable<RouteInformation> FindRoutesLessThanMaxDistance(Route startDestination, Route endDestination,
        int maxDistance)
        => RoutesLessThanMaxDistance(startDestination, endDestination, maxDistance);

    public IEnumerable<RouteInformation> FindRoutesLessThanMaxDistance(string startDestination, string endDestination,
        int maxDistance)
        => RoutesLessThanMaxDistance(char.Parse(startDestination).ToRoute(), char.Parse(endDestination).ToRoute(),
            maxDistance);

    /// <summary>
    /// Find all routes from start destination to end destination with total route stops less than
    /// the <param name="maxStops">max stops</param> given.
    /// </summary>
    /// <param name="startDestination">The destination to start at for the finding routes</param>
    /// <param name="endDestination">The destination to end at for finding routes</param>
    /// <param name="maxStops">The maximum route stops</param>
    /// <returns>
    /// All routes that can be made from startDestination to endDestination with less than
    /// <param name="maxStops">max stops</param>.
    /// </returns>
    private IEnumerable<RouteInformation> RoutesLessThanMaxStops(Route startDestination, Route endDestination, int maxStops)
    {
        var startStationId = (int)startDestination;
        var end = endDestination.ToString();
        _terminalQueue.Enqueue(_terminals[startStationId], TrainRoute.None);

        while (!_terminalQueue.IsEmpty())
        {
            var (currentTerminal, currentTrainRoute) = _terminalQueue.Dequeue();

            foreach (var (terminal, distance) in currentTerminal.RoutesDictionary)
            {
                // Skip iteration if current maxStops of traversed routes is greater than maxStops.
                if (currentTrainRoute.Stops > maxStops) continue;

                // Enqueue the next route onto the queue.
                _terminalQueue.Enqueue(terminal, currentTrainRoute, distance);

                // If we reached the destination station then we yield the current train route which is a record
                // of the exact routes taken, the distance of that exact route, and the total maxStops of the route.
                if (terminal.StationName == end) yield return currentTrainRoute.BuildRouteInformation(end, distance);
            }
        }
    }

    public IEnumerable<RouteInformation> FindRoutesLessThanMaxStops(string startDestination, string endDestination, int k) =>
        RoutesLessThanMaxStops(char.Parse(startDestination).ToRoute(), char.Parse(endDestination).ToRoute(), k);

    public IEnumerable<RouteInformation> FindRoutesLessThanMaxStops(Route startDestination, Route endDestination, int maxStops) =>
        RoutesLessThanMaxStops(startDestination, endDestination, maxStops);


    /// <summary>
    /// Find all routes from start destination to end destination with total route stops less than
    /// the <param name="maxStops">max stops</param> given.
    /// </summary>
    /// <param name="startDestination">The destination to start at for the finding routes</param>
    /// <param name="endDestination">The destination to end at for finding routes</param>
    /// <param name="maxStops">The maximum route stops</param>
    /// <returns>
    /// All routes that can be made from startDestination to endDestination with less than
    /// <param name="maxStops">max stops</param>.
    /// </returns>
    private IEnumerable<RouteInformation> RoutesEqualToMaxStops(Route startDestination, Route endDestination, int maxStops)
    {
        var startStationId = (int)startDestination;
        var end = endDestination.ToString();
        _terminalQueue.Enqueue(_terminals[startStationId], TrainRoute.None);

        while (!_terminalQueue.IsEmpty())
        {
            var (currentTerminal, currentTrainRoute) = _terminalQueue.Dequeue();
            foreach (var (terminal, distance) in currentTerminal.RoutesDictionary)
            {
                // Skip iteration if current maxStops of traversed routes is greater than maxStops.
                if (currentTrainRoute.Stops > maxStops) continue;

                // Enqueue the next route onto the queue.
                _terminalQueue.Enqueue(terminal, currentTrainRoute, distance);

                // If we reached the destination station then we yield the current train route which is a record
                // of the exact routes taken, the distance of that exact route, and the total maxStops of the route.
                if (terminal.StationName == end && currentTrainRoute.Stops == maxStops)
                    yield return currentTrainRoute.BuildRouteInformation(end, distance);
            }
        }
    }

    public IEnumerable<RouteInformation> FindRoutesEqualToMaxStops(string startDestination, string endDestination, int maxStops) =>
        RoutesEqualToMaxStops(char.Parse(startDestination).ToRoute(), char.Parse(endDestination).ToRoute(), maxStops);

    public IEnumerable<RouteInformation> FindRoutesEqualToMaxStops(Route startDestination, Route endDestination, int maxStops) =>
        RoutesEqualToMaxStops(startDestination, endDestination, maxStops);

    /// <summary>
    /// Find shortest distance from one destination to another using
    /// breadth first search algorithm.
    ///
    /// Time complexity is O(E Log V). This is basically Dijkstra's Shortest Path Algorithm.
    /// </summary>
    /// <param name="startDestination">The route to start at.</param>
    /// <param name="endDestination">The route to end at</param>
    /// <returns></returns>
    public int ShortestRoute(Route startDestination, Route endDestination)
    {
        var source = (int)startDestination;
        var destination = (int)endDestination;

        _terminalQueue.Enqueue(_terminals[source], TrainRoute.None);

        var distance = new int[_terminals.Count];
        Array.Fill(distance, -1);
        distance[source] = 0;

        while (!_terminalQueue.IsEmpty())
        {
            var (currentTerminal, currentTrainRoute) = _terminalQueue.Dequeue();
            foreach (var (terminal, dist) in currentTerminal.RoutesDictionary)
            {
                // If we have already seen this terminal then skip the current iteration.
                if (distance[terminal.StationId] > 0) continue;

                // Update the distance array with the added distance of the current route.
                distance[terminal.StationId] = dist + distance[currentTerminal.StationId];
                _terminalQueue.Enqueue(terminal, currentTrainRoute);
            }
        }

        // `distance` is an array of int values representing the shortest distances for any given route
        // starting at the given sourceRoute. In this instance, we store the shortest distance calculation
        // at the index of destination.
        return distance[destination];
    }

    public int ShortestRoute(string startDestination, string endDestination) =>
        ShortestRoute(char.Parse(startDestination).ToRoute(), char.Parse(endDestination).ToRoute());

    /// <summary>
    /// Calculate total distance of a specific route. For instance if you want to
    /// calculate a routes distance when going from A to B to C then provide
    /// a list of those routes in that order.
    ///
    /// Time complexity in the worst case is O(N + M) where N is the total number of routes and
    /// M is the total edges that each route has.
    ///
    /// Routes must be unique in terms of the Route enum or the wrong distance will be returned.
    /// </summary>
    /// <param name="routes">List of routes</param>
    /// <returns>
    /// (int: Distance, bool: HasRoute)
    ///     - where int is the distance of the route taken or -1 if there is no route
    ///     - bool is true if there is a route otherwise false
    ///
    /// </returns>
    public (int, bool) RouteDistance(IEnumerable<Route> routes)
    {
        var routesList = routes.Select(route => (int)route).ToList();
        var queue = new Queue<int>(routesList);
        var start = queue.Dequeue();
        var distance = 0;
        var terminal = _terminals[start];

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            var t = terminal.RoutesDictionary.Keys.FirstOrDefault(t => t.StationId == next);
            if (t == null) return (-1, false);
            distance += terminal.RoutesDictionary[t];
            terminal = t;
        }

        return distance > 0 ? (distance, true) : (-1, false);
    }

    public (int, bool) RouteDistance(IEnumerable<string> routes) =>
        RouteDistance(routes.Select(s => char.Parse(s).ToRoute()));
}