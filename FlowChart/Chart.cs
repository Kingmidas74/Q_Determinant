using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FlowChart.AllBlocks;

namespace FlowChart
{
    public class Chart
    {
        private List<AbstractBlock> _flowchart;

        private AbstractBlock FindBlocks(ulong id)
        {
            return _flowchart.SingleOrDefault(x => x.Id == id);
        }
        private List<AbstractBlock> FindBlocks(IEnumerable<ulong> id)
        {
            return _flowchart.Where(block => id.Contains(block.Id)).ToList();
        }

        private bool IsLooped(AbstractBlock targetBlock, AbstractBlock currentBlock)
        {
            var result = false;
            switch (currentBlock.Type)
            {
                case BlockTypes.Process:
                    result = ((ProcessBlock) currentBlock).OutputLink == targetBlock || IsLooped(targetBlock, ((ProcessBlock) currentBlock).OutputLink);
                    break;
                case BlockTypes.Condition:
                    result = IsLooped(targetBlock, ((CondtitionBlock) currentBlock).TruePath) ||
                             IsLooped(targetBlock, ((CondtitionBlock) currentBlock).FalsePath);
                    break;
            }
            return result;
        }

        public Chart()
        {
            var newBlock = BlocksFactory.CreateStartBlock();
            newBlock.Id = 0;
            newBlock.Content="Start";
            
            var newBlockTwo = BlocksFactory.CreateBlock(BlockTypes.)


            _flowchart
        }


    }
}
