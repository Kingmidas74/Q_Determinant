﻿using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace QStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region CustomEvents

            #region SelectingFile
            public static readonly RoutedEvent SelectingFileEvent = EventManager.RegisterRoutedEvent("SelectingFile",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler SelectingFile
            {
                add { AddHandler(SelectingFileEvent, value); }
                remove { RemoveHandler(SelectingFileEvent, value); }
            }
            #endregion

            #region SaveAll
            public static readonly RoutedEvent SaveAllEvent = EventManager.RegisterRoutedEvent("SaveAll",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler SaveAll
            {
                add { AddHandler(SaveAllEvent, value); }
                remove { RemoveHandler(SaveAllEvent, value); }
            }
            #endregion

            #region CloseSolution
            public static readonly RoutedEvent CloseSolutionEvent = EventManager.RegisterRoutedEvent("CloseSolution",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler CloseSolution
            {
                add { AddHandler(CloseSolutionEvent, value); }
                remove { RemoveHandler(CloseSolutionEvent, value); }
            }
            #endregion

            #region Error
            public static readonly RoutedEvent ErrorEvent = EventManager.RegisterRoutedEvent("Error",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler Error
            {
                add { AddHandler(ErrorEvent, value); }
                remove { RemoveHandler(ErrorEvent, value); }
            }
            #endregion
        
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            AddHandler(BasicComponentsPack.SolutionExplorer.SelectingFileEvent, new RoutedEventHandler(SelectedFile));
            AddHandler(BasicComponentsPack.WorkplaceTabs.ErrorExceptionEvent, new RoutedEventHandler(ErrorHandler));
            AddHandler(SelectingFileEvent, new RoutedEventHandler(WorkplaceTabs.SelectedFileListener));
            AddHandler(SaveAllEvent, new RoutedEventHandler(WorkplaceTabs.SaveAllListener));
            AddHandler(CloseSolutionEvent, new RoutedEventHandler(WorkplaceTabs.CloseSolutionListener));
            AddHandler(CloseSolutionEvent, new RoutedEventHandler(SolutionExplorer.CloseSolutionListener));
            AddHandler(ErrorEvent, new RoutedEventHandler(ErrorMessage));
        }

        private void ErrorMessage(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(e.OriginalSource.ToString());
        }

        private void ErrorHandler(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ErrorEvent, e.OriginalSource));
        }

        private void SelectedFile(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SelectingFileEvent, e.OriginalSource));
        }

        private void CompilerClick(object sender, RoutedEventArgs e)
        {/*
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
            */
            //SolutionExplorer.CurrentSolutionPath = @"D:\tempforQ\NewQSOL\Testsolution.qsln";

        }

        private void OpenSolution(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.DefaultExt = ".qsln";
            openDialog.Filter = "SolutionFiles (*.qsln)|*.qsln";
            var result = openDialog.ShowDialog();
            if (result == true)
            {
                SolutionExplorer.CurrentSolutionPath = openDialog.FileName;
            }
        }

        private void CloseProgram(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void SaveAllClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SaveAllEvent));
        }

        private void CloseSolutionClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CloseSolutionEvent));
        }
    }
}
