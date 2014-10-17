﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModernControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls;assembly=ModernControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:MWindow/>
    ///
    /// </summary>
    internal static class LocalExtensions
    {
        public static void ForWindowFromChild(this object childDependencyObject, Action<Window> action)
        {
            var element = childDependencyObject as DependencyObject;
            while (element != null)
            {
                element = VisualTreeHelper.GetParent(element);
                if (element is Window) { action(element as Window); break; }
            }
        }

        public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
        {
            Window window = ((FrameworkElement)templateFrameworkElement).TemplatedParent as Window;
            if (window != null) action(window);
        }

        public static IntPtr GetWindowHandle(this Window window)
        {
            WindowInteropHelper helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }
    
    public class MWindow : Window
    {
        
        static MWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MWindow), new FrameworkPropertyMetadata(typeof(MWindow)));
        }

        public override void OnApplyTemplate()
        {
            /*var btn = GetTemplateChild("CloseButton") as Button;
            btn.MouseLeftButtonDown += CloseButtonClick;*/
            var b = (Button)GetTemplateChild("MinButton");
            b.MouseLeftButtonDown += MinButtonClick;
            var _border = (Border)GetTemplateChild("TitleBar");
            _border.MouseLeftButtonDown += TitleBarMouseLeftButtonDown;
            _border.MouseMove += TitleBarMouseMove;

            ((Button)GetTemplateChild("SettingsButton")).MouseLeftButtonDown += ShowHideAsideClick;
            ((Button)GetTemplateChild("MaxButton")).MouseLeftButtonDown += MaxButtonClick;
            ((Button)GetTemplateChild("CloseButton")).MouseLeftButtonDown += CloseButtonClick;

        }

        void CloseButtonClick(object sender, MouseButtonEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.Close());
        }

        void MinButtonClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show("a");
            sender.ForWindowFromTemplate(w => w.WindowState = WindowState.Minimized);
        }

        void MaxButtonClick(object sender, MouseButtonEventArgs e)
        {
            sender.ForWindowFromTemplate(w => w.WindowState = (w.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
        }

        void ShowHideAsideClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("A");
            // (((((((sender as Button).Parent as Grid).Parent as Border).Parent as DockPanel).Children[1] as Grid).Children[1] as Frame).Content as Aside).ShowHideAsideClick(sender, e);
        }

        public static void AppBarShowHide(object sender, MouseButtonEventArgs e)
        {
            /*if ((((((sender as Grid).Children[0] as ContentPresenter).Content as Page).Content as Grid).Children.Count > 1) && (((((sender as Grid).Children[0] as ContentPresenter).Content as Page).Content as Grid).Children[1] is AppBar))
            {
                (((((sender as Grid).Children[0] as ContentPresenter).Content as Page).Content as Grid).Children[1] as AppBar).AppBarShowHide(sender, e);
            }*/
        }

        void TitleBarMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
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
