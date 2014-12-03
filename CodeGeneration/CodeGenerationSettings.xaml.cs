using System.Windows;
using DefaultControlsPack;

namespace CodeGeneration
{
    /// <summary>
    /// Interaction logic for CodeGenerationSettings.xaml
    /// </summary>
    public partial class CodeGenerationSettings : ModernWindow
    {
        public CodeGenerationSettings()
        {
            InitializeComponent();
        }

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
