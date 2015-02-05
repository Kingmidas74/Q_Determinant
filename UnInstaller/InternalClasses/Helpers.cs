using System;
using System.Collections.Generic;
using System.Windows;
using DefaultControlsPack;

namespace UnInstaller.InternalClasses
{
        internal class Helpers
        {
            private static List<Type> _queue = new List<Type>()
        {
            typeof (WarningWindow),
            typeof (UnInstallWindow),
            typeof (FinishWindow),
        };

            internal static void GetNextWindow(object context)
            {
                var index = _queue.IndexOf(context.GetType());
                if (index < _queue.Count - 1)
                {
                    var nextWindow = (Activator.CreateInstance(_queue[index + 1]));
                    (nextWindow as ModernWindow).Top = (context as ModernWindow).Top;
                    (nextWindow as ModernWindow).Left = (context as ModernWindow).Left;
                    (nextWindow as ModernWindow).Show();
                    (context as ModernWindow).Close();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }

            internal static void GetPreviousWindow(object context)
            {
                var index = _queue.IndexOf(context.GetType());
                if (index > 0)
                {
                    var nextWindow = (Activator.CreateInstance(_queue[index - 1]));
                    (nextWindow as ModernWindow).Top = (context as ModernWindow).Top;
                    (nextWindow as ModernWindow).Left = (context as ModernWindow).Left;
                    (nextWindow as ModernWindow).Show();
                    (context as ModernWindow).Close();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }
}
