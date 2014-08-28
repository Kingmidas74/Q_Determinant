namespace FlowChart
{
    public interface IFlowChart
    {
        void AddBlock(BlockTypes type, string context, int[] inputLink);
        void RemoveBlock(int id);
        int GetTotalBlocksCount();
        void ChangeContentBlock(int id, string content);
        void SetJsonFile(string filePath);
        void SetJson(string json);
        void SaveToFile(string filePath);
    }
}
