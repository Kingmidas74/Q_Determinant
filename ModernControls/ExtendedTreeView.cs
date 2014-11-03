using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using ModernControls.InternalClasses;

namespace ModernControls
{
    public class ExtendedTreeView : TreeView
    {

        
        private WriteLogsDelegate _logsDelegate;
        public string CurrentSolutionPath { get; set; }

        public ExtendedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTreeView), new FrameworkPropertyMetadata(typeof(ExtendedTreeView)));
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("RefreshSolutionButton") as Button).Click += RefreshSolutionButtonClick;           
            base.OnApplyTemplate();
        }

        private void RefreshSolutionButtonClick(object sender, RoutedEventArgs e)
        {
            _logsDelegate("Click Refresh Solution");
            RefreshSolution();
        }

        public void RefreshSolution(string pathToSolutionFile = null)
        {
            try
            {
                if (String.IsNullOrEmpty(CurrentSolutionPath) && String.IsNullOrEmpty(pathToSolutionFile))
                    throw new Exception("Не указан путь до решения");
                var pathToSolution = CurrentSolutionPath;
                if (!String.IsNullOrEmpty(pathToSolutionFile))
                {
                    pathToSolution = pathToSolutionFile;
                }
                var result = new SolutionTreeItem(SolutionItemTypes.Solution);
                var solutionXmlDoc = new XmlDocument();
                var rootDirectory = Path.GetDirectoryName(pathToSolution);
                solutionXmlDoc.Load(@pathToSolution);
                result.Title = solutionXmlDoc.SelectSingleNode("//Properties/Title").InnerText;
                foreach (XmlNode project in solutionXmlDoc.SelectNodes("//Project"))
                {
                    var currentAlgorithmProject = new SolutionTreeItem(SolutionItemTypes.Project);
                    var projectXmlDoc = new XmlDocument();
                    currentAlgorithmProject.FilePath = Path.Combine(rootDirectory, project.Attributes["Path"].InnerText);
                    currentAlgorithmProject.Title = Path.GetFileName(currentAlgorithmProject.FilePath);
                    var projectDirectory = Path.GetDirectoryName(currentAlgorithmProject.FilePath);
                    projectXmlDoc.Load(currentAlgorithmProject.FilePath);
                    foreach (XmlNode file in projectXmlDoc.SelectNodes("//File"))
                    {
                        var currentFileInProject = new SolutionTreeItem((SolutionItemTypes)Enum.Parse(typeof(SolutionItemTypes), file.Attributes["Type"].InnerText));
                        currentFileInProject.FilePath = Path.Combine(projectDirectory, file.Attributes["Path"].InnerText);
                        currentFileInProject.Title = Path.GetFileName(currentFileInProject.FilePath);
                        currentAlgorithmProject.Items.Add(currentFileInProject);
                    }
                    var currentProjectReference = new SolutionTreeItem(SolutionItemTypes.ReferenceCollection);
                    currentProjectReference.Title = "References";
                    foreach (XmlNode reference in projectXmlDoc.SelectNodes("//Reference"))
                    {
                        var currentReferenceInProject = new SolutionTreeItem(SolutionItemTypes.Reference);
                        currentReferenceInProject.Title = reference.Attributes["ProjectName"].InnerText;
                        currentProjectReference.Items.Add(currentReferenceInProject);
                    }
                    currentAlgorithmProject.Items.Add(currentProjectReference);
                    result.Items.Add(currentAlgorithmProject);
                }
                ItemsSource = null;
                ItemsSource = result.Items;
                CurrentSolutionPath = pathToSolution;
            }
            catch (Exception e)
            {
                _logsDelegate(e.Data.ToString(), LogType.Error);
            }
        }


        public void SetLogsDelegate(WriteLogsDelegate writeDelegate)
        {
            _logsDelegate = writeDelegate;
        }
    }
}
