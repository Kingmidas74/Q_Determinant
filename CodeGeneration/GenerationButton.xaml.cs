using System;
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

        private SolutionExplorer _se;
        public SolutionExplorer SE
        {
            get { return _se; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _se = value;
                G_Button.IsEnabled = !String.IsNullOrEmpty(value.CurrentProjectPath);
            }
        }

        private void GC_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var settingsWindow = new CodeGenerationSettings();
            settingsWindow.SetSolutionPath(SE.CurrentSolutionPath);
            if (settingsWindow.ShowDialog() == true)
            {
                Generator.GenerateByProject(SE.CurrentProjectPath);
                SE.CurrentSolutionPath = SE.CurrentSolutionPath;
            }
        }
    }
}
