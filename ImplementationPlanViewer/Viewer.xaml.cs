using System.IO;
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Viewer : VisualCore.ITabContent
    {
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

        public void ReLoad()
        {
            _vvm.Reload();
        }
    }
}
