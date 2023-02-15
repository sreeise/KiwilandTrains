using Kiwiland.RouteComputation;
using Kiwiland.RouteComputation.Digraph;
using Kiwiland.RouteComputation.Generic;

namespace Kiwiland.Cli.Builder;

public abstract class Helper
{
    public static Dictionary<int, Node> MapInput(string input)
    {
        var list = input.Split(new[] { ' ', ',', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.ToCharArray())
            .ToList();

        var adj = new Dictionary<int, Node>();
        foreach (var charArray in list)
        {
            var source = (int)charArray[0].ToRoute();
            var destination = (int)charArray[1].ToRoute();
            var weight = int.Parse(char.ToString(charArray[2]));

            // If we already have the source in the list then add a new edge.
            if (adj.ContainsKey(source)) adj[source].Edges.Add(new Edge(destination, weight));

            // If we have not seen the source then create a new list and add the edge then
            // add the list to the dictionary.
            else adj.Add(source, new Node(source, new List<IEdge> { new Edge(destination, weight) }));
        }

        return adj;
    }
}