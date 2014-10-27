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
    ///     <MyNamespace:Workplace/>
    ///
    /// </summary>
    public class Workplace : Control
    {
        public Workplace()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Workplace), new FrameworkPropertyMetadata(typeof(Workplace)));
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
                .Append(" -ip -o ")
                .Append(System.IO.Path.GetDirectoryName(currentSolutionPath))
                .Append(@"\Operations.json");
            MessageBox.Show(startupstring.ToString());
            p.StartInfo.FileName = "Compiler.exe";
            p.StartInfo.Arguments = startupstring.ToString();
            p.Start();
            p.WaitForExit();
            (GetTemplateChild("SolutionTree") as ExtendedTreeView).RefreshSolution();
        }
    }
}
