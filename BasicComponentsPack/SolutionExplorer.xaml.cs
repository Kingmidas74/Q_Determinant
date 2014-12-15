using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BasicComponentsPack.InternalClasses;
using Core.Atoms;
using Core.Serializers;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.ProjectModels;
using Core.Serializers.SerializationModels.SolutionModels;
using Microsoft.Win32;
using Core.Converters;
using VisualCore;
using VisualCore.Events;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for SolutionExplorer.xaml
    /// </summary>
    public partial class SolutionExplorer : ICompile
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

        #region SetProject
        public static readonly RoutedEvent SetProjectEvent = EventManager.RegisterRoutedEvent("SetProject",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SolutionExplorer));

        public event RoutedEventHandler SetProject
        {
            add { AddHandler(SetProjectEvent, value); }
            remove { RemoveHandler(SetProjectEvent, value); }
        }
        #endregion

        #region ErrorException
        public static readonly RoutedEvent ErrorExceptionEvent = EventManager.RegisterRoutedEvent("ErrorException",
            RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(SolutionExplorer));

        public event RoutedEventHandler ErrorException
        {
            add { AddHandler(ErrorExceptionEvent, value); }
            remove { RemoveHandler(ErrorExceptionEvent, value); }
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

        private string _currentProjectPath = String.Empty;

        public string CurrentProjectPath
        {
            get { return _currentProjectPath; }
            set
            {
                _currentProjectPath = value;
                RaiseEvent(String.IsNullOrEmpty(_currentProjectPath)
                    ? new RoutedEventArgs(SetProjectEvent, "NULL")
                    : new RoutedEventArgs(SetProjectEvent, _currentProjectPath));
            }
        }

        private readonly Dictionary<string, Func<string, string, SolutionTreeItem>> _elementRevealers;

        private SolutionTreeItem PerformRevealer(string filePath, string title)
        {
            return !_elementRevealers.ContainsKey(System.IO.Path.GetExtension(filePath)) ? CreateDefaultTreeItem(filePath, title) : _elementRevealers[System.IO.Path.GetExtension(filePath)](filePath, title);
        }

        

        public void DefineRevealer(string extention, Func<string,string, SolutionTreeItem> revealer)
        {
            try
            {
                if (_elementRevealers.ContainsKey(extention))
                {
                    _elementRevealers[extention] = revealer;
                }
                else
                {
                    _elementRevealers.Add(extention, revealer);
                }
            }
            catch (Exception e)
            {
               RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, e.Message));
            }
        }

        private void RefreshSolution()
        {
            SearchTreeView.ItemsSource = null;
            if (!String.IsNullOrEmpty(CurrentSolutionPath))
            {
                try
                {
                    Solution solution;
                    var serializer = SerializersFactory.GetSerializer();
                    serializer.DeserializeSolution(CurrentSolutionPath, out solution);
                    var result = PerformRevealer(CurrentSolutionPath, solution.Title);
                    foreach (
                        var currentProjectView in
                            solution.Projects.Select(
                                project =>
                                    PerformRevealer(Path.Combine(Path.GetDirectoryName(result.FilePath), project.Path),
                                        null)))
                    {
                        Core.Serializers.SerializationModels.ProjectModels.Project currentProjectModel;
                        serializer.DeserializeProject(currentProjectView.FilePath, out currentProjectModel);
                        currentProjectView.Title = currentProjectModel.Title;
                        if (currentProjectModel.Properties.Type ==
                            Core.Serializers.SerializationModels.ProjectTypes.Algorithm)
                        {
                            CurrentProjectPath = currentProjectView.FilePath;
                        }
                        var referenceCollection =
                            PerformRevealer(currentProjectView.FilePath, "References");
                        foreach (
                            var currentReferenceView in
                                currentProjectModel.References.Select(
                                    reference => PerformRevealer(reference.ProjectPath, reference.ProjectTitle)))
                        {
                            referenceCollection.Items.Add(currentReferenceView);
                        }
                        currentProjectView.Items.Add(referenceCollection);
                        foreach (
                            var currentFileView in
                                currentProjectModel.Files.Select(
                                    file =>
                                        PerformRevealer(
                                            Path.Combine(Path.GetDirectoryName(currentProjectView.FilePath), file.Path),
                                            Path.GetFileName(file.Path))))
                        {
                            currentProjectView.Items.Add(currentFileView);
                        }
                        result.Items.Add(currentProjectView);
                    }
                    var solutions = new List<SolutionTreeItem> {result};
                    SearchTreeView.ItemsSource = solutions;
                }
                catch (Exception e)
                {
                    RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, e.Message));
                }
            }
        }

        public SolutionExplorer()
        {
            InitializeComponent();
            _elementRevealers = new Dictionary<string, Func<string, string, SolutionTreeItem>>
            {
                {".qpr", CreateProjectTreeItem},
                {".fc", CreateFlowChartTreeItem},
                {".ip", CreateImplementationPlanTreeItem},
                {".qsln", CreateSolutionTreeItem},
            };
        }

        private SolutionTreeItem CreateDefaultTreeItem(string filePath, string title)
        {
            return new SolutionTreeItem { FilePath = filePath, Title = title };
        }

        private SolutionTreeItem CreateSolutionTreeItem(string filePath, string title)
        {
            return new SolutionTreeItem
            {
                FilePath = filePath,
                Title = title,
                Icon = Helpers.XamlClone(FindResource("SolutionIcon") as System.Windows.Shapes.Path)
            };
        }

        private SolutionTreeItem CreateImplementationPlanTreeItem(string filePath, string title)
        {
            return new SolutionTreeItem
            {
                FilePath = filePath,
                Title = title,
                Icon = Helpers.XamlClone(FindResource("ImplementationPlanIcon") as System.Windows.Shapes.Path)
            };
        }

        private SolutionTreeItem CreateProjectTreeItem(string filePath, string title)
        {
            return new SolutionTreeItem
            {
                FilePath = filePath,
                Title = title,
                Icon = Helpers.XamlClone(FindResource("ProjectIcon") as System.Windows.Shapes.Path)
            };
        }

        private SolutionTreeItem CreateFlowChartTreeItem(string filePath, string title)
        {
            return new SolutionTreeItem
            {
                FilePath = filePath,
                Title = title,
                Icon = Helpers.XamlClone(FindResource("FlowChartIcon") as System.Windows.Shapes.Path)
            };
        }

        public void CloseSolutionListener(object sender, RoutedEventArgs e)
        {
            CurrentSolutionPath = null;
            CurrentProjectPath = null;
        }

        private void ChooseTreeElement(object sender, MouseButtonEventArgs e)
        {
            if ((sender as TextBlock).Tag != null)
            {
                var file = new FileInfo((sender as TextBlock).Tag.ToString());
                if (!file.Extension.Equals(".qsln"))
                {
                    var directory = new DirectoryInfo(Path.GetDirectoryName(file.FullName));
                    foreach (var projectFile in directory.GetFiles().Where(x=>x.Extension.Equals(".qpr")))
                    {
                        CurrentProjectPath = projectFile.FullName;
                        break;
                    }
                }
                if (e.ClickCount == 2)
                {
                    RaiseEvent(new RoutedEventArgs(SelectingFileEvent, (sender as TextBlock).Tag.ToString()));
                }
            }
        }

        public void OpenSolutionListener(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog
            {
                DefaultExt = ".qsln",
                Filter = "SolutionFiles (*.qsln)|*.qsln",
                Title = "Открытие решения..."
            };
            var result = openDialog.ShowDialog();
            if (result == true)
            {
                CurrentSolutionPath = openDialog.FileName;
            }
        }

        public void NewSolutionListener(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog
            {
                DefaultExt = ".qsln",
                Filter = "SolutionFiles (*.qsln)|*.qsln",
                Title = "Создание решения..."
            };
            var result = saveDialog.ShowDialog();
            if (result == true)
            {
                var newSolution = new Core.Serializers.SerializationModels.SolutionModels.Solution();
                var solutionFile = new FileInfo(saveDialog.FileName);
                newSolution.Title = Path.GetFileNameWithoutExtension(solutionFile.Name);
                newSolution.Properties.MaxCPU = 1;
                SerializersFactory.GetSerializer().SerializeSolution(saveDialog.FileName, newSolution);
                CurrentSolutionPath = saveDialog.FileName;
                CreateProject(Path.GetFileNameWithoutExtension(saveDialog.FileName), Path.GetFileNameWithoutExtension(saveDialog.FileName),1);
                CurrentSolutionPath = saveDialog.FileName;
            }
        }

        public void NewProjectListener(object sender, RoutedEventArgs e)
        {
            var createDialog = new CreateProject();
            if (createDialog.ShowDialog() == true && !String.IsNullOrEmpty(createDialog.ProjectTitle) &&
                !String.IsNullOrEmpty(CurrentSolutionPath))
            {
                CreateProject(createDialog.ProjectTitle, createDialog.ProjectTitle,createDialog.ProjectType);
                RefreshSolution();
            }
        }

        private void CreateProject(string folder, string title, int projectType)
        {
            var projectTitile = title;
            var newProject = new Core.Serializers.SerializationModels.ProjectModels.Project
            {
                Title = projectTitile,
                Properties = {Type = projectType == 0 ? ProjectTypes.Function : ProjectTypes.Algorithm}
            };
            newProject.Files.Add(new Core.Serializers.SerializationModels.ProjectModels.File { Path = "FlowChart.fc" });
            var references = new List<Reference>();
            var globalReferenceDirectory = new DirectoryInfo("BasicFunctions");
            foreach (var referenceDirectory in globalReferenceDirectory.GetDirectories())
            {
                foreach (var file in referenceDirectory.GetFiles().Where(file => file.Extension.Equals(".qpr")))
                {
                    Core.Serializers.SerializationModels.ProjectModels.Project referenceProject;
                    SerializersFactory.GetSerializer().DeserializeProject(file.FullName,out referenceProject);
                    var reference = new Reference
                    {
                        ProjectPath = file.FullName,
                        ProjectTitle = referenceProject.Title
                    };
                    references.Add(reference);
                    break;
                }
            }
            newProject.References = references;
            Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(CurrentSolutionPath, out solution);
            solution.Projects.Add(new Core.Serializers.SerializationModels.SolutionModels.Project { Path = string.Format("{0}\\{0}.qpr", folder), Title = newProject.Title, Type = newProject.Properties.Type });
            Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), folder));
            System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), folder,
                    "FlowChart.fc"), Converter.GraphToData(new Graph(), ConverterFormats.JSON));
            var serializer = SerializersFactory.GetSerializer();
            serializer.SerializeSolution(CurrentSolutionPath, solution);
            serializer.SerializeProject(
                Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), folder,
                    folder + ".qpr"), newProject);
        }

        public void AfterCompilerListener(object sender, RoutedEventArgs e)
        {
            RefreshSolution();
        }

        public void ReferenceManagerListener(object sender, RoutedEventArgs e)
        {
            var referenceDialog = new ReferenceManager
            {
                CurrentSolutionPath = CurrentSolutionPath,
                CurrentProjectPath = CurrentProjectPath
            };
            if (referenceDialog.ShowDialog() == true)
            {
                Core.Serializers.SerializationModels.ProjectModels.Project currentProject;
                SerializersFactory.GetSerializer().DeserializeProject(CurrentProjectPath, out currentProject);
                currentProject.References = referenceDialog.ReferenceCollection;
                SerializersFactory.GetSerializer().SerializeProject(CurrentProjectPath, currentProject);
                RefreshSolution();
            }
        }

        public void BeforeCompilerListener(object sender, RoutedEventArgs e)
        {
        }

    }
}
