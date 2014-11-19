using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DefaultControlsPack
{
    internal static class LocalExtensions
    {
        public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
        {
            var window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as ModernWindow;
            if (window != null) action(window);
        }
    }

    public class ModernWindow : Window
    {
        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow), new FrameworkPropertyMetadata(typeof(ModernWindow)));
        }
        public override void OnApplyTemplate()
        {
            (GetTemplateChild("CloseButton") as Button).Click += CloseButtonClick;
            (GetTemplateChild("MinButton") as Button).Click += MinButtonClick;
            (GetTemplateChild("MaxButton") as Button).Click += MaxButtonClick;
            var border = (Border)GetTemplateChild("TitleBar");
            border.MouseLeftButtonDown += TitleBarMouseLeftButtonDown;
            border.MouseMove += TitleBarMouseMove;
            base.OnApplyTemplate(); 
        }


        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.Close());
        }

        void MinButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }

        void MaxButtonClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = (w.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
        }

        void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1 && ResizeMode!=ResizeMode.NoResize && ResizeMode!=ResizeMode.CanMinimize)
            {
                MaxButtonClick(sender, e);
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w => w.DragMove());
            }
        }

        void TitleBarMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                sender.ForWindowFromTemplate(w =>
                {
                    if (w.WindowState == WindowState.Maximized)
                    {
                        w.BeginInit();
                        double adjustment = 40.0;
                        var mouse1 = e.MouseDevice.GetPosition(w);
                        var width1 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                        w.WindowState = WindowState.Normal;
                        var width2 = Math.Max(w.ActualWidth - 2 * adjustment, adjustment);
                        w.Left = (mouse1.X - adjustment) * (1 - width2 / width1);
                        w.Top = -7;
                        w.EndInit();
                        w.DragMove();
                    }
                });
            }
        }

        
    }
}
