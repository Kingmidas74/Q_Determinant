using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Microsoft.Win32;
using ModernControls.InternalClasses;

namespace ModernControls
{
    public delegate void WriteLogsDelegate(string Message, LogType Type = LogType.Default);
    public class Workplace : Control
    {
        public DebugConsole _debugConsole;
        public static readonly RoutedEvent AboutClickEvent = EventManager.RegisterRoutedEvent("AboutClick",
             RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtendedTabItem));

        public event RoutedEventHandler CloseTab
        {
            add { AddHandler(AboutClickEvent, value); }
            remove { RemoveHandler(AboutClickEvent, value); }
        }
        void AboutMenuItemClick(object sender, RoutedEventArgs e)
        {
            this.RaiseEvent(new RoutedEventArgs(AboutClickEvent, this));
        }

        public Workplace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Workplace), new FrameworkPropertyMetadata(typeof(Workplace)));
            AddHandler(ExtendedTreeViewItem.OpenDocumentEvent, new RoutedEventHandler(OpenDocument));
        }

        private void OpenDocument(object source, RoutedEventArgs args)
        {
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).AddTab(args.OriginalSource as ExtendedTreeViewItem);
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("OpenSolutionMenuItem") as MenuItem).Click += new RoutedEventHandler(OpenSolutionMenuItemClick);
            (GetTemplateChild("CloseMenuItem") as MenuItem).Click += new RoutedEventHandler(CloseMenuItemClick);
            (GetTemplateChild("AboutMenuItem") as MenuItem).Click += new RoutedEventHandler(AboutMenuItemClick);
            (GetTemplateChild("Compiler") as Button).Click += new RoutedEventHandler(CompilerClick);
            _debugConsole = (GetTemplateChild("DebugConsole") as DebugConsole);
            var WriteDelegate = new WriteLogsDelegate(WriteLog);
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).SetLogsDelegate(WriteDelegate);
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).SetLogsDelegate(WriteDelegate);
            base.OnApplyTemplate();
        }

        
        private void OpenSolutionMenuItemClick(object sender, RoutedEventArgs e)
        {            
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".qsln";
            dlg.Filter = "SolutionFiles (*.qsln)|*.qsln"; 
            var result = dlg.ShowDialog();
            if (result == true)
            {
                (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution(dlg.FileName);
            }
        }

        private void CloseMenuItemClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        
        private void CompilerClick(object sender, RoutedEventArgs e)
        {
            var currentSolutionPath = (GetTemplateChild("SolutionTree") as ExtendedTreeView).CurrentSolutionPath;
            var p = new Process();
            var startupstring = new StringBuilder("");
            startupstring.Append(" -s ")
                .Append(currentSolutionPath)
                .Append(" -ip ");
            p.StartInfo.FileName = "Compiler.exe";
            p.StartInfo.Arguments = startupstring.ToString();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            //pr.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(866);
            p.StartInfo.CreateNoWindow = true;
            p.OutputDataReceived += new DataReceivedEventHandler(WriteToLog);
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
           // WriteLog(Logs.AllDebugInfo);
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution();
        }

        private void WriteToLog(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //Logs.Instance.WriteLog(outLine.Data,LogType.Default);
        }

        public void WriteLog(string message, LogType type=LogType.Default)
        {
            _debugConsole.WriteLog(message, type);
        }

        
    }
}
