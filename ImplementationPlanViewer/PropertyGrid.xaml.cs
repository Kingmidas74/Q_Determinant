using System.Windows.Controls;
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for PropertyGrid.xaml
    /// </summary>
    public partial class PropertyGrid : UserControl
    {
        private readonly PropertyGridVM _pgvm=new PropertyGridVM();
        public PropertyGrid()
        {
            InitializeComponent();
            DataContext = _pgvm;
        }

        internal void SetFilePath(System.IO.FileInfo file)
        {
            _pgvm.CurrentFile = file;
        }
    }

    
}
