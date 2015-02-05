using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using UnInstaller.InternalClasses;

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
            RemoveProgram();
            RemoveAssociate();
            RemoveFolder();
            RemoveIcon();
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
            /*if (ProjectInstall.CreateIcon)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    LogBox.Text = "Создание значка на рабочем столе";
                });

                string shortcutLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QStudio.lnk");
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);
                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = 75;
                });
                shortcut.Description = string.Format("{0} startup",Settings.Default.ProgramName);
                shortcut.IconLocation = ProjectInstall.GetPathToIcon();
                shortcut.TargetPath = ProjectInstall.GetPathToQStudio();
                shortcut.Save();
                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = 100;
                    TotalBar.Value += _step;
                });
            }*/
        }



        private void RemoveProgram()
        {/*
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 0;
                LogBox.Text = "Установка QStudio";
            });

            var PF = Properties.Resources.PF;
            var tempPath = System.IO.Path.GetTempPath();
            var tempFile = System.IO.Path.Combine(tempPath, "part1.zip");

            var fs = new FileStream(tempFile, FileMode.Create);
            fs.Write(PF, 0, PF.Length);
            fs.Close();
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 25;
            });

            ZipFile.ExtractToDirectory(tempFile, ProjectInstall.GetShortPathToStartupDirectory());
            File.Delete(tempFile);
            System.Threading.Thread.Sleep(500);
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 50;
            });
            var MD = Properties.Resources.MD;
            fs = new FileStream(tempFile, FileMode.Create);
            fs.Write(MD, 0, MD.Length);
            fs.Close();
            System.Threading.Thread.Sleep(500);
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 70;
            });
            ZipFile.ExtractToDirectory(tempFile, ProjectInstall.GetPathToDocuments());
            File.Delete(tempFile);
            System.Threading.Thread.Sleep(500);
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 95;
            });
            Application.Current.Dispatcher.Invoke(delegate
            {
                TotalBar.Value += _step;
            });
          * */
        }

        private void RemoveAssociate()
        {/*
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
