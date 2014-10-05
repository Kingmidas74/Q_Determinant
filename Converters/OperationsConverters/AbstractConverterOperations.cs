using System.Collections.Generic;
using System.IO;
using Core;

namespace Converters.OperationsConverters
{
    public abstract class AbstractConverterOperations : IOperationsConverter
    {
        protected List<Operation> Blocks;
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


        public List<Operation> GetBlocks()
        {
            return Blocks;
        }

        public List<Link> GetLinks()
        {
            return Links;
        }

        public void SetBlocks(List<Operation> blocks)
        {
            Blocks = blocks;
        }

        public void SetLinks(List<Link> links)
        {
            Links = links;
        }

        protected AbstractConverterOperations()
        {
            Blocks = new List<Operation>();
            Links = new List<Link>();
        }
    }
}
