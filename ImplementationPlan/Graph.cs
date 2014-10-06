using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace ImplementationPlan
{
    internal class Graph : IGraph
    {
        private readonly List<Block> _vertex;
        private readonly List<Link> _edge;

        public Graph(List<Block> vertex, List<Link> edge)
        {
            _vertex = vertex;
            _edge = edge;
        }

        public Graph()
        {
            _vertex=new List<Block>();
            _edge = new List<Link>();
        }


        public void AddVertex(ulong level, string content)
        {
            ulong id = 1;
            if (_vertex.Count > 0)
            {
                id = _vertex.Max(x => x.Id) + 1;
            }
            if (!String.IsNullOrEmpty(content))
            {
                _vertex.Add(new Block {Id = id, Level = level, Content = content});
            }
        }

        public void AddEdge(ulong from, ulong to, LinkTypes type)
        {
            _edge.Add(new Link { From = from, To = to, Type = type});
        }


        public ulong GetMaxLevel()
        {
            ulong result = 0;
            foreach (var block in _vertex)
            {
                if (block.Level > result)
                {
                    result = block.Level;
                }
            }
            return result;
        }


        public ulong GetMaxOperationsInLevel()
        {
            ulong result = 0;
            var maxLevel = GetMaxLevel();
            for (ulong i = 1; i <= maxLevel; i++)
            {
                var operationsInLevel = (ulong)(_vertex.Count(x => x.Level == i));
                if (operationsInLevel > result)
                {
                    result = operationsInLevel;
                }

            }
            return result;
        }


        public List<Link> GetEdgeGraphs()
        {
            return _edge;
        }

        public List<Block> GetVertexGraphs()
        {
            return _vertex;
        }
    }
}
