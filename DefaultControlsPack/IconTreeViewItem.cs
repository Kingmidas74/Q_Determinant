using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DefaultControlsPack
{
    public class IconTreeViewItem:TreeViewItem
    {
        #region Icon
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(IconTreeViewItem),
                new FrameworkPropertyMetadata());

        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        static IconTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconTreeViewItem), new FrameworkPropertyMetadata(typeof(IconTreeViewItem)));
        }
    }
}
