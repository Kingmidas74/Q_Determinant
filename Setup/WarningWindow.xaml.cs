using System.Windows;
using System.Windows.Media.Animation;
using Setup.InternalClasses;

namespace Setup
{
    /// <summary>
    /// Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow
    {
        public WarningWindow()
        {
            InitializeComponent();
        }

        private void NextWindow(object sender, RoutedEventArgs e)
        {
            (this.FindResource("OnUnChecking") as Storyboard).Begin();
            Helpers.GetNextWindow(this);
        }

        private void ExitOut(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PreviousWindow(object sender, RoutedEventArgs e)
        {
            Helpers.GetPreviousWindow(this);
        }
    }
}
