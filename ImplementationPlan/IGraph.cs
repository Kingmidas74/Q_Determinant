using System.Collections.Generic;
using Core;

namespace ImplementationPlan
{
    internal interface IGraph
    {
        void AddVertex(ulong level, string content);
        void AddEdge(ulong from, ulong to, LinkTypes type);
        ulong GetMaxLevel();
        ulong GetMaxOperationsInLevel();
        List<Link> GetEdgeGraphs();
        List<Block> GetVertexGraphs();
    }
}
