using System.Collections.Generic;
using System.IO;
using Core;

namespace Converters.ImplementationPlanConverters
{
    public abstract class AbstractConverterImplementationPlan : IImplementationPlanConverter
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

        public abstract void SaveToFile(string filePath);
        public abstract string GetAsString();


        public List<Block> GetBlocks()
        {
            return Blocks;
        }

        public List<Link> GetLinks()
        {
            return Links;
        }

        public void SetBlocks(List<Block> blocks)
        {
            Blocks = blocks;
        }

        public void SetLinks(List<Link> links)
        {
            Links = links;
        }

        protected AbstractConverterImplementationPlan()
        {
            Blocks = new List<Block>();
            Links = new List<Link>();
        }
    }
}
