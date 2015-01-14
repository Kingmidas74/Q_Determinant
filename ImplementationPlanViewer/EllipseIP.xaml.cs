using System.Windows;
using ImplementationPlanViewer.InternalClasses;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for EllipseIP.xaml
    /// </summary>
    public partial class EllipseIP
    {
        private readonly VisualBlockVM _vbvm = new VisualBlockVM();

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(EllipseIP),
                new FrameworkPropertyMetadata());


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        public EllipseIP()
        {
            InitializeComponent();
            DataContext = _vbvm;
            IsSelected = false;
        }

        public double GetWidth()
        {
            return Element.Width;
        }
        public double GetHeight()
        {
            return Element.Height;
        }

        private void ClickToEllipse(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _vbvm.IsSelected = !_vbvm.IsSelected;
        }
    }
}
