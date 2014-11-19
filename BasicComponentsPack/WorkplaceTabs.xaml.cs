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
    }
}
