using System.Collections.Generic;
using System.Linq;
using Core;

namespace ImplementationPlan
{
    internal class Graph : IGraph
    {
        private readonly List<VertexGraph> _vertex;
        private readonly List<Link> _edge;

        private ulong Max(ulong first, ulong second)
        {
            return first >= second ? first : second;
        }

        public Graph(List<VertexGraph> vertex, List<Link> edge)
        {
            _vertex = vertex;
            _edge = edge;
        }

        public Graph()
        {
            _vertex=new List<VertexGraph>();
            _edge = new List<Link>();
        }


        public void AddVertex(ulong level, string content/*, BaseOperations operation*/)
        {
            _vertex.Add(new VertexGraph {Id = _vertex.Max(x=>x.Id)+1, Level = level, /*Operation = operation*/ Content = content});
        }

        public void AddEdge(ulong from, ulong to, LinkTypes type)
        {
            _edge.Add(new Link { From = from, To = to, Type = type});
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


        public List<Link> GetEdgeGraphs()
        {
            return _edge;
        }

        public List<VertexGraph> GetVertexGraphs()
        {
            return _vertex;
        }
    }
}
