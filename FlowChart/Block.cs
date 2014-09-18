using System;

namespace FlowChart
{
    [Serializable()]
    public class Block
    {
        public BlockTypes Type;
        public string Content;
        public ulong Id;
    }
}
