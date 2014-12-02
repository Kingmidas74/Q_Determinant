using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Shapes;
using BasicComponentsPack.InternalClasses;
using Core.Serializers;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.ProjectModels;
using DefaultControlsPack;

namespace BasicComponentsPack
{
    /// <summary>
    /// Interaction logic for ReferenceManager.xaml
    /// </summary>
    public partial class ReferenceManager : ModernWindow
    {
        public ReferenceManager()
        {
            InitializeComponent();
            ReferenceCollection = new List<Reference>();
        }

        public string CurrentProjectPath
        {
            get; set;
        }

        public string CurrentSolutionPath
        {
            get;
            set;
        }

        public List<Reference> ReferenceCollection { get; private set; } 

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            ReferenceCollection.Clear();
            ReferenceGrid.UnselectAllCells();
            ReferenceGrid.SelectedIndex++;
            foreach (var item in ReferenceGrid.Items)
            {
                var _item = item as ReferenceGridViewItem;
                Debug.WriteLine(_item.Include,"INCLUDE");
                if (_item.Include)
                {
                    ReferenceCollection.Add(new Reference
                    {
                        ProjectPath = _item.Path,
                        ProjectTitle = _item.Title
                    });
                }
                
            }
            Debug.WriteLine(ReferenceCollection.Count);
            foreach (var reference in ReferenceCollection)
            {
                Debug.WriteLine(reference.ProjectTitle,"TITLE: ");
            }
            DialogResult = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Core.Serializers.SerializationModels.ProjectModels.Project currentProject;
            SerializersFactory.GetSerializer().DeserializeProject(CurrentProjectPath, out currentProject);
            var ReferenceView = currentProject.References.Select(reference => new ReferenceGridViewItem
            {
                Include = true, Path = reference.ProjectPath, Title = reference.ProjectTitle
            }).ToList();

            var globalReferenceDirectory = new DirectoryInfo("BasicFunctions");
            foreach (var referenceDirectory in globalReferenceDirectory.GetDirectories())
            {
                foreach (var file in referenceDirectory.GetFiles().Where(file => file.Extension.Equals(".qpr")))
                {
                    Core.Serializers.SerializationModels.ProjectModels.Project referenceProject;
                    SerializersFactory.GetSerializer().DeserializeProject(file.FullName, out referenceProject);
                    if (!ReferenceContainsProjectByPath(ReferenceView, file.FullName))
                    {
                        ReferenceView.Add(new ReferenceGridViewItem
                        {
                            Path = file.FullName,
                            Title = referenceProject.Title,
                            Include = false
                        });
                    }
                    break;
                }
            }

            Core.Serializers.SerializationModels.SolutionModels.Solution currentSolution;
            SerializersFactory.GetSerializer().DeserializeSolution(CurrentSolutionPath, out currentSolution);
            foreach (var function in currentSolution.Projects.Where(function => function.Type == ProjectTypes.Function && !function.Title.Equals(System.IO.Path.GetFileNameWithoutExtension(CurrentProjectPath))).Where(function => ReferenceContainsProjectByPath(ReferenceView, function.Path) == false))
            {
                ReferenceView.Add(new ReferenceGridViewItem
                {
                    Path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), function.Path), Title = function.Title, Include = false
                });
            }
            ReferenceGrid.ItemsSource = null;
            ReferenceGrid.ItemsSource = ReferenceView;
        }

        private bool ReferenceContainsProjectByPath(List<ReferenceGridViewItem> references, string referenceProjectPath)
        {
            return references.Any(reference => reference.Path.Equals(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), referenceProjectPath)));
        }


        private bool ReferenceContainsProject(List<ReferenceGridViewItem> references,
            Core.Serializers.SerializationModels.SolutionModels.Project project)
        {
            return references.Any(reference => reference.Path.Equals(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), project.Path)));
        }
    }
}
