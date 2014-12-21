using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CodeGeneration.Enums;
using Core.Atoms;
using Core.Converters;
using Core.Serializers;
using Core.Serializers.SerializationModels.SolutionModels;

namespace CodeGeneration.InternalClasses
{
    public class CGViewModel:INotifyPropertyChanged
    {
        public string CurrentSolutionPath { get; set; }
        public string PathToPlan { get; set; }
        ObservableCollection<Project> _projectsCollection = new ObservableCollection<Project>();
        public ObservableCollection<Project> ProjectsCollection
        {
            get
            {
                return _projectsCollection;
            }
            set
            {
                _projectsCollection = value;
                OnPropertyChanged("ProjectsCollection");
            }
        }

        private int _currentProjectIndex=-1;
        public int CurrentProjectIndex
        {
            get
            {
                return _currentProjectIndex;
            }
            set
            {
                _currentProjectIndex = value;
                PlansCollection.Clear();
                CurrentPlanIndex = -1;
                var projectPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath), ProjectsCollection[value].Path);
                var project = Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(projectPath);
                foreach (var file in project.Files.Where(x => System.IO.Path.GetExtension(x.Path).Equals(".ip")))
                {
                    PlansCollection.Add(file);
                }
                if (PlansCollection.Count > 0)
                {
                    CurrentPlanIndex = 0;
                }
                OnPropertyChanged("CurrentProjectIndex");
            }
        }

        ObservableCollection<Core.Serializers.SerializationModels.ProjectModels.File> _plansCollection = new ObservableCollection<Core.Serializers.SerializationModels.ProjectModels.File>();
        public ObservableCollection<Core.Serializers.SerializationModels.ProjectModels.File> PlansCollection
        {
            get
            {
                return _plansCollection;
            }
            set
            {
                _plansCollection = value;
                OnPropertyChanged("PlansCollection");
            }
        }

        private int _currentPlanIndex = -1;
        public int CurrentPlanIndex
        {
            get
            {
                return _currentPlanIndex;
            }
            set
            {
                _currentPlanIndex = value;
                if (value > -1)
                {
                    PathToPlan = PlansCollection[value].Path;
                    var plan = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(
                        System.IO.Path.Combine(System.IO.Path.GetDirectoryName(CurrentSolutionPath),System.IO.Path.GetDirectoryName(ProjectsCollection[CurrentProjectIndex].Path), PathToPlan)
                        ), ConverterFormats.JSON);
                    Variables.Clear();
                    foreach (var vertex in plan.Vertices.FindAll(x => x.Level == 0))
                    {
                        Variables.Add(new Variable {Title = vertex.Content, Type = VariableTypes.String});
                    }
                }
                OnPropertyChanged("CurrentPlanIndex");
            }
        }


        ObservableCollection<Variable> _variables = new ObservableCollection<Variable>();
        public ObservableCollection<Variable> Variables
        {
            get { return _variables; }
            set 
            { 
                _variables = value; 
                OnPropertyChanged("Variables"); 
            }
        }

        ObservableCollection<string> _languageCollection = new ObservableCollection<string>();
        public ObservableCollection<string> LanguageCollection
        {
            get { return _languageCollection; }
            set
            {
                _languageCollection = value;
                OnPropertyChanged("LanguageCollection");
            }
        }

        private string _outputName = string.Empty;
        public string OutputName
        {
            get
            {
                return _outputName;
            }
            set
            {
                _outputName = value;
                OnPropertyChanged("OutputName");
            }
        }

        private int _currentLanguageIndex = -1;
        public int CurrentLanguageIndex
        {
            get
            {
                return _currentLanguageIndex;
            }
            set
            {
                _currentLanguageIndex = value;
                OnPropertyChanged("CurrentPlanIndex");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
