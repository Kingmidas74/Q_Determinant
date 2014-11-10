using System.Collections.Generic;
using System.Linq;
using Core;

namespace ImplementationPlan
{
    public class Plan
    {
        private Graph _graph;

        public ulong CountProcessors { get; private set; }
        public ulong CountTacts { get; private set; }

        public Plan(List<Operation> operations, QDet qDet)
        {
            var transformer = new Transformer(qDet, operations);
            _graph = transformer.Graph;
            CountProcessors = _graph.GetMaxOperationsInLevel();
            CountTacts = _graph.GetMaxLevel();
        }

        public Plan(List<Operation> operations, QDet qDet, ulong countProcessors)
        {
            var transformer = new Transformer(qDet, operations);
            _graph = transformer.Graph;
            OptimizeForMaxEffective(countProcessors);
        }

        public void OptimizeForMaxEffective(ulong countProcessors)
        {
            var blocks = (GetVertexGraph()).Where(x => x.Level > 0).OrderBy(x=>x.Level).ThenBy(x=>x.Id);
            var links = GetEdgesGraph();
            var nBlocks = new List<Block>();
            ulong currentNewLevel = 1;
            var tempBlock = blocks.First();
            nBlocks.Add(tempBlock);
            blocks = blocks.Where(x => x.Id != tempBlock.Id).OrderBy(x => x.Level).ThenBy(x => x.Id);
            foreach (var block in blocks)
            {
                var isNewLevel = (ulong)nBlocks.LongCount(x => x.Level == currentNewLevel) < countProcessors == false;
                foreach (var link in links)
                {
                    if (link.To == block.Id)
                    {
                        if (blocks.FirstOrDefault(x => x.Id == link.From) != null)
                        {
                            tempBlock = blocks.FirstOrDefault(y => y.Id == link.From);
                            if (tempBlock != null && tempBlock.Level == currentNewLevel)
                            {
                                isNewLevel = true;
                                break;
                            }
                        }
                        
                    }
                }
                if (isNewLevel == true)
                {
                    currentNewLevel++;
                }
                block.Level = currentNewLevel;
                nBlocks.Add(block);
            }
            var ZeroBlocks = (GetVertexGraph()).Where(x => x.Level == 0).OrderBy(x => x.Id).ToList();
            ZeroBlocks.AddRange(nBlocks);
            _graph = new Graph(ZeroBlocks,links);
            CountProcessors = countProcessors;
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
