using System.Windows;
using ModernControls;

namespace Q_Determinant
{
    public partial class MainWindow:MWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            AddHandler(Workplace.AboutClickEvent, new RoutedEventHandler(ShowAbout));
        }

        public void ShowAbout(object sender, RoutedEventArgs e)
        {
            (new AboutWindow()).ShowDialog();
        }
    }
}
