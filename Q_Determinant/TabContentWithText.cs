using System.Windows;
using System.Windows.Controls;

namespace Q_Determinant
{
    public class TabContentWithText : TabContent
    {
        
        public TextBlock Content { get; set; }

        public string Text { get; set; }

        public TabContentWithText()
        {
            Content = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Type = 0;
        }
    }
}
