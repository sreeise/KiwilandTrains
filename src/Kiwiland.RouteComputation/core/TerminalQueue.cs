using Kiwiland.RouteComputation.Generic;
using Newtonsoft.Json;

namespace Kiwiland.RouteComputation.core;

public class TerminalQueue : ITerminalQueue
{
    private readonly Queue<KeyValuePair<Terminal, TrainRoute>> _trainQueue;

    public TerminalQueue()
    {
        _trainQueue = new Queue<KeyValuePair<Terminal, TrainRoute>>();
    }

    public void Enqueue(Terminal terminal, TrainRoute route)
        => _trainQueue.Enqueue(new KeyValuePair<Terminal, TrainRoute>(terminal, new TrainRoute()
        {
            Directions = $"{route.Directions} {terminal.StationName} =>",
            Distance = route.Distance
        }));

    public void Enqueue(Terminal terminal, TrainRoute route, int distance)
        => _trainQueue.Enqueue(new KeyValuePair<Terminal, TrainRoute>(terminal, new TrainRoute()
        {
            Directions = $"{route.Directions} {terminal.StationName} =>",
            Distance = route.Distance + distance
        }));

    public KeyValuePair<Terminal, TrainRoute> Dequeue() => _trainQueue.Dequeue();

    public bool IsEmpty() => _trainQueue.Count <= 0;

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
