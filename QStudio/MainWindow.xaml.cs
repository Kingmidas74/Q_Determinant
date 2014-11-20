using System.Collections.Generic;
using System.Windows;
using Core.Serializers;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.ProjectModels;
using Core.Serializers.SerializationModels.SolutionModels;
using Project = Core.Serializers.SerializationModels.SolutionModels.Project;

namespace QStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompilerClick(object sender, RoutedEventArgs e)
        {
            var solution = new Solution {Title = "Testsolution", Properties = new Core.Serializers.SerializationModels.SolutionModels.Properties {MaxCPU = 6}};
            solution.Projects.Add(new Project {Path = @"TestProject\TestProject.qpr", Title="TestProject"});
            solution.Projects.Add(new Project { Path = @"NewProject\NewProject.qpr", Title = "NewProject" });
            var s = Factory.GeSerializer();
            s.SerializeSolution(@"D:\tempforQ\NewQSOL\Testsolution.qsln", solution);

            var TestProject = new Core.Serializers.SerializationModels.ProjectModels.Project
            {
                Properties =
                    new Core.Serializers.SerializationModels.ProjectModels.Properties {Type = ProjectTypes.Algorithm},
                Title = "TestProject",
                Files = new List<File> {new File {Path = @"FlowChart.fc"}},
                References = new List<Reference> {new Reference {ProjectTitle = "NewProject"}}
            };
            var NewProject = new Core.Serializers.SerializationModels.ProjectModels.Project
            {
                Properties =
                    new Core.Serializers.SerializationModels.ProjectModels.Properties { Type = ProjectTypes.Function },
                Title = "NewProject",
                Files = new List<File>
                { 
                    new File { Path = @"FlowChart.fc" },
                    new File {Path = @"ImplementationPlan.ip"} 
                },
                References = new List<Reference>()
            };
            s.SerializeProject(@"D:\tempforQ\NewQSOL\TestProject\TestProject.qpr", TestProject);
            s.SerializeProject(@"D:\tempforQ\NewQSOL\NewProject\NewProject.qpr", NewProject);




        }
    }
}
