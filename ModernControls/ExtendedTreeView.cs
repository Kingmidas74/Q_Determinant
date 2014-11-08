﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Converters.FlowchartsConverters;
using Converters.ImplementationPlanConverters;
using ModernControls.InternalClasses;
using ModernControls.InternalClasses.Dialogs;
using Converters;

namespace ModernControls
{
    public class ExtendedTreeView : TreeView
    {

        
        private WriteLogsDelegate _logsDelegate;
        public string CurrentSolutionPath { get; set; }
        public string CurrentProjectPath { get; set; }

        public ExtendedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTreeView), new FrameworkPropertyMetadata(typeof(ExtendedTreeView)));
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("RefreshSolutionButton") as Button).Click += RefreshSolutionButtonClick;           
            base.OnApplyTemplate();
        }

        public void ExportFC()
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(CurrentProjectPath));
            foreach (var file in directory.GetFiles().Where(file => file.Extension.Equals(".fc")))
            {
                var dialog = DialogFactory.CallSaveDialog(DialogTypes.FlowChart);
                if (dialog.ShowDialog()==true)
                {
                    var FromConverter = Manufactory.CreateFlowChartConverter(ConverterTypes.XML);
                    FromConverter.ParseDocument(file.FullName);
                    var targetFile = new FileInfo(dialog.FileName);
                    IFlowchartsConverter ToConverter;
                    switch (targetFile.Extension)
                    {
                        case ".xml" :
                            ToConverter = FromConverter;
                            ToConverter.SaveToFile(targetFile.FullName);
                            break;
                        case ".json":
                            ToConverter = Manufactory.CreateFlowChartConverter(ConverterTypes.JSON);
                            ToConverter.SetBlocks(FromConverter.GetBlocks());
                            ToConverter.SetLinks(FromConverter.GetLinks());
                            ToConverter.SaveToFile(targetFile.FullName);
                            break;
                    }
                }
            }
        }

        public void ExportQD()
        {
            MessageBox.Show("QD");
        }
        public void ExportIP()
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(CurrentProjectPath));
            foreach (var file in directory.GetFiles().Where(file => file.Extension.Equals(".fc")))
            {
                var dialog = DialogFactory.CallSaveDialog(DialogTypes.ImplementationPlan);
                if (dialog.ShowDialog() == true)
                {
                    var FromConverter = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
                    FromConverter.ParseDocument(file.FullName);
                    var targetFile = new FileInfo(dialog.FileName);
                    IImplementationPlanConverter ToConverter;
                    switch (targetFile.Extension)
                    {
                        case ".json":
                            ToConverter = FromConverter;
                            ToConverter.SaveToFile(targetFile.FullName);
                            break;
                        case ".xml":
                            ToConverter = Manufactory.CreateImplementationPlanConverter(ConverterTypes.XML);
                            ToConverter.SetBlocks(FromConverter.GetBlocks());
                            ToConverter.SetLinks(FromConverter.GetLinks());
                            ToConverter.SaveToFile(targetFile.FullName);
                            break;
                    }
                }
            }
        }

        public void AddProject()
        {
            var dialog = new NewProjectDialog();
            if (dialog.ShowDialog() == true && !String.IsNullOrEmpty(dialog.ProjectTitle))
            {
                Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), dialog.ProjectTitle));
                var xmlDoc = new XmlDocument();
                
                var projectNode = xmlDoc.CreateNode(XmlNodeType.Element, "Project", null);
                var filesNode = xmlDoc.CreateNode(XmlNodeType.Element, "Files", null);
                var flowchartNode = xmlDoc.CreateNode(XmlNodeType.Element, "File", null);
                var pathAttribute = xmlDoc.CreateAttribute("Path");
                pathAttribute.InnerText = "FlowChart.fc";
                flowchartNode.Attributes.Append(pathAttribute);
                var typeAttribute = xmlDoc.CreateAttribute("Type");
                typeAttribute.InnerText = "FlowChart";
                flowchartNode.Attributes.Append(typeAttribute);
                filesNode.AppendChild(flowchartNode);
                var referencesNode = xmlDoc.CreateNode(XmlNodeType.Element, "References", null);
                projectNode.AppendChild(referencesNode);
                projectNode.AppendChild(filesNode);
                xmlDoc.AppendChild(projectNode);
                xmlDoc.Save(Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), dialog.ProjectTitle, dialog.ProjectTitle+".qpr"));
                xmlDoc = new XmlDocument();
                var FCNode = xmlDoc.CreateNode(XmlNodeType.Element, "FlowChart", null);
                xmlDoc.AppendChild(FCNode);
                xmlDoc.Save(Path.Combine(Path.GetDirectoryName(CurrentSolutionPath), dialog.ProjectTitle, "FlowChart.fc"));
                xmlDoc = new XmlDocument();
                xmlDoc.Load(CurrentSolutionPath);
                var rChild = xmlDoc.SelectSingleNode("//Projects");
                var nChild = xmlDoc.CreateNode(XmlNodeType.Element, "Projects", null);
                foreach (XmlNode node in rChild.ChildNodes)
                {
                    nChild.AppendChild(node);
                }
                var nProj = xmlDoc.CreateNode(XmlNodeType.Element, "Project", null);
                var pathProj = xmlDoc.CreateAttribute("Path");
                pathProj.InnerText = dialog.ProjectTitle + "\\" + dialog.ProjectTitle + ".qpr";
                nProj.Attributes.Append(pathProj);
                nChild.AppendChild(nProj);
                xmlDoc.SelectSingleNode("//Solution").ReplaceChild(nChild, rChild);
                xmlDoc.Save(CurrentSolutionPath);
                RefreshSolution();
            }
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
                result.ProjectPath = "";
                var solutionXmlDoc = new XmlDocument();
                var rootDirectory = Path.GetDirectoryName(pathToSolution);
                solutionXmlDoc.Load(@pathToSolution);
                result.Title = solutionXmlDoc.SelectSingleNode("//Properties/Title").InnerText;
                foreach (XmlNode project in solutionXmlDoc.SelectNodes("//Project"))
                {
                    var currentAlgorithmProject = new SolutionTreeItem(SolutionItemTypes.Project);
                    var projectXmlDoc = new XmlDocument();
                    currentAlgorithmProject.FilePath = Path.Combine(rootDirectory, project.Attributes["Path"].InnerText);
                    currentAlgorithmProject.ProjectPath = currentAlgorithmProject.FilePath;
                    currentAlgorithmProject.Title = Path.GetFileName(currentAlgorithmProject.FilePath);
                    var projectDirectory = Path.GetDirectoryName(currentAlgorithmProject.FilePath);
                    projectXmlDoc.Load(currentAlgorithmProject.FilePath);
                    foreach (XmlNode file in projectXmlDoc.SelectNodes("//File"))
                    {
                        var currentFileInProject = new SolutionTreeItem((SolutionItemTypes)Enum.Parse(typeof(SolutionItemTypes), file.Attributes["Type"].InnerText));
                        currentFileInProject.FilePath = Path.Combine(projectDirectory, file.Attributes["Path"].InnerText);
                        currentFileInProject.Title = Path.GetFileName(currentFileInProject.FilePath);
                        currentFileInProject.ProjectPath = currentAlgorithmProject.ProjectPath;
                        currentAlgorithmProject.Items.Add(currentFileInProject);
                    }
                    var currentProjectReference = new SolutionTreeItem(SolutionItemTypes.ReferenceCollection);
                    currentProjectReference.Title = "References";
                    currentProjectReference.ProjectPath = currentAlgorithmProject.ProjectPath;
                    foreach (XmlNode reference in projectXmlDoc.SelectNodes("//Reference"))
                    {
                        var currentReferenceInProject = new SolutionTreeItem(SolutionItemTypes.Reference);
                        currentReferenceInProject.Title = reference.Attributes["ProjectName"].InnerText;
                        currentReferenceInProject.ProjectPath = currentAlgorithmProject.ProjectPath;
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
