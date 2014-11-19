using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DefaultControlsPack;

namespace BasicComponentsPack
{
    public partial class WorkplaceTabs : UserControl
    {
        public WorkplaceTabs()
        {
            InitializeComponent();
            AddHandler(EnclosedTabControl.CloseTabEvent, new RoutedEventHandler(CloseTab));
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            var currentItem = (e.OriginalSource as EnclosedTabControl).SelectedItem as EnclosedTabItem;
            WorkplaceTabControl.Items.Remove(currentItem);
        }

        public void AddTab(string FilePath)
        {
            var item = new EnclosedTabItem()
            {
                Header = "NEW",
                Content =
                    new TextBlock()
                    {
                        Text = "TESTTESTTEST",
                        Foreground = new SolidColorBrush(Colors.Black),
                        FontSize = 14,
                        Width = 200,
                        Background = new SolidColorBrush(Colors.Red)
                    }
            };
            WorkplaceTabControl.Items.Add(item);
        }
        
    }
}
