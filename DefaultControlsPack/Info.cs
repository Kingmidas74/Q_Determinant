using System.Windows;
using System.Windows.Controls;

namespace DefaultControlsPack
{
    public class Info : ContentControl
    {
        #region Image
        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(object), typeof(Info),
                new FrameworkPropertyMetadata());

        public object Image
        {
            get { return (object)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        #endregion

        #region InfoText
        public static readonly DependencyProperty InfoTextProperty =
            DependencyProperty.Register("InfoText", typeof(object), typeof(Info),
                new FrameworkPropertyMetadata());

        public object InfoText
        {
            get { return (object)GetValue(InfoTextProperty); }
            set { SetValue(InfoTextProperty, value); }
        }
        #endregion
        static Info()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Info), new FrameworkPropertyMetadata(typeof(Info)));
        }
        
    }
}
