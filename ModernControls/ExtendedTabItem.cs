using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls;assembly=ModernControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ExtendedTabControl/>
    ///
    /// </summary>
    public class ExtendedTabItem : TabItem
    {
        static ExtendedTabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTabItem), new FrameworkPropertyMetadata(typeof(ExtendedTabItem)));
        }

        public static readonly RoutedEvent CloseTabEvent = EventManager.RegisterRoutedEvent("CloseTab",
            RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (ExtendedTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(CloseTabEvent, value); }
            remove { RemoveHandler(CloseTabEvent, value); }
        }
        public override void OnApplyTemplate()
        {
            (base.GetTemplateChild("CloseTabButton") as Button).Click += new RoutedEventHandler(CloseTabButtonClick);
            base.OnApplyTemplate();
        }

        void CloseTabButtonClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(CloseTabEvent, this));
        }
    }
}
