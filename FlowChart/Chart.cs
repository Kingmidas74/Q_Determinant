using System.Collections.Generic;
using System.Linq;
using Core;

namespace FlowChart
{
    public class Chart
    {
        private List<Block> _elements;
        private List<Link> _links;
        private List<List<bool>> adjacency_matrix;


        private ulong GetNewId()
        {
            return _elements.Max(x => x.Id)+1;
        }
        public List<Block> GetBlocks()
        {
            return _elements;
        }

        public List<Link> GetLinks()
        {
            return _links;
        }

        public Chart(List<Block> blocks, List<Link> links)
        {
            _elements = blocks;
            _links = links;
        }

        public Chart()
        {
            _elements = new List<Block>();
            _links = new List<Link>();
        }
       
        public int GetCountBlocks()
        {
            return _elements.Count;
        }

        public int GetCountLinks()
        {
            return _links.Count;
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
            _elements.Add(new Block() { Content = content, Id = _elements.Max(x => x.Id) + 1, Type = type });
        }

        public void AddLink(ulong from, ulong to, LinkTypes type)
        {
            _links.Add(new Link() { From = from, To = to, Type = type });
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
