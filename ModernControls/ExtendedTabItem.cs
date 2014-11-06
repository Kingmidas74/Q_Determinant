using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    public class ExtendedTabItem : TabItem
    {
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtendedTabItem));
        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }

        static ExtendedTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTabItem), new FrameworkPropertyMetadata(typeof(ExtendedTabItem)));
        }

        public void RefreshStatus(bool status)
        {
            (GetTemplateChild("ChangeSymbol") as TextBlock).Text = status ? "*" : "";
        }
        
        public override void OnApplyTemplate()
        {
            (GetTemplateChild("CloseTabButton") as Button).Click += CloseTabButtonClick;
            base.OnApplyTemplate();
        }

        void CloseTabButtonClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
