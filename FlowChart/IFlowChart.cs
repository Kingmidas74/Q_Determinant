namespace FlowChart
{
    interface IFlowChart
    {

        void AddBlock(string context, int[] inputLink);
        void RemoveBlock(int id);
        dynamic GetObject();
        int GetBlocksCount();


    }
}
