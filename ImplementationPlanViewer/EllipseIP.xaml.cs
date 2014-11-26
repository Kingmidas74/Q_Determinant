using System.Windows.Controls;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for EllipseIP.xaml
    /// </summary>
    public partial class EllipseIP : UserControl
    {
        public EllipseIP()
        {
            InitializeComponent();
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
