## Kiwiland Trains 

### Build and Run

- Requires .net6.0


    dotnet build

## Pass routes in a single line

This will use the routes distances to find, max distance, and max stops provided in the scenario.
You can change the routes but it will always use the given scenario values. See file configuration
or input as separate commands for more options.

    dotnet run --project src\Kiwiland.Cli --routes AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7

## File Configuration

There is a file_config.json in the root of this repo with the correct config
and uses the routes given in the scenario. Replace file_config.json below
with the full path to file_config.json: `~./path/to/file_config.json` or `C:\Users\userName\KiwiLandTrans\file_config.json`

You can change anything in the file.

    dotnet run --project src\Kiwiland.Cli file --file file_config.json

For example, here is the JSON for file_config.json based on inputs for the given scenario:

    {
        "Routes": ["AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7"],
        "FindRoutesDistances": [
            ["A", "B", "C"],
            ["A", "D"],
            ["A", "D", "C"],
            ["A", "E", "B", "C", "D"],
            ["A", "E", "D"]
        ],
        "RoutesLessThanMaxStops": {
            "StartDest": "C",
            "EndDest": "C",
            "Num": 3
        },
        "RoutesEqualToMaxStops": {
            "StartDest": "A",
            "EndDest": "C",
            "Num": 4
        },
        "ShortestRoutes": [
            { "StartDest": "A", "EndDest": "C" },
            { "StartDest": "B", "EndDest": "B" }
        ],
        "RoutesLessThanMaxDistance": {
            "StartDest": "C",
            "EndDest": "C",
            "Num": 30
        }
    }

## Provide input as separate commands

You can provide routes as separate commands. Only difference between this and the file configuration is that you are limited
to only providing a single input for each command. With the file configuration you can provide multiple routes to get distances
and you can provide multiple routes to get shortest route distances.

This is the only input option that will not print output in line with scenario. This option will print the 4 outputs based on command arguments
explained below.


    dotnet run --project src\Kiwiland.Cli routes-options --routes AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7 --distance A B C --max-stops A C 4 
        --max-stops-less-than C C 3 --max-distance C C 30


- `--distance`: Gets the distance of following exact routes provided or if no route exists then prints NO SUCH ROUTE. You can provide as many routes from A - E
- `--max-stops`: Gets the total routes with exact stops given. This command takes 3 characters: startDestination, endDestination, maxStops such as A C 4
- `--max-stops-less-than`: Gets the total routes less than stops given. This command takes 3 characters: startDestination, endDestination, maxStops such as C C 3
- `--max-distance`: Gets the total routes less than max distance. This command takes 3 characters: startDestination, endDestination, maxDistance such as C C 30


### There are three projects:

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
