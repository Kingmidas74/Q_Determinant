using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using DefaultControlsPack;
using PluginController;
using System.Diagnostics;
using VisualCore.Events;

namespace QStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow:ModernWindow
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
            AddHandler(OpenSolutionEvent, new RoutedEventHandler(SolutionExplorer.OpenSolutionListener));
            AddHandler(NewSolutionEvent, new RoutedEventHandler(SolutionExplorer.NewSolutionListener));
            AddHandler(AfterCompilerEvent, new RoutedEventHandler(SolutionExplorer.AfterCompilerListener));
            AddHandler(ReferenceManagerEvent, new RoutedEventHandler(SolutionExplorer.ReferenceManagerListener));
            AddHandler(BeforeCompilerEvent, new RoutedEventHandler(WorkplaceTabs.BeforeCompilerListener));
            AddHandler(AfterCompilerEvent, new RoutedEventHandler(WorkplaceTabs.AfterCompilerListener));
            AddHandler(NewProjectEvent, new RoutedEventHandler(SolutionExplorer.NewProjectListener));
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
        {
            if (!String.IsNullOrEmpty(SolutionExplorer.CurrentSolutionPath))
            {
                RaiseEvent(new RoutedEventArgs(BeforeCompilerEvent));
                var currentSolutionPath = SolutionExplorer.CurrentSolutionPath;
                var p = new Process();
                var startupstring = new StringBuilder(" ");
                startupstring.Append(currentSolutionPath).Append(" ").Append(4);

                p.StartInfo.FileName = "Compiler.exe";
                p.StartInfo.Arguments = startupstring.ToString();
                //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
                p.StartInfo.CreateNoWindow = true;
                //p.OutputDataReceived += WriteToLog;
                p.Start();
                p.BeginOutputReadLine();
                p.WaitForExit();
                // WriteLog(_compilerResultString.ToString());
                RaiseEvent(new RoutedEventArgs(AfterCompilerEvent));
            }
        }

        private void OpenSolutionClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenSolutionEvent));
        }

        private void NewSolutionClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NewSolutionEvent));
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
            var pluginController = new PluginHost("plugins", "PluginController.IPlugin");
            var plugins = pluginController.Plugins;
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
                plugin.Initialize(objects);
            }
        }

        private void ShowDebugSettings(object sender, RoutedEventArgs e)
        {
            var debugSettings = new DebugSettings();
            debugSettings.Show();
        }
    }
}
