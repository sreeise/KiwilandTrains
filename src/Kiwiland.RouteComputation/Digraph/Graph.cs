using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.RouteComputation.Digraph;

public class Graph : IGraph, INodeMap<int, Node>
{
    private Dictionary<int, Node> Map { get; set; }

    public Graph()
    {
        Map = new Dictionary<int, Node>();
    }

    public Graph(Dictionary<int, Node> adj)
    {
        Map = adj;
    }

    public Node this[int i]
    {
        get => Map[i];
        set => Map[i] = value;
    }

    public void AddEdge(int key, IEdge edge)
    {
        Map[key].Edges.Add(edge);
    }

    public IEnumerable<IEdge>? GetEdges(int key)
    {
        return Map[key].Edges;
    }

    public void AddNode(int key, Node node)
    {
        Map[key] = node;
    }

    public Node? GetNode(int key)
    {
        return Map[key];
    }

    /// <summary>
    /// Find shortest distance from one destination to another using
    /// breadth first search algorithm.
    ///
    /// Time complexity is O(E Log V). This is basically Dijkstra's Shortest Path
    /// Algorithm.
    /// </summary>
    /// <param name="sourceRoute">The route to start at.</param>
    /// <param name="destinationRoute"></param>
    /// <returns></returns>
    public int ShortestRoute(Route sourceRoute, Route destinationRoute)
    {
        var source = (int)sourceRoute;
        var destination = (int)destinationRoute;

        var distance = new int[Map.Count];
        Array.Fill(distance, -1);

        distance[source] = 0;
        var queue = new Queue<Node>();
        queue.Enqueue(Map[source]);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            foreach (var edge in current.Edges)
            {
                var edgeVertexDistance = distance[edge.Vertex];
                var currentVertexDistance = distance[current.Vertex];
                if (edgeVertexDistance <= 0)
                {
                    distance[edge.Vertex] = edge.Weight + currentVertexDistance;
                    queue.Enqueue(Map[edge.Vertex]);
                }
            }
        }

        return distance[destination];
    }

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
    /// <returns></returns>
    public (int, bool) RouteDistance(IEnumerable<Route> routes)
    {
        var routesList = routes.Select(route => (int)route).ToList();
        var queue = new Queue<int>(routesList);
        var start = queue.Dequeue();
        var weight = 0;
        var node = Map[start];

        while (queue.Count > 0)
        {
            var next = queue.Dequeue();
            var nextEdge = node.Edges.Find(edge => edge.Vertex == next);
            if (nextEdge == null) return (-1, false);
            weight += nextEdge.Weight;
            node = Map[next];
        }

        return weight > 0 ? (weight, true) : (-1, false);
    }

    private int RoutesWithMaxDistance(int start, int end, int distance, int maxDistance, int totalRoutes)
    {
        if (start == end && distance < maxDistance) totalRoutes++;
        if (distance >= maxDistance) return totalRoutes;

        var edges = Map[start].Edges;
        return edges.Aggregate(totalRoutes, (current, edge) => RoutesWithMaxDistance(edge.Vertex, end, distance + edge.Weight, maxDistance, current));
    }

    /// <summary>
    /// Find total routes from given start route to ending start route with distance less than maxDistance.
    /// </summary>
    /// <param name="start">Starting Route</param>
    /// <param name="end">Ending Route</param>
    /// <param name="maxDistance">The max distance of routes to return</param>
    /// <returns>Int: Total amount of routes</returns>
    public int RoutesWithMaxDistance(Route start, Route end, int maxDistance) =>
        RoutesWithMaxDistance((int)start, (int)end, 0, maxDistance, -1);

    private int FindRoutesGivenK(int start, int end, int k, int visited, int routes)
    {
        if (start == end && visited > 0) routes++;
        if (visited == k) return routes;

        var edges = Map[start].Edges;
        return edges.Aggregate(routes, (current, edge) => FindRoutesGivenK(edge.Vertex, end, k, visited + 1, current));
    }

    /// <summary>
    /// Find number of routes less than or equal to k that can be taken from a given start to a given destination 
    /// </summary>
    /// <param name="start">Starting Route</param>
    /// <param name="end">Ending Route</param>
    /// <param name="k">Number of routes to find less than or equal to k</param>
    /// <returns>Int: total amount of routes</returns>
    public int FindRoutesGivenK(Route start, Route end, int k) =>
        FindRoutesGivenK((int)start, (int)end, k, 0, 0);

    /// <summary>
    /// Get the total routes with a specific amount of stops given the start Route and end Route
    /// </summary>
    /// <param name="start">Starting Route</param>
    /// <param name="end">Ending Route</param>
    /// <param name="k">Max stops</param>
    /// <returns>Int: Total routes with specific amount of stops</returns>
    public int FindRoutesEqualToK(Route start, Route end, int k) =>
        FindRoutesGivenK((int)start, (int)end, k, 0, 0) - FindRoutesGivenK((int)start, (int)end, k - 1, 0, 0);
}

