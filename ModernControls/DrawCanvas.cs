using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Core;
using ModernControls.InternalClasses;

namespace ModernControls
{
    public class DrawCanvas : Canvas
    {
        public DrawCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DrawCanvas), new FrameworkPropertyMetadata(typeof(DrawCanvas)));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (Equals(e.Source, this))
            {
                MessageBox.Show(Singleton.CurrentBlockType.ToString());
                Focus();
                e.Handled = true;
            }
        }

        
    }
}
