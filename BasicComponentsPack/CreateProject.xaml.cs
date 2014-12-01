using System.Windows;
using DefaultControlsPack;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for CreateProject.xaml
    /// </summary>
    public partial class CreateProject : ModernWindow
    {
        public CreateProject()
        {
            InitializeComponent();
        }

        public string ProjectTitle
        {
            get { return TitleProject.Text; }
            set { TitleProject.Text = value; }
        }

        public int ProjectType { get { return TypeProject.SelectedIndex; } set { TypeProject.SelectedIndex = value; } }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
