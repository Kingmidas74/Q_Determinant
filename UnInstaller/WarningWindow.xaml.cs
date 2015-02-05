using System.Windows;
using System.Windows.Media.Animation;
using UnInstaller.InternalClasses;

namespace UnInstaller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class WarningWindow
    {
        public WarningWindow()
        {
            InitializeComponent();
        }

        private void NextWindow(object sender, RoutedEventArgs e)
        {
            (FindResource("OnUnChecking") as Storyboard).Begin();
            Helpers.GetNextWindow(this);
        }
    }
}
