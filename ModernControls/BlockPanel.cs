using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    public class BlockPanel : ContentControl
    {
        public static readonly DependencyProperty TitleProperty =
    DependencyProperty.Register("Title", typeof(string), typeof(BlockPanel), new FrameworkPropertyMetadata());

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        static BlockPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BlockPanel), new FrameworkPropertyMetadata(typeof(BlockPanel)));
        }

        
    }
}
