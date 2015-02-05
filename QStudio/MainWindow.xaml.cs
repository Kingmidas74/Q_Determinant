using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using BasicComponentsPack;
using PluginController;
using System.Diagnostics;
using VisualCore.Events;

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

            #region OpenSolution
            public static readonly RoutedEvent OpenSolutionEvent = EventManager.RegisterRoutedEvent("OpenSolution",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler OpenSolution
            {
                add { AddHandler(OpenSolutionEvent, value); }
                remove { RemoveHandler(OpenSolutionEvent, value); }
            }
            #endregion

            #region NewSolution
            public static readonly RoutedEvent NewSolutionEvent = EventManager.RegisterRoutedEvent("NewSolution",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler NewSolution
            {
                add { AddHandler(NewSolutionEvent, value); }
                remove { RemoveHandler(NewSolutionEvent, value); }
            }
            #endregion

            #region NewProject
            public static readonly RoutedEvent NewProjectEvent = EventManager.RegisterRoutedEvent("NewProject",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler NewProject
            {
                add { AddHandler(NewProjectEvent, value); }
                remove { RemoveHandler(NewProjectEvent, value); }
            }
            #endregion

            #region ReferenceManager
            public static readonly RoutedEvent ReferenceManagerEvent = EventManager.RegisterRoutedEvent("ReferenceManager",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));
            #endregion

            #region BeforeCompiler
            public static readonly RoutedEvent BeforeCompilerEvent = EventManager.RegisterRoutedEvent("BeforeCompiler",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler BeforeCompiler
            {
                add { AddHandler(BeforeCompilerEvent, value); }
                remove { RemoveHandler(BeforeCompilerEvent, value); }
            }
            #endregion

            #region AfterCompiler
            public static readonly RoutedEvent AfterCompilerEvent = EventManager.RegisterRoutedEvent("AfterCompiler",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler AfterCompiler
            {
                add { AddHandler(AfterCompilerEvent, value); }
                remove { RemoveHandler(AfterCompilerEvent, value); }
            }
            #endregion

            #region SetProject
            public static readonly RoutedEvent SetProjectEvent = EventManager.RegisterRoutedEvent("SetProject",
                RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(MainWindow));

            public event RoutedEventHandler SetProject
            {
                add { AddHandler(SetProjectEvent, value); }
                remove { RemoveHandler(SetProjectEvent, value); }
            }
            #endregion
        
        #endregion

        public MainWindow(string SolutionPath=null)
        {
            InitializeComponent();
            if (!String.IsNullOrEmpty(SolutionPath))
            {
                SolutionExplorer.CurrentSolutionPath = SolutionPath;
            }
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
        {
            CompileSolution(SolutionExplorer.CurrentSolutionPath, 4);
        }

        private void OpenSolutionClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenSolutionEvent));
        }

        private void NewSolutionClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewSolutionEvent));
        }

        private void OpenSolutionByPathListener(object sender, RoutedEventArgs e)
        {
            SolutionExplorer.CurrentSolutionPath = e.OriginalSource.ToString();
        }

        private void NewProjectClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewProjectEvent));
        }
        private void ReferenceManagerClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(ReferenceManagerEvent));
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

        private void LoadPlugins(object sender, RoutedEventArgs e)
        {
            var plugins =
                PluginHost.GetPlugins(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                    @"QStudio", "plugins"));
            foreach(var plugin in plugins)
            {
                var pluginMenuItem = new MenuItem {Header = plugin.Title};
                PluginsMenuItem.Items.Add(pluginMenuItem);
                var objects = new List<object> {pluginMenuItem};
                var toolBar = new ToolBar();
                ToolBarTray.ToolBars.Add(toolBar);
                objects.Add(toolBar);
                objects.AddRange(plugin.InitializeObjects.Select(FindName).Where(_object => _object != null));
                if (plugin is ICompile)
                {
                    AddHandler(BeforeCompilerEvent, new RoutedEventHandler((plugin as ICompile).BeforeCompilerListener));
                    AddHandler(AfterCompilerEvent, new RoutedEventHandler((plugin as ICompile).AfterCompilerListener));
                }
                if (plugin is ISetProjectAndSolution)
                {
                    AddHandler(SetProjectEvent, new RoutedEventHandler((plugin as ISetProjectAndSolution).SetProjectListener));
                }
                plugin.Initialize(objects);
            }
            RegistrateDefaultHandler();
        }

        private void RegistrateDefaultHandler()
        {
            AddHandler(BasicComponentsPack.SolutionExplorer.SelectingFileEvent, new RoutedEventHandler(SelectedFile));
            AddHandler(BasicComponentsPack.SolutionExplorer.SetProjectEvent, new RoutedEventHandler(SetProjectHandler));
            AddHandler(BasicComponentsPack.WorkplaceTabs.ErrorExceptionEvent, new RoutedEventHandler(ErrorHandler));
            AddHandler(BasicComponentsPack.DebugConsole.ErrorExceptionEvent, new RoutedEventHandler(ErrorHandler));
            AddHandler(BasicComponentsPack.DebugConsole.OpenSolutionEvent, new RoutedEventHandler(OpenSolutionByPathListener));
            AddHandler(BasicComponentsPack.DebugConsole.CompileSolutionEvent, new RoutedEventHandler(CompileSolutionListener));
            AddHandler(BasicComponentsPack.SolutionExplorer.ErrorExceptionEvent, new RoutedEventHandler(ErrorHandler));
            AddHandler(SelectingFileEvent, new RoutedEventHandler(WorkplaceTabs.SelectedFileListener));
            AddHandler(SaveAllEvent, new RoutedEventHandler(WorkplaceTabs.SaveAllListener));
            AddHandler(CloseSolutionEvent, new RoutedEventHandler(WorkplaceTabs.CloseSolutionListener));
            AddHandler(CloseSolutionEvent, new RoutedEventHandler(SolutionExplorer.CloseSolutionListener));
            AddHandler(OpenSolutionEvent, new RoutedEventHandler(SolutionExplorer.OpenSolutionListener));
            AddHandler(NewSolutionEvent, new RoutedEventHandler(SolutionExplorer.NewSolutionListener));
            AddHandler(AfterCompilerEvent, new RoutedEventHandler(SolutionExplorer.AfterCompilerListener));
            AddHandler(ReferenceManagerEvent, new RoutedEventHandler(SolutionExplorer.ReferenceManagerListener));
            AddHandler(BeforeCompilerEvent, new RoutedEventHandler(WorkplaceTabs.BeforeCompilerListener));
            AddHandler(AfterCompilerEvent, new RoutedEventHandler(WorkplaceTabs.AfterCompilerListener));
            AddHandler(NewProjectEvent, new RoutedEventHandler(SolutionExplorer.NewProjectListener));
            AddHandler(ErrorEvent, new RoutedEventHandler(DebugConsole.ErrorListener));
        }

        private void CompileSolutionListener(object sender, RoutedEventArgs e)
        {
            CompileSolution(SolutionExplorer.CurrentSolutionPath, ulong.Parse(e.OriginalSource.ToString()));
        }

        private void CompileSolution(string path = null, ulong maxCPU = 0)
        {
            if (!String.IsNullOrEmpty(path))
            {
                RaiseEvent(new RoutedEventArgs(BeforeCompilerEvent));
                var p = new Process();
                var startupstring = new StringBuilder(" ");
                startupstring.Append(path).Append(" ").Append(maxCPU);

                p.StartInfo.FileName = @"Compiler.exe";
                p.StartInfo.Arguments = startupstring.ToString();
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                p.StartInfo.CreateNoWindow = true;
                p.OutputDataReceived +=
                    (sender, e) =>
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Send, (ThreadStart) (() =>
                            RaiseEvent(new RoutedEventArgs(ErrorEvent, e.Data))));
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
                RaiseEvent(new RoutedEventArgs(AfterCompilerEvent));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(ErrorEvent, String.Format("No availible {0}", path)));
            }
        }

        private void SetProjectHandler(object sender, RoutedEventArgs e)
        {
            ProjectItem.Visibility = !String.IsNullOrEmpty(e.OriginalSource.ToString()) ? Visibility.Visible : Visibility.Collapsed;
            DebugItem.Visibility = !String.IsNullOrEmpty(e.OriginalSource.ToString()) ? Visibility.Visible : Visibility.Collapsed;
            RaiseEvent(new RoutedEventArgs(SetProjectEvent, e.OriginalSource.ToString()));
        }

        private void ShowDebugSettings(object sender, RoutedEventArgs e)
        {
            var debugSettings = new DebugSettings();
            debugSettings.Show();
        }

        private void AboutClick(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow {Title = "About QStudio", Image = new System.Windows.Shapes.Path
            {
                Data = FindResource("Icon") as Geometry,
                Fill = FindResource("ErrorBrush") as SolidColorBrush
            }};
            about.ShowDialog();
        }
    }
}
