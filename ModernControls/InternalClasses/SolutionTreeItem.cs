﻿using System.Collections.Generic;

namespace ModernControls.InternalClasses
{
    internal class SolutionTreeItem
    {
        public string IconPath { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public string ProjectPath { get; set; }
        public List<SolutionTreeItem> Items { get; set; }

        public SolutionItemTypes Type { get; set; }

        public SolutionTreeItem(SolutionItemTypes ItemType)
        {
            Items=new List<SolutionTreeItem>();
            Type = ItemType;
        }
    }
}
