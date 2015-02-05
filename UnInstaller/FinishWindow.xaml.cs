using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;
using UnInstaller.Properties;

namespace UnInstaller
{
    /// <summary>
    /// Interaction logic for FinishWindow.xaml
    /// </summary>
    public partial class FinishWindow
    {
        public FinishWindow()
        {
            InitializeComponent();
        }

        private void ExitOut(object sender, RoutedEventArgs e)
        {
           if (VisitSite.IsChecked == true)
            {
                System.Diagnostics.Process.Start(Settings.Default.Site);
            }
            App.Current.Shutdown();
        }
    }
}
