using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DefaultControlsPack;
using VisualCore;
using System.Collections.Generic;

namespace BasicComponentsPack
{
    public partial class WorkplaceTabs : UserControl, IWorkPlaceTabs
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

        public void SelectedFileListener(object sender, RoutedEventArgs e)
        {
            AddTab(e.OriginalSource.ToString());
        }

        public void AddTab(string filePath)
        {
            var item = new EnclosedTabItem()
            {
                Header = filePath,
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
