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
using BasicComponentsPack.InternalClasses;
using Core.Serializers;
using Core.Serializers.SerializationModels.SolutionModels;
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
                var solution = new Core.Serializers.SerializationModels.SolutionModels.Solution();
                var serializer = Factory.GeSerializer();
                serializer.DeserializeSolution(CurrentSolutionPath, out solution);
                var result = new SolutionTreeItem();
                result.FilePath = CurrentSolutionPath;
                result.Title = solution.Title;
                foreach (var project in solution.Projects)
                {
                    var currentProject = new SolutionTreeItem();
                    currentProject.Title = project.Title;
                    currentProject.FilePath = project.Path;
                    result.Items.Add(currentProject);
                }
                var solutions = new List<Solution>() {solution};
                SearchTreeView.ItemsSource = null;
                SearchTreeView.ItemsSource = solutions;
            }
        }

        public SolutionExplorer()
        {
            InitializeComponent();
        }
    }
}
