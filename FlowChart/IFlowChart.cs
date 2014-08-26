namespace FlowChart
{
    interface IFlowChart
    {
        void AddBlock(BlockTypes type, string context, int[] inputLink);
        void RemoveBlock(int id);
        int GetBlocksCount(bool withSystemBlocks=false);
        void ChangeContentBlock(int id, string content);
        void SetJsonFile(string filePath);
        void SetJson(string json);
        void SaveToFile(string filePath);
    }
}
