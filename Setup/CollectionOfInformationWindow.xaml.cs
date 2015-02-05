using System;
using System.Windows;
using System.Windows.Forms;
using Setup.InternalClasses;

namespace Setup
{
    /// <summary>
    /// Interaction logic for CollectionOfInformationWindow.xaml
    /// </summary>
    public partial class CollectionOfInformationWindow
    {
        private readonly BaseViewModel _bvm = BaseViewModel.Instance;
        public CollectionOfInformationWindow()
        {
            InitializeComponent();
            DataContext = _bvm;
        }

        private void ChooseFolder(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.MyComputer;
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _bvm.StartupPath = dialog.SelectedPath;
            }
        }

        private void NextWindow(object sender, RoutedEventArgs e)
        {
            Helpers.GetNextWindow(this);
        }

        private void ExitOut(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void PreviousWindow(object sender, RoutedEventArgs e)
        {
            Helpers.GetPreviousWindow(this);
        }

        private void CheckProperty(object sender, RoutedEventArgs e)
        {/*
            if ((sender as System.Windows.Controls.CheckBox).Name.Equals("CreateIcon"))
            {
                StaticInstaller.CreateIcon = true;
            }
            if ((sender as System.Windows.Controls.CheckBox).Name.Equals("CreatePart"))
            {
                StaticInstaller.CreatePart = true;
            }*/
        }

        private void UnCheckProperty(object sender, RoutedEventArgs e)
        {/*
            if ((sender as System.Windows.Controls.CheckBox).Name.Equals("CreateIcon"))
            {
                StaticInstaller.CreateIcon = false;
            }
            if ((sender as System.Windows.Controls.CheckBox).Name.Equals("CreatePart"))
            {
                StaticInstaller.CreatePart = false;
            }*/
        }
    }
}
