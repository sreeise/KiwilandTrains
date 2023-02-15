namespace Kiwiland.RouteComputation.Generic;

public interface INodeMap<TKey, TNode>
{
    void AddEdge(TKey key, IEdge edge);

    IEnumerable<IEdge>? GetEdges(TKey key);

    void AddNode(TKey key, TNode node);

    TNode? GetNode(TKey key);
}