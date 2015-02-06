using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using IWshRuntimeLibrary;
using Setup.InternalClasses;
using Setup.Properties;
using File = IWshRuntimeLibrary.File;

namespace Setup
{
    /// <summary>
    /// Interaction logic for InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow
    {
        private double _step;
        readonly BackgroundWorker _worker;
        
        public InstallWindow()
        {
            InitializeComponent();
            BaseViewModel.Instance.UpdateModel();
            _worker = new BackgroundWorker();
            _worker.DoWork += worker_DoWork;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            CalculateStep();
            SetupService();
            SetupProgram();
            Associate();
            CreateFolder();
            CreateIcon();
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

        private void SetupService()
        {
            if (ProjectInstall.SetupUpdateService)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    PartialBar.Value = 0;
                    LogBox.Text = "Установка службы обновлений";
                });

                for (var i = 0; i <= 100; i += 20)
                {
                    System.Threading.Thread.Sleep(500);
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        PartialBar.Value = i;
                    });
                }
                Application.Current.Dispatcher.Invoke(delegate
                {
                    TotalBar.Value += _step;
                });
            }
        }

        private void CreateFolder()
        {
            if (ProjectInstall.CreateFolder)
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
            }
        }

        private void CreateIcon()
        {
            if (ProjectInstall.CreateIcon)
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    LogBox.Text = "Создание значка на рабочем столе";
                });

                string shortcutLocation = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Settings.Default.ProgramName+".lnk");
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
            }
        }

        private void UnzipResource(byte[] currentArchive, string targetPath)
        {
            var tempPath = System.IO.Path.GetTempPath();
            var tempFile = System.IO.Path.Combine(tempPath, "QTemp.zip");
            var tempFolder = System.IO.Path.Combine(tempPath, "QTemp");
            if (System.IO.Directory.Exists(tempFolder))
            {
                System.IO.Directory.Delete(tempFolder, true);
            }
            System.IO.Directory.CreateDirectory(tempFolder);
            var transformerToZip = new System.IO.FileStream(tempFile, System.IO.FileMode.Create);
            transformerToZip.Write(currentArchive, 0, currentArchive.Length);
            transformerToZip.Close();
            System.IO.Compression.ZipFile.ExtractToDirectory(tempFile, tempFolder);
            System.IO.File.Delete(tempFile);
            foreach (var dirPath in System.IO.Directory.GetDirectories(tempFolder, "*", System.IO.SearchOption.AllDirectories))
                System.IO.Directory.CreateDirectory(dirPath.Replace(tempFolder, targetPath));

            foreach (var newPath in System.IO.Directory.GetFiles(tempFolder, "*.*", System.IO.SearchOption.AllDirectories))
                System.IO.File.Copy(newPath, newPath.Replace(tempFolder, targetPath), true);

            System.IO.Directory.Delete(tempFolder, true);
        }

        private void SetupProgram()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 0;
                LogBox.Text = "Установка QStudio";
            });

            UnzipResource(Properties.Resources.BasePart, ProjectInstall.GetShortPathToStartupDirectory());
            UnzipResource(Properties.Resources.UserPart, ProjectInstall.GetPathToDocuments());
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 95;
            });
            Application.Current.Dispatcher.Invoke(delegate
            {
                TotalBar.Value += _step;
            });
        }

        private void Associate()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 0;
                LogBox.Text = "Ассоциация файлов";
            });
            var descriptionToExtensions = new Dictionary<string, string>()
            {
                {".fc", Settings.Default.ProgramName + ".FlowChart.Reader"},
                {".qd", Settings.Default.ProgramName + ".QDeterminant.Reader"},
                {".ip", Settings.Default.ProgramName + ".ImplemenatationPlan.Reader"},
                {".qpr", Settings.Default.ProgramName + ".Project.Launcher"},
                {".qsln", Settings.Default.ProgramName + ".Solution.Launcher"},
            };
            foreach (var extension in ProjectInstall.AssociateFiles.Where(x => x.Value))
            {
                if (InternalClasses.Associate.IsAssociated(extension.Key))
                {
                    InternalClasses.Associate.RemoveAssociate(extension.Key, descriptionToExtensions[extension.Key]);
                }
                InternalClasses.Associate.SetAssociate(extension.Key, ProjectInstall.GetPathToQStudio(), descriptionToExtensions[extension.Key], descriptionToExtensions[extension.Key],ProjectInstall.GetPathToIcon());
            }
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 50;
            });
            InternalClasses.Associate.AddToUnInstallPanel(ProjectInstall.GetPathToUnInstaller(),Settings.Default.ProgramName, ProjectInstall.GetPathToIcon(),Settings.Default.CompanyName,ProjectInstall.GetShortPathToStartupDirectory());
            Application.Current.Dispatcher.Invoke(delegate
            {
                PartialBar.Value = 75;
            });
            Application.Current.Dispatcher.Invoke(delegate
            {
                TotalBar.Value += _step;
            });
        }

        private void CalculateStep()
        {
            var steps = 2;
            if (ProjectInstall.SetupUpdateService)
            {
                steps++;
            }
            if (ProjectInstall.CreateFolder)
            {
                steps++;
            }
            if (ProjectInstall.CreateIcon)
            {
                steps++;
            }
            _step = 100 / steps;
        }

        private void InstallWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            TotalBar.Value = 0;
            PartialBar.Value = 0;
            _worker.RunWorkerAsync();
        }

    }
}
