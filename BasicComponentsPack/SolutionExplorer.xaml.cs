using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using DefaultControlsPack;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for SolutionExplorer.xaml
    /// </summary>
    public partial class SolutionExplorer : UserControl
    {
        #region SelectingFile
        public static readonly RoutedEvent SelectingFileEvent = EventManager.RegisterRoutedEvent("SelectingFile",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(SolutionExplorer));

        public event RoutedEventHandler SelectingFile
        {
            add { AddHandler(SelectingFileEvent, value); }
            remove { RemoveHandler(SelectingFileEvent, value); }
        }
        #endregion
        
        private string _currentSolutionPath = String.Empty;

        public string CurrentSolutionPath
        {
            get { return _currentSolutionPath; }
            set
            {
                _currentSolutionPath = value;
                RefreshSolution();
            }
        }

        private void RefreshSolution()
        {
            if (!String.IsNullOrEmpty(CurrentSolutionPath))
            {
                MessageBox.Show(CurrentSolutionPath);
            }
        }
        public SolutionExplorer()
        {
            InitializeComponent();
        }

        private void SelectedItem(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            this.RaiseEvent(new RoutedEventArgs(SelectingFileEvent,(e.NewValue as IconTreeViewItem).Tag.ToString()));
        }


        
    }
}
