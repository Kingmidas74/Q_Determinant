using System.Windows;
using System.Windows.Controls;

namespace DefaultControlsPack
{
    public class SearchTreeView:TreeView
    {
        static SearchTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SearchTreeView), new FrameworkPropertyMetadata(typeof(SearchTreeView)));
        }
    }
}
