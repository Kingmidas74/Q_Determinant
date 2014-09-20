using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlowChart.AllConverters;

namespace FlowChart
{
    public class Chart : IFlowChart
    {
        private List<Block> _elements;
        private List<Link> _links;

        public Chart()
        {
            _elements=new List<Block>();
            _links = new List<Link>();
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

        public string GetAsText(ConverterTypes format)
        {
            var converter = ConvertersFactory.CreateConverter(format);
            return converter.GetAsString(_elements, _links);
        }

        public string GetMatrixToString()
        {
            var result = new StringBuilder();
           
            return result.ToString();
        }

        public void RemoveBlock(ulong id)
        {
            _elements.RemoveAll(x => x.Id == id);
        }

        public void RemoveLink(ulong from, ulong to)
        {
            _links.RemoveAll(x => x.From == from && x.To == to);
        }

        public void AddBlock(BlockTypes type, string content)
        {
            _elements.Add(new Block(){Content = content,Id=_elements.Max(x=>x.Id)+1,Type=type});
        }

        public void AddLink(ulong from, ulong to, BrunchTypes type)
        {
            _links.Add(new Link(){From = from, To = to, Brunch = type});
        }

        public void ChangeContentBlock(ulong id, string content)
        {
            _elements.First(e => e.Id == id).Content = content;
        }

        public void ChangeTypeBlock(ulong id, BlockTypes type)
        {
            _elements.First(e => e.Id == id).Type = type;
        }

        public void ChangePropertiesLink(ulong oldFrom, ulong oldTo, ulong newFrom, ulong newTo)
        {
            _links.First(x => x.From == oldFrom && x.To == oldTo).From = newFrom;
            _links.First(x => x.From == newFrom && x.To == oldTo).To = newTo;
            foreach (var l in _links.Where(l => l.From == oldFrom && l.To == oldTo))
            {
                l.From = newFrom;
                l.To = newTo;
            }
        }

        public bool CheckIntegrityScheme()
        {
            var adjacency_matrix = new List<List<bool>>();
            return false;
        }



    }
}
