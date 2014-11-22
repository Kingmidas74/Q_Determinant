using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicComponentsPack.InternalClasses
{
    internal class SolutionTreeItem
    {
        public string IconPath { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public List<SolutionTreeItem> Items { get; set; }
        public SolutionTreeItem()
        {
            Items = new List<SolutionTreeItem>();
            
        }
    }
}
