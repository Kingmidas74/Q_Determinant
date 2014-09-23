using System.Collections.Generic;
using System.IO;

namespace FlowChart.AllConverters
{
    public abstract class AbstractConverter:IConverter
    {
        protected List<Block> Blocks;
        protected List<Link> Links;

        protected abstract bool Validation(string data);
        protected abstract void Parse(string data);


        public void ParseDocument(string filePath)
        {
            Parse(File.ReadAllText(filePath));
        }

        public void ParseString(string originalString)
        {
            Parse(originalString);   
        }
        public abstract void SaveToFile(List<Block> elements, List<Link> links, string filePath);
        public abstract string GetAsString(List<Block> elements, List<Link> links);


        public List<Block> GetBlocks()
        {
            return Blocks;
        }

        public List<Link> GetLinks()
        {
            return Links;
        }

        protected AbstractConverter()
        {
            Blocks = new List<Block>();
            Links = new List<Link>();
        }


        
    }
}
