using System;
using Core;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ModernControls.InternalClasses;

namespace ModernControls
{
    public delegate void WriteLogsDelegate(string message, LogType type = LogType.Default);
    public class Workplace : Control
    {
        private StringBuilder _compilerResultString;
        public static readonly RoutedEvent AboutClickEvent = EventManager.RegisterRoutedEvent("AboutClick",
             RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtendedTabItem));
        public event RoutedEventHandler AboutClick
        {
            add { AddHandler(AboutClickEvent, value); }
            remove { RemoveHandler(AboutClickEvent, value); }
        }
        

        public Workplace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Workplace), new FrameworkPropertyMetadata(typeof(Workplace)));
            AddHandler(ExtendedTreeViewItem.OpenDocumentEvent, new RoutedEventHandler(OpenDocument));
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("OpenSolutionMenuItem") as MenuItem).Click += OpenSolutionMenuItemClick;
            (GetTemplateChild("CloseMenuItem") as MenuItem).Click += CloseMenuItemClick;
            (GetTemplateChild("AboutMenuItem") as MenuItem).Click += AboutMenuItemClick;
            (GetTemplateChild("Compiler") as Button).Click += CompilerClick;
            var writeDelegate = new WriteLogsDelegate(WriteLog);
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).SetLogsDelegate(writeDelegate);
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).SetLogsDelegate(writeDelegate);
            (GetTemplateChild("ElementList") as ListBox).SelectionChanged += SelectedBlockTypeClick;
            base.OnApplyTemplate();
        }

        private void SelectedBlockTypeClick(object sender, SelectionChangedEventArgs e)
        {
            Singleton.CurrentBlockType =
                (BlockTypes) Enum.Parse(typeof (BlockTypes), (sender as ListBox).SelectedItem.ToString());
        }

        void AboutMenuItemClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AboutClickEvent, this));
        }
        

        private void OpenDocument(object source, RoutedEventArgs args)
        {
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).AddTab(args.OriginalSource as ExtendedTreeViewItem);
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

        private void WriteToLog(object sendingProcess, DataReceivedEventArgs outLine)
        {
            _compilerResultString.AppendLine(outLine.Data);
        }
        
        private void CompilerClick(object sender, RoutedEventArgs e)
        {
            _compilerResultString = new StringBuilder("");
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
            p.OutputDataReceived += WriteToLog;
            p.Start();
            p.BeginOutputReadLine();
            p.WaitForExit();
            WriteLog(_compilerResultString.ToString());
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution();
        }

        public void WriteLog(string message, LogType type=LogType.Default)
        {
            (GetTemplateChild("DebugConsole") as DebugConsole).WriteLog(message, type);
        }

        
    }
}
