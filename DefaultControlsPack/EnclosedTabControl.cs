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

        #region CloseTabEvent
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EnclosedTabControl));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }
        #endregion

        public EnclosedTabControl()
        {
            AddHandler(EnclosedTabItem.CloseTabEvent, new RoutedEventHandler(CloseWorkTab));
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
            Items.Remove((sender as Button).Tag as EnclosedTabItem);
        }

        private void CloseWorkTab(object source, RoutedEventArgs args)
        {
            //RemoveItem(args.OriginalSource as EnclosedTabItem);
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
        
    }
}
