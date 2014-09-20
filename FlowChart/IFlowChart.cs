using FlowChart.AllConverters;

namespace FlowChart
{
    public interface IFlowChart
    {
        void ChartFromFile(ConverterTypes format, string filepath);
        void ChartFromString(ConverterTypes format, string originalString);
        int GetCountBlocks();
        int GetCountLinks();
        void SaveToFile(ConverterTypes format, string filePath);
        string GetAsText(ConverterTypes format);
        string GetMatrixToString();
        void RemoveBlock(ulong id);
        void RemoveLink(ulong from, ulong to);
        void AddBlock(BlockTypes type, string content);
        void AddLink(ulong from, ulong to, BrunchTypes type);
        void ChangeContentBlock(ulong id, string content);
        void ChangeTypeBlock(ulong id, BlockTypes type);
        void ChangePropertiesLink(ulong oldFrom, ulong oldTo, ulong newFrom, ulong newTo);
        bool CheckIntegrityScheme();
    }
}