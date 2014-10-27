using System.Windows;
using System.Windows.Controls;

namespace Q_Determinant
{
    public class TabContentWithCanvas:TabContent
    {
        public TextBlock Content { get; set; }

        public TabContentWithCanvas()
        {
            Content = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Type = 1;
        }
    }
}
