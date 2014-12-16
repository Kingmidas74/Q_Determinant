using System;
using System.Collections.Generic;
using Core.Atoms;

namespace CodeGeneration.InternalClasses
{
    [Serializable]
    public class CGGraph
    {
        public List<CGBlock> Vertices;
        public List<Link> Edges;

        public CGGraph()
        {
            this.Vertices = new List<CGBlock>();
            this.Edges = new List<Link>();
        }

        public CGGraph(List<CGBlock> vertices, List<Link> edges)
        {
            this.Vertices = vertices;
            this.Edges = edges;
        }
    }
}
