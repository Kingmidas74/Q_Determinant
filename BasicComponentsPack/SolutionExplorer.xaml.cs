using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BasicComponentsPack.InternalClasses;
using Core.Serializers;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.SolutionModels;
using Microsoft.Win32;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for SolutionExplorer.xaml
    /// </summary>
    public partial class SolutionExplorer : UserControl
    {
        #region SelectingFile
        public static readonly RoutedEvent SelectingFileEvent = EventManager.RegisterRoutedEvent("SelectingFile",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SolutionExplorer));

        public event RoutedEventHandler SelectingFile
        {
            add { AddHandler(SelectingFileEvent, value); }
            remove { RemoveHandler(SelectingFileEvent, value); }
        }
        #endregion

        private string _currentSolutionPath = String.Empty;

        public string CurrentSolutionPath
        {
            get { return _currentSolutionPath; }
            set
            {
                _currentSolutionPath = value;
                RefreshSolution();
            }
        }

        private void RefreshSolution()
        {
            SearchTreeView.ItemsSource = null;
            if (!String.IsNullOrEmpty(CurrentSolutionPath))
            {
                Solution solution;
                var serializer = SerializersFactory.GetSerializer();
                serializer.DeserializeSolution(CurrentSolutionPath, out solution);
                var result = new SolutionTreeItem {FilePath = CurrentSolutionPath, Title = solution.Title};
                foreach (var currentProjectView in solution.Projects.Select(project => new SolutionTreeItem
                {
                    FilePath = Path.Combine(Path.GetDirectoryName(result.FilePath), project.Path)
                }))
                {
                    Core.Serializers.SerializationModels.ProjectModels.Project currentProjectModel;
                    serializer.DeserializeProject(currentProjectView.FilePath, out currentProjectModel);
                    currentProjectView.Title = currentProjectModel.Title;
                    foreach (var file in currentProjectModel.Files)
                    {
                        var currentFileView = new SolutionTreeItem
                        {
                            FilePath = Path.Combine(Path.GetDirectoryName(currentProjectView.FilePath), file.Path),
                            Title = file.Path
                        };
                        currentProjectView.Items.Add(currentFileView);
                    }
                    result.Items.Add(currentProjectView);
                }
                var solutions = new List<SolutionTreeItem> {result};
                SearchTreeView.ItemsSource = solutions;
            }
        }

        public SolutionExplorer()
        {
            InitializeComponent();
        }

        public void CloseSolutionListener(object sender, RoutedEventArgs e)
        {
            CurrentSolutionPath = null;
        }

        private void ChooseTreeElement(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                RaiseEvent(new RoutedEventArgs(SelectingFileEvent, (sender as TextBlock).Tag.ToString()));
            }
        }

        public void OpenSolutionListener(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".qsln";
            openDialog.Filter = "SolutionFiles (*.qsln)|*.qsln";
            openDialog.Title = "Открытие решения...";
            var result = openDialog.ShowDialog();
            if (result == true)
            {
                CurrentSolutionPath = openDialog.FileName;
            }
        }

        public void NewSolutionListener(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = ".qsln";
            saveDialog.Filter = "SolutionFiles (*.qsln)|*.qsln";
            saveDialog.Title = "Создание решения...";
            var result = saveDialog.ShowDialog();
            if (result == true)
            {
                var newSolution = new Core.Serializers.SerializationModels.SolutionModels.Solution();
                
                var solutionFile = new FileInfo(saveDialog.FileName);
                newSolution.Title = Path.GetFileNameWithoutExtension(solutionFile.Name);
                newSolution.Properties.MaxCPU = 1;
                newSolution.Projects.Add(new Core.Serializers.SerializationModels.SolutionModels.Project { Path = string.Format("{0}\\{0}.qpr", newSolution.Title), Title = newSolution.Title, Type = ProjectTypes.Algorithm });
                
                var defaultProject = new Core.Serializers.SerializationModels.ProjectModels.Project();
                defaultProject.Title = newSolution.Title;
                defaultProject.Properties.Type = ProjectTypes.Algorithm;
                defaultProject.Files.Add(new Core.Serializers.SerializationModels.ProjectModels.File {Path = "FlowChart.fc"});

                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(solutionFile.FullName), newSolution.Title));

                var serializer = SerializersFactory.GetSerializer();
                serializer.SerializeSolution(saveDialog.FileName, newSolution);
                serializer.SerializeProject(
                    Path.Combine(Path.GetDirectoryName(solutionFile.FullName), newSolution.Title,
                        newSolution.Title + ".qpr"), defaultProject);
                CurrentSolutionPath = saveDialog.FileName;
            }
        }

        public void NewProjectListener(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("NPL");
        }
    }
}
