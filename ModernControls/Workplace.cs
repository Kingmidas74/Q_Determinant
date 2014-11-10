using System;
using System.Linq;
using Core;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using ModernControls.InternalClasses;
using ModernControls.InternalClasses.Dialogs;

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

        private void SaveAllMenuItemClick(object sender, RoutedEventArgs e)
        {
            var tabControl = (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl);
            foreach (ExtendedTabItem tab in tabControl.Items.Cast<ExtendedTabItem>().Where(tab => tab.Content is IEditable))
            {
                (tab.Content as IEditable).SaveFile();
            }
        }

        private void SaveMenuItemClick(object sender, RoutedEventArgs e)
        {
            var tabItem = ((GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).SelectedItem as ExtendedTabItem);
            if (tabItem.Content is IEditable)
            {
                (tabItem.Content as IEditable).SaveFile();
            }
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("OpenSolutionMenuItem") as MenuItem).Click += OpenSolutionMenuItemClick;
            (GetTemplateChild("EFCMenuItem") as MenuItem).Click += EFCMenuItemClick;
            (GetTemplateChild("SAIFCMenuItem") as MenuItem).Click += SAIFCMenuItemClick;
            (GetTemplateChild("SAIIPMenuItem") as MenuItem).Click += SAIIPMenuItemClick;
            (GetTemplateChild("NewProjectMenuItem") as MenuItem).Click += NewProjectMenuItemClick;
            //(GetTemplateChild("EQDMenuItem") as MenuItem).Click += EQDMenuItemClick;
            (GetTemplateChild("EIPMenuItem") as MenuItem).Click += EIPMenuItemClick;
            (GetTemplateChild("CloseMenuItem") as MenuItem).Click += CloseMenuItemClick;
            (GetTemplateChild("AboutMenuItem") as MenuItem).Click += AboutMenuItemClick;
            (GetTemplateChild("SaveAllMenuItem") as MenuItem).Click += SaveAllMenuItemClick;
            (GetTemplateChild("SaveMenuItem") as MenuItem).Click += SaveMenuItemClick;
            (GetTemplateChild("Compiler") as Button).Click += CompilerClick;
            var writeDelegate = new WriteLogsDelegate(WriteLog);
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).SetLogsDelegate(writeDelegate);
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).SetLogsDelegate(writeDelegate);
            (GetTemplateChild("ElementList") as ListBox).SelectionChanged += SelectedBlockTypeClick;
            base.OnApplyTemplate();
        }

        private void SAIIPMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).SAIIP();
        }

        private void SAIFCMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).SAIFC();
        }

        private void NewProjectMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).AddProject();
        }
        
        /*private void IIPMenuItemClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(QDClickEvent, this));
        }

        private void IQDMenuItemClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(IQDClickEvent, this));
        }*/

        private void EFCMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).ExportFC();
        }

        private void EQDMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).ExportQD();
        }

        private void EIPMenuItemClick(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).ExportIP();
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
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).CurrentProjectPath =
                (args.OriginalSource as ExtendedTreeViewItem).ProjectPath;
            (GetTemplateChild("WorkPlaceTabs") as ExtendedTabControl).AddTab(args.OriginalSource as ExtendedTreeViewItem);
        }

        private void OpenSolutionMenuItemClick(object sender, RoutedEventArgs e)
        {
            var dialog = DialogFactory.CallOpenDialog(DialogTypes.Solution);
            var result = dialog.ShowDialog();
            if (result == true)
            {
                (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution(dialog.FileName);
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
            //p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
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
