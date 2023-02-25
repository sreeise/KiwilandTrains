namespace Kiwiland.RouteComputation.core;

/// <summary>
/// Terminal represents a train station or railroad terminal.
///
/// Each Terminal has a Dictionary that stores the Terminals that can be reached
/// from the current Terminal tracks along with the distance to that Terminal.
/// </summary>
public class Terminal
{
    public Terminal()
    {
        StationName = string.Empty;
    }

    public Terminal(string stationName)
    {
        StationName = stationName;
    }

    /// <summary>
    /// The name of the current Terminal.
    /// </summary>
    public string StationName { get; set; }

    public int StationId => (int)char.Parse(StationName).ToRoute();

    /// <summary>
    /// Dictionary that stores the Terminals that can be reached from the
    /// current Terminal tracks along with the distance to that Terminal
    /// </summary>
    public IDictionary<Terminal, int> RoutesDictionary { get; set; } = new Dictionary<Terminal, int>();


    /// <summary>
    /// Add a destination Terminal and distance for a new route from the current station to the given terminal.
    /// </summary>
    /// <param name="terminal">The Destination Terminal</param>
    /// <param name="distanceToTerminal">The distance from this Terminal to the destination Terminal given</param>
    public void AddRoute(Terminal terminal, int distanceToTerminal) => RoutesDictionary.Add(terminal, distanceToTerminal);
}