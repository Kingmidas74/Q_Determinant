namespace FlowChart
{
    public interface IFlowChart
    {
        void AddBlock(BlockTypes type, int previousBlocks = 0, string content = null);
        void RemoveBlock(int id);
        int GetBlocksCount();
        void ChangeContentBlock(int id, string content);
        void SetJsonFile(string filePath);
        void SetJson(string json);
        void SaveToFile(string filePath);
    }
}
