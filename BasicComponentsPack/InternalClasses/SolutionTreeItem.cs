using System.Collections.Generic;

namespace BasicComponentsPack.InternalClasses
{
    public class SolutionTreeItem
    {
        public string IconPath { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public List<SolutionTreeItem> Items { get; set; }
        public SolutionTreeItem()
        {
            Items = new List<SolutionTreeItem>();
            Enabled = true;

        }

        public bool Enabled { get; set; }
    }
}
