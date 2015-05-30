using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace Core.Atoms
{
    public class Graph
    {
        public List<Block> Blocks { get; set; }
        public List<Link> Links { get; set; }

        
        public Graph()
        {
            Blocks = new List<Block>();
            Links = new List<Link>();
        }

        public Graph(List<Block> blocks, List<Link> links)
        {
            Blocks = blocks;
            Links = links;
        }

        public Graph(List<Block> blocks)
        {
            Blocks = blocks;
            Links = new List<Link>();
        }

        public Graph(List<Link> links)
        {
            Blocks = new List<Block>();
            Links = links;
        }

        public virtual ulong AddBlock(string content = null)
        {
            var newId = GetNewId();
            Blocks.Add(new Block {Content = String.IsNullOrEmpty(content) ? string.Empty : content, Id = newId});
            return newId;
        }

        public virtual void AddBlock(ref ulong id, Block block=null)
        {
            if (block == null || block.Id == 0)
            {
                var newId = GetNewId();
                Blocks.Add(new Block { Content = string.Empty, Id = newId });
                id = newId;
            }
            else
            {
                Blocks.ToList().Add(block);    
            }
        }

        public void RemoveBlock(ulong id)
        {
            Blocks.RemoveAll(x => x.Id == id);
        }
        public void RemoveBlock(IEnumerable<ulong> ids)
        {
            Blocks.RemoveAll(x => ids.Contains(x.Id));
        }

        public Dictionary<ulong, List<ulong>> GetMatrix()
        {
            return Blocks.ToDictionary(block => block.Id, block => Blocks.Select(inBlock => (ulong)(Links.Exists(x => x.From == block.Id && x.To == inBlock.Id) ? 1 : 0)).ToList());
        }

        public ulong GetMaxId()
        {
            return Blocks.Count > 0 ? Blocks.Max(x => x.Id) : 0;
        }

        public ulong GetNewId()
        {
            return GetMaxId() + 1;
        }
        
    }
}
