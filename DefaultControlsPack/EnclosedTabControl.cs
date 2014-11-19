using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DefaultControlsPack.Enums;

namespace DefaultControlsPack
{
    public class EnclosedTabControl : TabControl
    {

        #region VerticalContentPosition
        public static readonly DependencyProperty VerticalContentPositionProperty =
            DependencyProperty.Register("VerticalContentPosition", typeof(VerticalContentPosition), typeof(TabControl),
                new FrameworkPropertyMetadata());

        public VerticalContentPosition VerticalContentPosition
        {
            get { return (VerticalContentPosition)GetValue(VerticalContentPositionProperty); }
            set { SetValue(VerticalContentPositionProperty, value); }
        }
        #endregion

        public EnclosedTabControl()
        {
            AddHandler(EnclosedTabItem.CloseTabEvent, new RoutedEventHandler(CloseTab));
        }

        public override void OnApplyTemplate()
        {
            var closeButton = (GetTemplateChild("CloseTabButton") as Button);
            if (closeButton != null)
            {
                closeButton.Click += ClosePluginPanel;
            }
            base.OnApplyTemplate();
        }

        private void ClosePluginPanel(object sender, RoutedEventArgs e)
        {
            RemoveItem((sender as Button).Tag as EnclosedTabItem);
        }

        private void CloseTab(object source, RoutedEventArgs args)
        {
            RemoveItem(args.OriginalSource as EnclosedTabItem);
        }

        private void RemoveItem(EnclosedTabItem item)
        {
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }
}
