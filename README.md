## Kiwiland Trains 

### Build and Run

- Requires .net6.0

Run `dotnet build`

Run `dotnet run --project src\Kiwiland.Cli --routes AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7`

There are three projects:

- Kiwiland.RouteComputation: Library for computing route information such as shortest distance.
- Kiwiland.Cli: A CLI that will run route computation on the routes given.
- RouteComputationTests: Runs tests for building the output and tests based on the trains scenario.

### Explanation

- Finding the shortest distance is done using breadth first search and in this case it is a version of Dijkstra's Shortest Path Algorithm.
Time complexity is O(E Log V).
- Finding the routes less than max distance as well as finding total routes equal to or less than a given number is
done using a similar approach to shortest distance with Dijkstra's algorithm and uses a queue to work through the nodes. 
The main difference is how the max stops and max distance use `yield return` to yield each set of routes that match
the given criteria. Time complexity is the same at O(E Log V).
- Finding the total distance of a specific route is done by just following the pointers to the next
edge in a weighted graph until the end is reached. Time complexity is O(N + M).
