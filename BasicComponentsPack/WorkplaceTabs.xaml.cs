using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DefaultControlsPack;
using VisualCore;
using System.Collections.Generic;

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
            var currentTab = (e.OriginalSource as EnclosedTabItem);
            if (currentTab.Content is ISaveable)
            {
                (currentTab.Content as ISaveable).Save();
            }
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

        public void SaveAllListener(object sender, RoutedEventArgs e)
        {
            foreach (var content in (from object item in WorkplaceTabControl.Items select (item as EnclosedTabItem).Content).OfType<ISaveable>())
            {
                content.Save();
            }
        }

        public void CloseSolutionListener(object sender, RoutedEventArgs e)
        {
            SaveAllListener(sender, e);
            WorkplaceTabControl.Items.Clear();
        }
    }
}
