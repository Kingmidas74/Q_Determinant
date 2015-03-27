using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Atoms;
using Core.Interfaces;

namespace QDeterminant
{
    public class CycleHeadler
    {
        private List<ulong> viewedblocklist;

        public bool IsCycled(ulong id, Graph flowChart, Link link)
        {
            var block = new Block();
            viewedblocklist = new List<ulong>();

            block = flowChart.Vertices.FirstOrDefault(y => y.Id == link.To);

            if (block.Id == id)
            {
                return true;
            }
            else
            {
                if (!viewedblocklist.Contains(block.Id))
                {
                    viewedblocklist.Add(block.Id);
                    if (block.Type == BlockTypes.Condition)
                    {

                        if (IsCycled(id, flowChart,
                            flowChart.Edges.FirstOrDefault(y => (y.From == block.Id) && (y.Type == LinkTypes.FalseBrunch))))
                        {
                            return true;
                        }
                        if (IsCycled(id, flowChart,
                            flowChart.Edges.FirstOrDefault(y => (y.From == block.Id) && (y.Type == LinkTypes.TrueBrunch))))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (block.Type != BlockTypes.End)
                        {
                            if (IsCycled(id, flowChart, flowChart.Edges.FirstOrDefault(y => y.From == block.Id)))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }
    }
}
