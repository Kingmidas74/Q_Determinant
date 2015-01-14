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
