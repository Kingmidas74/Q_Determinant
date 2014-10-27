using System.Windows;
using System.Windows.Controls;

namespace Q_Determinant
{
    public class TabContentWithText : TabContent
    {
        
        public TextBox Content { get; set; }

        public string Text { get; set; }

        public TabContentWithText()
        {
            Content = new TextBox()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Type = 0;
        }
    }
}
