using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowChart.AllConverters;

namespace FlowChart
{
    public class Chart
    {
        private List<Block> _elements;
        private List<Link> _links;
        private List<List<bool>> adjacency_matrix;

        public Chart()
        {
            _elements=new List<Block>();
            _links = new List<Link>();
            adjacency_matrix = new List<List<bool>>();
        }
        public void ChartFromFile(ConverterTypes format, string filepath)
        {
            var converter = ConvertersFactory.CreateConverter(format);
            converter.ParseDocument(filepath);
            _elements = converter.GetBlocks();
            _links = converter.GetLinks();
        }

        public void ChartFromString(ConverterTypes format, string originalString)
        {
            var converter = ConvertersFactory.CreateConverter(format);
            converter.ParseString(originalString);
            _elements = converter.GetBlocks().OrderBy(x=>x.Id).ToList();
            _links = converter.GetLinks().OrderBy(x=>x.From).ToList();
        }

        public int GetCountBlocks()
        {
            return _elements.Count;
        }

        public int GetCountLinks()
        {
            return _links.Count;
        }

        public void SaveToFile(ConverterTypes format, string filePath)
        {
            var converter = ConvertersFactory.CreateConverter(format);
            converter.SaveToFile(_elements,_links,filePath);
        }

        public string GetMatrixToString()
        {
            var result = new StringBuilder();
           
            return result.ToString();
        }



    }
}
