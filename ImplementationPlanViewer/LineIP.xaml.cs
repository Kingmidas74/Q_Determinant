
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for LineIP.xaml
    /// </summary>
    public partial class LineIP
    {
        private readonly VisualLinkVM _vlvm = new VisualLinkVM();
        public LineIP()
        {
            InitializeComponent();
            DataContext = _vlvm;
        }
    }
}
