using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BasicComponentsPack.InternalClasses;
using Core.Serializers;
using Core.Serializers.SerializationModels.SolutionModels;
using DefaultControlsPack;

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
            //SearchTreeView.ItemsSource = null;
            if (!String.IsNullOrEmpty(CurrentSolutionPath))
            {
                var solution = new Core.Serializers.SerializationModels.SolutionModels.Solution();
                var serializer = SerializersFactory.GeSerializer();
                serializer.DeserializeSolution(CurrentSolutionPath, out solution);
                var result = new SolutionTreeItem();
                result.FilePath = CurrentSolutionPath;
                result.Title = solution.Title;
                foreach (var project in solution.Projects)
                {
                    var currentProjectView = new SolutionTreeItem();
                    currentProjectView.FilePath = Path.Combine(Path.GetDirectoryName(result.FilePath), project.Path);
                    var currentProjectModel = new Core.Serializers.SerializationModels.ProjectModels.Project();
                    serializer.DeserializeProject(currentProjectView.FilePath, out currentProjectModel);
                    currentProjectView.Title = currentProjectModel.Title;
                    foreach (var file in currentProjectModel.Files)
                    {
                        var currentFileView = new SolutionTreeItem();
                        currentFileView.FilePath = Path.Combine(Path.GetDirectoryName(currentProjectView.FilePath), file.Path);
                        currentFileView.Title = file.Path;
                        currentProjectView.Items.Add(currentFileView);
                    }
                    result.Items.Add(currentProjectView);
                }
                var solutions = new List<SolutionTreeItem>() {result};
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
    }
}
