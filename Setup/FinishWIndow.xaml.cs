using System.Diagnostics;
using System.Windows;
using Setup.InternalClasses;
using Setup.Properties;

namespace Setup
{
    /// <summary>
    /// Interaction logic for FinishWIndow.xaml
    /// </summary>
    public partial class FinishWindow
    {
        public FinishWindow()
        {
            InitializeComponent();
        }

        private void ExitOut(object sender, RoutedEventArgs e)
        {
            if (StartUpProgramm.IsChecked == true)
            {
                Process.Start(ProjectInstall.GetPathToQStudio());
            }
            if (VisitSite.IsChecked == true)
            {
                System.Diagnostics.Process.Start(Settings.Default.Site);
            }
            App.Current.Shutdown();
        }
    }
}
