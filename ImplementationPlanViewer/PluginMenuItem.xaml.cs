using System.Windows;
using BasicComponentsPack;
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for PluginMenuItem.xaml
    /// </summary>
    public partial class PluginMenuItem
    {
        public PluginMenuItem()
        {
            InitializeComponent();
        }

        private void ShowAbout(object sender, RoutedEventArgs e)
        {
            var aboutInfo = new AboutWindow();
            aboutInfo.Text = "123";
            aboutInfo.ShowDialog();
        }

        private void ExtractToProject(object sender, RoutedEventArgs e)
        {
            Methods.CreateProject();
        }
    }
}
