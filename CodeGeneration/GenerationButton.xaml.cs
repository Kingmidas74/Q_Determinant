using System.Windows;
using System.Windows.Controls;
using BasicComponentsPack;

namespace CodeGeneration
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class GenerationButton : UserControl
    {
        public GenerationButton()
        {
            InitializeComponent();
        }

        public SolutionExplorer SE { get; set; }

        private void GC_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var settingsWindow = new CodeGenerationSettings();
            if (settingsWindow.ShowDialog() == true)
            {
                Generator.GenerateByProject(SE.CurrentProjectPath);   
            }
        }
    }
}
