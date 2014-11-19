using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DefaultControlsPack
{
    public class EnclosedTabItem : TabItem
    {
        #region CloseTabEvent
        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (EnclosedTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }
        #endregion

        #region MiddleClickEvent
        public static readonly RoutedEvent MiddleClickEvent = EventManager.RegisterRoutedEvent("MiddleClick",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(EnclosedTabItem));

        public event RoutedEventHandler MiddleClick
        {
            add { AddHandler(MiddleClickEvent, value); }
            remove { RemoveHandler(MiddleClickEvent, value); }
        }
        #endregion
        
        static EnclosedTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnclosedTabItem), new FrameworkPropertyMetadata(typeof(EnclosedTabItem)));
        }

        public override void OnApplyTemplate()
        {
            var closeButton = (GetTemplateChild("CloseTabButton") as Button);
            if (closeButton != null)
            {
                closeButton.Click += CloseTabButtonClick;
            }
            this.MouseDown += TabMiddleClick;
            base.OnApplyTemplate();
        }

        void TabMiddleClick(object sender, MouseButtonEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(MiddleClickEvent, this));
        }

        void CloseTabButtonClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
