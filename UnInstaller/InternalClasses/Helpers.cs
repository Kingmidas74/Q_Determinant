using System;
using System.Collections.Generic;
using System.Windows;
using DefaultControlsPack;

namespace UnInstaller.InternalClasses
{
        internal class Helpers
        {
            private static dynamic LastWindow;
            private static List<Type> _queue = new List<Type>()
        {
            typeof (WarningWindow),
            typeof (UnInstallWindow),
            typeof (FinishWindow),
        };


            internal static void GetNextWindow(object context)
            {
                var index = _queue.IndexOf(context.GetType());
                if (index == 0)
                {
                    LastWindow = (Activator.CreateInstance(_queue[_queue.Count - 1]));
                    (LastWindow as ModernWindow).Top = (context as ModernWindow).Top;
                    (LastWindow as ModernWindow).Left = (context as ModernWindow).Left;
                }
                if (index < _queue.Count - 1)
                {
                    try
                    {
                        var nextWindow = (Activator.CreateInstance(_queue[index + 1]));
                        (nextWindow as ModernWindow).Top = (context as ModernWindow).Top;
                        (nextWindow as ModernWindow).Left = (context as ModernWindow).Left;
                        (nextWindow as ModernWindow).Show();
                        (context as ModernWindow).Close();
                    }
                    catch (Exception e)
                    {
                        (LastWindow as ModernWindow).Show();
                        (context as ModernWindow).Close();
                    }
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
