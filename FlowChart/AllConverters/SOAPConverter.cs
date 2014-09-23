using System.Collections.Generic;

namespace FlowChart.AllConverters
{
    public class SOAPConverter : AbstractConverter
    {
        public override void SaveToFile(List<Block> elements, List<Link> links, string filePath)
        {
            throw new System.NotImplementedException();
        }

        protected override bool Validation(string data)
        {
            throw new System.NotImplementedException();
        }

        protected override void Parse(string data)
        {
            throw new System.NotImplementedException();
        }

        public override string GetAsString(List<Block> elements, List<Link> links)
        {
            return null;
        }
    }
}
