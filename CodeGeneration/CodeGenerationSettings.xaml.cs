using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using CodeGeneration.InternalClasses;
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
        
        readonly CGViewModel _cgvm = new CGViewModel();
        private string currentSolutionPath { get; set; }

        public CodeGenerationSettings()
        {
            InitializeComponent();
            DataContext = _cgvm;
        }
        public void SetSolutionPath(string currentPathToSolution)
        {
            currentSolutionPath = currentPathToSolution;
            Core.Serializers.SerializationModels.SolutionModels.Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(currentSolutionPath, out solution);
            _cgvm.CurrentSolutionPath = currentSolutionPath;
            foreach (var project in solution.Projects.Where(x => x.Type == ProjectTypes.Algorithm))
            {
                _cgvm.ProjectsCollection.Add(project);
            }
            if (_cgvm.ProjectsCollection.Count > 0)
            {
                _cgvm.CurrentProjectIndex = 0;
            }
            var languageXSLTFiles =
                (new DirectoryInfo(System.IO.Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"CodeGeneration\LanguagesTransformers")))
                    .GetFiles();
            foreach (var languageXsltFile in languageXSLTFiles)
            {
                _cgvm.LanguageCollection.Add(languageXsltFile.FullName);
            }
            if (_cgvm.LanguageCollection.Count > 0)
            {
                _cgvm.CurrentLanguageIndex = 0;
            }
        }

        
        private void CancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                _cgvm.Variables.Count+" "+
                _cgvm.OutputName + " " +
                _cgvm.LanguageCollection[_cgvm.CurrentLanguageIndex] + " " +
                _cgvm.PlansCollection[_cgvm.CurrentPlanIndex]
                );
            MessageBox.Show(Generator.ConvertWithTemplate(_cgvm.LanguageCollection[_cgvm.CurrentLanguageIndex], _cgvm.Variables, _cgvm.PlansCollection[_cgvm.CurrentPlanIndex]));
        }
    }
}
