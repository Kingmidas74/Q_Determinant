using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BasicComponentsPack;
using Core.Atoms;
using Core.Converters;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.ProjectModels;
using DefaultControlsPack;

namespace ImplementationPlanViewer.InternalClasses
{
    public static class Methods
    {
        public static Viewer CurrentViewer { get; set; }
        public static SolutionExplorer SE { get; set; }

        public static EnclosedTabItem PropertyGridInstance { get; set; }


        public static void CreateProject()
        {
            if (CurrentViewer == null) return;
            var SelectedBlocks = (CurrentViewer.DataContext as ViewerVM).Blocks.Select(x => x.DataContext as VisualBlockVM).Where(context => context.IsSelected && context.Block.Level!=0).ToList();
            if(SelectedBlocks.Count==0) return;
            var createDialog = new CreateProject();
            if (createDialog.ShowDialog() == true && !String.IsNullOrEmpty(createDialog.ProjectTitle) &&
                !String.IsNullOrEmpty(SE.CurrentSolutionPath))
            {
                MessageBox.Show("NEWP");
                //CreateProject(createDialog.ProjectTitle, createDialog.ProjectTitle, createDialog.ProjectType);
            }
        }

        private static void CreateProject(string folder, string title, int projectType, Core.Serializers.SerializationModels.SolutionModels.Solution currentSolution = null)
        {
            var newProject = new Core.Serializers.SerializationModels.ProjectModels.Project
            {
                Title = title,
                Properties = { Type = projectType == 0 ? ProjectTypes.Function : ProjectTypes.Algorithm }
            };
            newProject.AddFile(new Core.Serializers.SerializationModels.ProjectModels.File { Path = "FlowChart.fc" });

            var globalReferenceDirectory = new DirectoryInfo("BasicFunctions");
            foreach (var referenceDirectory in globalReferenceDirectory.GetDirectories())
            {
                foreach (var file in referenceDirectory.GetFiles().Where(file => file.Extension.Equals(".qpr")))
                {
                    var referenceProject = Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(file.FullName);
                    newProject.AddReference(new Reference
                    {
                        ProjectPath = file.FullName,
                        ProjectTitle = referenceProject.Title
                    });
                    break;
                }
            }

            var solution = currentSolution ?? Core.Serializers.SerializationModels.SolutionModels.Solution.Deserialize(SE.CurrentSolutionPath);

            solution.AddProject(new Core.Serializers.SerializationModels.SolutionModels.Project { Path = string.Format("{0}\\{0}.qpr", folder), Title = newProject.Title, Type = newProject.Properties.Type });

            Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(SE.CurrentSolutionPath), folder));
            System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(SE.CurrentSolutionPath), folder,
                    "FlowChart.fc"), Converter.GraphToData(new Graph(), ConverterFormats.JSON));

            solution.Serialize(SE.CurrentSolutionPath);
            newProject.Serialize(Path.Combine(Path.GetDirectoryName(SE.CurrentSolutionPath), folder,
                    folder + ".qpr"));
        }
    }
}
