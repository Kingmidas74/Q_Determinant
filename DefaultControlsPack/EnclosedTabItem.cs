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
            base.OnApplyTemplate();
        }

        void CloseTabButtonClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
