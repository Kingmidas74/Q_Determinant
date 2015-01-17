using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Atoms
{
    [Serializable]
    public class Graph
    {
        public List<Block> Vertices;
        public List<Link> Edges;

        public Graph()
        {
            this.Vertices = new List<Block>();
            this.Edges = new List<Link>();
        }

        public Graph(List<Block> vertices, List<Link> edges)
        {
            this.Vertices = vertices;
            this.Edges = edges;
        }

        public bool[,] GetAdjancencyMatrix()
        {
            return null;
        }

        public ulong GetMaxId()
        {
            ulong result = 0;
            foreach (var vertex in Vertices)
            {
                if (vertex.Id > result)
                {
                    result = vertex.Id;
                }
            }
            return result;
        }

        public ulong GetMaxLevel()
        {
            ulong result = 0;
            foreach (var vertex in Vertices)
            {
                if (vertex.Level > result)
                {
                    result = vertex.Level;
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
                var operationsInLevel = (ulong)Vertices.LongCount(x => x.Level == i);
                if (operationsInLevel > result)
                {
                    result = operationsInLevel;
                }
            }
            return result;
        }
    }
}
