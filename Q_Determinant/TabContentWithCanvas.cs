using System.Windows;
using System.Windows.Controls;

namespace Q_Determinant
{
    public class TabContentWithCanvas:TabContent
    {
        public StackPanel Content { get; set; }

        public TabContentWithCanvas()
        {
            Content = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
        }
    }
}
