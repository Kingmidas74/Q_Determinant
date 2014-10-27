﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ModernControls.InternalClasses;

namespace ModernControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls;assembly=ModernControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ExtendedTreeView/>
    ///
    /// </summary>
    public class ExtendedTreeView : TreeView
    {

        public string CurrentSolutionPath { get; set; }
        static ExtendedTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTreeView), new FrameworkPropertyMetadata(typeof(ExtendedTreeView)));
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("RefreshSolutionButton") as Button).Click += new RoutedEventHandler(RefreshSolutionButtonClick);
            base.OnApplyTemplate();
        }

        private void RefreshSolutionButtonClick(object sender, RoutedEventArgs e)
        {
            RefreshSolution();
        }

        public void RefreshSolution(string pathToSolutionFile = null)
        {
         /*   if (String.IsNullOrEmpty(CurrentSolutionPath) && String.IsNullOrEmpty(pathToSolutionFile)) throw new Exception("Не указан путь до решения");
            var pathToSolution = CurrentSolutionPath;
            if (!String.IsNullOrEmpty(pathToSolutionFile))
            {
                pathToSolution = pathToSolutionFile;
            }
            var rootDirectory = new DirectoryInfo(@pathToSolution);
            foreach (var item in rootDirectory.GetDirectories())
            {
                var currentAlgorithmProject = new SolutionTreeItem(SolutionItemTypes.Project);

                currentAlgorithmProject.Name = item.Name;
                foreach (var file in item.GetFiles("*.*").Where(s => validExtensions.Any(e => s.Extension.EndsWith(e))))
                {
                    var currentAlgorithmFile = new AlgorithmFile();
                    currentAlgorithmFile.Name = file.Name;
                    currentAlgorithmFile.Path = file.FullName;
                    currentAlgorithmProject.Files.Add(currentAlgorithmFile);
                }
                result.Add(currentAlgorithmProject);
            }*/
             

        }

    
    }
}