using System.Windows;
using Core.Serializers;

namespace CodeGeneration
{
    internal class Generator
    {
        internal static void GenerateByProject(string currentProjectPath)
        {
            Core.Serializers.SerializationModels.ProjectModels.Project project;
            SerializersFactory.GetSerializer().DeserializeProject(currentProjectPath,out project);
            MessageBox.Show(project.Title);
        }

        internal static void GenerateBySolution(string currentSolutionPath)
        {
            Core.Serializers.SerializationModels.SolutionModels.Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(currentSolutionPath, out solution);
            foreach (var project in solution.Projects)
            {
                
                GenerateByProject(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentSolutionPath), project.Path));
            }
        }
    }
}
