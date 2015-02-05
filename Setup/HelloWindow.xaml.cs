using System.Windows;
using Setup.InternalClasses;

namespace Setup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelloWindow
    {
        public HelloWindow()
        {
            InitializeComponent();
        }
        
        private void NextWindow(object sender, RoutedEventArgs e)
        {
            Helpers.GetNextWindow(this);
        }
    }
}
