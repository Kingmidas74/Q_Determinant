using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BasicComponentsPack.Annotations;
using Core.Serializers.SerializationModels;
using Core.Serializers.SerializationModels.ProjectModels;

namespace BasicComponentsPack.InternalClasses
{
    public class ReferenceManagerVM:INotifyPropertyChanged
    {

        ObservableCollection<ReferenceGridViewItem> _referencesCollection = new ObservableCollection<ReferenceGridViewItem>();

        public ObservableCollection<ReferenceGridViewItem> ReferencesCollection
        {
            get
            {
                return _referencesCollection;
            }
            set
            {
                _referencesCollection = value;
                OnPropertyChanged();
            }
        }

        private string _currentProjectPath = string.Empty;

        public string CurrentProjectPath
        {
            get
            {
                return _currentProjectPath;
            }
            set
            {
                _currentProjectPath = value;
                UpdateReferenceCollection(value);
                OnPropertyChanged();
            }
        }

        private string _currentSolutionPath = string.Empty;

        public string CurrentSolutionPath
        {
            get
            {
                return _currentSolutionPath;
            }
            set
            {
                _currentSolutionPath = value;
                CurrentProjectPath = string.Empty;
                OnPropertyChanged();
            }
        }

        private void UpdateReferenceCollection(string projectPath)
        {
            ReferencesCollection.Clear();
            if(string.IsNullOrEmpty(projectPath)) return;
            var currentProject = DeserializeProject(CurrentProjectPath);

            foreach (var reference in currentProject.References)
            {
                ReferencesCollection.Add(new ReferenceGridViewItem
                {
                    Include = true,
                    Path = reference.ProjectPath,
                    Title = reference.ProjectTitle
                });
            }

            var globalReferenceDirectory = new DirectoryInfo("BasicFunctions");
            foreach (var referenceDirectory in globalReferenceDirectory.GetDirectories())
            {
                foreach (var file in referenceDirectory.GetFiles().Where(file => file.Extension.Equals(".qpr")))
                {
                    var referenceProject = DeserializeProject(file.FullName);
                    if (!ReferenceContainsProjectByPath(file.FullName))
                    {
                        ReferencesCollection.Add(new ReferenceGridViewItem
                        {
                            Path = file.FullName,
                            Title = referenceProject.Title,
                            Include = false
                        });
                    }
                    break;
                }
            }

            var currentSolution = Core.Serializers.SerializationModels.SolutionModels.Solution.Deserialize(CurrentSolutionPath);
            foreach (var function in currentSolution.Projects.Where(function => function.Type == ProjectTypes.Function && !function.Title.Equals(System.IO.Path.GetFileNameWithoutExtension(CurrentProjectPath))).Where(function => ReferenceContainsProjectByPath(function.Path) == false))
            {
                ReferencesCollection.Add(new ReferenceGridViewItem
                {
                    Path = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), function.Path),
                    Title = function.Title,
                    Include = false
                });
            }
        }

        private bool ReferenceContainsProjectByPath(string referenceProjectPath)
        {
            return ReferencesCollection.Any(reference => reference.Path.Equals(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), referenceProjectPath)));
        }

        private Core.Serializers.SerializationModels.ProjectModels.Project DeserializeProject(string pathToProject)
        {
            return Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(pathToProject);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void UpdateReferencesToProject()
        {
            var currentProject = DeserializeProject(CurrentProjectPath);
            currentProject.References.Clear();
            foreach (var reference in ReferencesCollection)
            {
                if (reference.Include)
                {
                    currentProject.AddReference(new Reference
                    {
                        ProjectPath = reference.Path,
                        ProjectTitle = reference.Title
                    });
                }
            }
            currentProject.Serialize(CurrentProjectPath);
        }
    }
}
