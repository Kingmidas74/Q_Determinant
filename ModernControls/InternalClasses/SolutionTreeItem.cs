using System.Collections.Generic;

namespace ModernControls.InternalClasses
{
    internal class SolutionTreeItem
    {
        public string IconPath { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public List<SolutionTreeItem> Items { get; set; }

        public SolutionTreeItem(SolutionItemTypes ItemType)
        {
            Items=new List<SolutionTreeItem>();
            switch (ItemType)
            {
                case SolutionItemTypes.Project:
                    break;
                default:
                    break;
            }
        }
    }
}
