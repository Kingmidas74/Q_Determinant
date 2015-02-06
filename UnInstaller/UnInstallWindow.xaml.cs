using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using UnInstaller.InternalClasses;
using UnInstaller.Properties;

namespace UnInstaller
{
    /// <summary>
    /// Interaction logic for UnInstallWindow.xaml
    /// </summary>
    public partial class UnInstallWindow
    {
        private double _step;
        readonly BackgroundWorker _worker;
        
        public UnInstallWindow()
        {
            InitializeComponent();
            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            RemoveAssociate();
            RemoveIcon();
            RemoveFolder();
            try
            {
                RemoveProgram();
            }
            catch
            {
            }
            Application.Current.Dispatcher.Invoke(ShowButton);

        }

        private void NextWindow(object sender, RoutedEventArgs e)
        {
            Helpers.GetNextWindow(this);
        }



        private void ShowButton()
        {
            ((Storyboard)FindResource("OnUnChecking")).Begin();
        }

        private void RemoveFolder()
        {
            /*if (ProjectInstall.CreateFolder)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = 0;
                    LogBox.Text = "Создание группы в меню \"Пуск\"";
                });
                System.Threading.Thread.Sleep(500);

                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = 50;
                    TotalBar.Value += _step;
                });
            }*/
        }

        private void RemoveIcon()
        {
            var pathToIcon = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Settings.Default.ProgramName + ".lnk");
            if (File.Exists(pathToIcon))
            {
                File.Delete(pathToIcon);
            }
        }



        private void RemoveProgram()
        {
            var dir = System.IO.Path.GetDirectoryName(
                Registry.ClassesRoot.OpenSubKey(@"QStudio.Solution.Launcher\Shell\Open\Command")
                    .GetValue("")
                    .ToString()
                    .Split(new[] {"\"%"}, StringSplitOptions.None)[0]
                    );
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir,true);
            }
        }

        private void RemoveAssociate()
        {
            var descriptionToExtensions = new Dictionary<string, string>()
            {
                {".fc", Settings.Default.ProgramName + ".FlowChart.Reader"},
                {".qd", Settings.Default.ProgramName + ".QDeterminant.Reader"},
                {".ip", Settings.Default.ProgramName + ".ImplemenatationPlan.Reader"},
                {".qpr", Settings.Default.ProgramName + ".Project.Launcher"},
                {".qsln", Settings.Default.ProgramName + ".Solution.Launcher"},
            };
            foreach (var extension in descriptionToExtensions.Where(extension => Associate.IsAssociated(extension.Key)))
            {
                Associate.RemoveAssociate(extension.Key, extension.Value);
            }
            Associate.RemoveFromUnInstallPanel(Settings.Default.ProgramName);

            /*
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 0;
                LogBox.Text = "Ассоциация файлов";
            });
            /*if (AssociateFile.IsAssociated)
            {
                AssociateFile.Remove();
            }
            AssociateFile.Associate();*/
            /*for (var i = 0; i <= 100; i += 20)
            {
                System.Threading.Thread.Sleep(500);
                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = i;
                });
            }*/

           /* Application.Current.Dispatcher.Invoke(delegate
            {
                TotalBar.Value += _step;
            });*/
        }

        private void InstallWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TotalBar.Value = 0;
            _worker.RunWorkerAsync();
        }
    }
}
