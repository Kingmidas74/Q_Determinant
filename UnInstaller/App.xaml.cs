using System;
using System.Windows;

namespace UnInstaller
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
        }
    }
}
