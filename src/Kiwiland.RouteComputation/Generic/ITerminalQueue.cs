using Kiwiland.RouteComputation.Core;

namespace Kiwiland.RouteComputation.Generic;

public interface ITerminalQueue
{
    void Enqueue(Terminal terminal, TrainRoute route);

    void Enqueue(Terminal terminal, TrainRoute route, int distance);

    KeyValuePair<Terminal, TrainRoute> Dequeue();

    bool IsEmpty();
}