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
using System.Windows.Shapes;
using Core.Serializers;
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
            DialogResult = true;
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            Core.Serializers.SerializationModels.ProjectModels.Project currentProject;
            SerializersFactory.GetSerializer().DeserializeProject(CurrentProjectPath, out currentProject);
            MessageBox.Show(currentProject.References.Count.ToString());
        }
    }
}
