using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for EllipseIP.xaml
    /// </summary>
    public partial class EllipseIP
    {
        private readonly VisualBlockVM _vbvm = new VisualBlockVM();
        public EllipseIP()
        {
            InitializeComponent();
            DataContext = _vbvm;
        }

        public void SetContent(string content)
        {
            Content.Text = content;
        }

        public void SetLevel(ulong level)
        {
            Level.Text = level.ToString();
        }

        public void SetNumber(int number)
        {
            Number.Text = number.ToString();
        }

        public double GetWidth()
        {
            return Element.Width;
        }
        public double GetHeight()
        {
            return Element.Height;
        }
    }
}
