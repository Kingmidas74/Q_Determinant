using System.Collections.Generic;

namespace ImplementationPlan
{
    public interface IGraph
    {
        void AddVertex(ulong level, BaseOperations operation);
        void AddEdge(ulong from, ulong to);
        ulong GetMaxLevel();
        ulong GetMaxOperationsInLevel();
        List<EdgeGraph> GetEdgeGraphs();
        List<VertexGraph> GetVertexGraphs();
    }
}
