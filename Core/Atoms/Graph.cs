using System;
using System.Collections.Generic;

namespace Core.Atoms
{
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
    }
}
