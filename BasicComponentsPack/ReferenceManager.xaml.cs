using System.Windows;
using BasicComponentsPack.InternalClasses;
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
