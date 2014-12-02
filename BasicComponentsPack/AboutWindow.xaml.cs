using System.Windows.Controls;
using DefaultControlsPack;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : ModernWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        public object Image {
            set { InfoPanel.Image = value; }
        }

        public string Text
        {
            get { return (FindResource("InfoText") as TextBlock).Text; }
            set { (FindResource("InfoText") as TextBlock).Text = value; }
        }

        private void CloseAbout(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }


    }
}
