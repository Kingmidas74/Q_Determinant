using System.Collections.Generic;

namespace FlowChart.AllConverters
{
    interface IConverter
    {
        void ParseDocument(string filePath);
        void ParseString(string originalString);
        void SaveToFile(List<Block> elements, List<Link> links, string filePath);
        List<Block> GetBlocks();
        List<Link> GetLinks();
        string GetAsString(List<Block> elements, List<Link> links);
    }
}
