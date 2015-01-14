using System.IO;
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Viewer : VisualCore.ISaveable,VisualCore.ITabContent
    {
        private string _originalFileName;
        readonly ViewerVM _vvm = new ViewerVM();
        public Viewer()
        {
            InitializeComponent();
            DataContext = _vvm;
        }



        public void SetContent(FileInfo file)
        {
          _vvm.SetContent(file);
        }
       

        public bool IsChange
        {
            get { return false; }
        }

        public void Save()
        {
            
        }

        public void ReLoad()
        {
            var file = new FileInfo(_originalFileName);
            SetContent(file);
        }
    }
}
