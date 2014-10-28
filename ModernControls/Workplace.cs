using System;
using System.Collections.Generic;
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
    public class Workplace : Control
    {
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
            (GetTemplateChild("Compiler") as Button).Click += new RoutedEventHandler(CompilerClick);
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

        private void CompilerClick(object sender, RoutedEventArgs e)
        {
            var currentSolutionPath = (GetTemplateChild("SolutionTree") as ExtendedTreeView).CurrentSolutionPath;
            var p = new System.Diagnostics.Process();
            var startupstring = new StringBuilder("");
            startupstring.Append(" -s ")
                .Append(currentSolutionPath)
                .Append(" -ip ");
            p.StartInfo.FileName = "Compiler.exe";
            p.StartInfo.Arguments = startupstring.ToString();
            p.Start();
            p.WaitForExit();
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution();
        }
    }
}
