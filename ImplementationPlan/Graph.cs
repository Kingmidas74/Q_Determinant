using System.Collections.Generic;
using System.Linq;

namespace ImplementationPlan
{
    public class Graph:IGraph
    {
        private readonly List<VertexGraph> _vertex;
        private readonly List<EdgeGraph> _edge;

        private ulong Max(ulong first, ulong second)
        {
            return first >= second ? first : second;
        }

        public Graph(List<VertexGraph> vertex, List<EdgeGraph> edge)
        {
            _vertex = vertex;
            _edge = edge;
        }


        public void AddVertex(ulong level, BaseOperations operation)
        {
            _vertex.Add(new VertexGraph {Id = _vertex.Max(x=>x.Id)+1, Level = level, Operation = operation});
        }

        public void AddEdge(ulong from, ulong to)
        {
            _edge.Add(new EdgeGraph {From = from, To = to});
        }


        public ulong GetMaxLevel()
        {
            return _vertex.Max(x => x.Level);
        }


        public ulong GetMaxOperationsInLevel()
        {
            ulong result = 0;
            var maxLevel = GetMaxLevel();
            for (ulong i = 1; i <= maxLevel; i++)
            {
                result = Max((ulong)(_vertex.FindAll(x => x.Level == i).Count),i);
             
            }
            return result;
        }


        public List<EdgeGraph> GetEdgeGraphs()
        {
            return _edge;
        }

        public List<VertexGraph> GetVertexGraphs()
        {
            return _vertex;
        }
    }
}
