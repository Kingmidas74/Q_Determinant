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
        readonly ReferenceManagerVM _rvvm = new ReferenceManagerVM();
        public ReferenceManager()
        {
            InitializeComponent();
            DataContext = _rvvm;
        }

        private string _currentProjectPath;
        public string CurrentProjectPath
        {
            get { return _currentProjectPath; }
            set { 
                _rvvm.CurrentProjectPath = value;
                _currentProjectPath = value;
            }
        }

        private string _currentSolutionPath;
        public string CurrentSolutionPath
        {
            get { return _currentSolutionPath; }
            set
            {
                _rvvm.CurrentSolutionPath = value;
                _currentSolutionPath = value;
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {  
            _rvvm.UpdateReferencesToProject();
            DialogResult = true;
        }
    }
}
