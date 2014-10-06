using System.Collections.Generic;
using Core;

namespace ImplementationPlan
{
    public class Plan
    {
        private readonly Graph _graph;

        public ulong CountProcessors { get; private set; }
        public ulong CountTacts { get; private set; }

        public Plan(List<Operation> operations, QDet qDet)
        {
            var transformer = new Transformer(qDet, operations);
            _graph = transformer.Graph;
            CountProcessors = _graph.GetMaxOperationsInLevel();
            CountTacts = _graph.GetMaxLevel();
        }

        public List<Block> GetVertexGraph()
        {
            return _graph.GetVertexGraphs();
        }
        public List<Link> GetEdgesGraph()
        {
            return _graph.GetEdgeGraphs();
        } 
        
    }
}
