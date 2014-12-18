using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Core.Atoms;
using Core.Converters;
using Core.Serializers;
using Core.Serializers.SerializationModels;
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

        private string currentSolutionPath { get; set; }

        public void SetSolutionPath(string currentPathToSolution)
        {
            currentSolutionPath = currentPathToSolution;
            Core.Serializers.SerializationModels.SolutionModels.Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(currentSolutionPath, out solution);
            var tabItems = new List<EnclosedTabItem>();
            foreach (var project in solution.Projects.Where(x => x.Type == ProjectTypes.Algorithm))
            {
                tabItems.Add(CreateTabitemByProject(project));
            }
            ProjectTabControl.ItemsSource = null;
            ProjectTabControl.ItemsSource = tabItems;
        }

        private EnclosedTabItem CreateTabitemByProject(Core.Serializers.SerializationModels.SolutionModels.Project project)
        {
            var pathToProject = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentSolutionPath),
                project.Path);
            var result = new EnclosedTabItem {Header = project.Title};
            var variables = new List<string>();
            foreach (var variable in Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(pathToProject),"ImplementationPlan.ip")),ConverterFormats.JSON).Vertices.Where(x=>x.Level==0))
            {
                decimal d;
                double l;
                if (!(Decimal.TryParse(variable.Content, out d) || Double.TryParse(variable.Content, out l)))
                {
                    variables.Add(variable.Content);
                }
            }
            var content = new SettingsProjectTabItem();
            content.SetContent(variables);
            result.Content = content;
            return result;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            Core.Serializers.SerializationModels.SolutionModels.Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(currentSolutionPath, out solution);
            foreach (var project in solution.Projects.Where(x => x.Type == ProjectTypes.Algorithm))
            {
                MessageBox.Show(Generator.ConvertWithTemplate(@"CodeGeneration\LanguagesTransformers\Cpp.xslt",null,
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentSolutionPath),
                        project.Path)));
            }
            
            DialogResult = true;
        }
    }
}
