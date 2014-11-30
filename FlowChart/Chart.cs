using System.Collections.Generic;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;

namespace FlowChart
{
    public class Chart:IFlowChart
    {
        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private Graph _flowChart;

        public Chart(List<Block> vertices, List<Link> edges)
        {
            _flowChart = new Graph(vertices, edges);
        }

        public Graph GetFlowChart()
        {
            return _flowChart;
        }

        
    }
}
