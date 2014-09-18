using System.Collections;
using System.Collections.Generic;
using System.Management.Instrumentation;
using FlowChart.AllBlocks;
namespace FlowChart
{
    class BlocksFactory
    {
        public static AbstractBlock CreateBlock(BlockTypes type)
        {
            switch (type)
            {
                case BlockTypes.Condition:
                    return CreateCondtitionBLock();
                case BlockTypes.Process:
                    return CreateProcessBLock();
                case BlockTypes.Input:
                    return CreateProcessBLock();
                case BlockTypes.Output:
                    return CreateProcessBLock();
                case BlockTypes.Start:
                    return CreateStartBlock();
                case BlockTypes.End:
                    return CreateEndBlock();
                default:
                    return null;
            }
        }

        public static AbstractBlock CreateCondtitionBLock()
        {
            return new CondtitionBlock();
        }

        public static AbstractBlock CreateProcessBLock()
        {
            return new ProcessBlock();
        }

        public static AbstractBlock CreateStartBlock()
        {
            return new StartBlock();
        }
        public static AbstractBlock CreateEndBlock()
        {
            return new EndBlock();
        }
    }
}
