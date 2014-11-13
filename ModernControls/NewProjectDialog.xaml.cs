using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ModernControls.InternalClasses;

namespace ModernControls
{
    /// <summary>
    /// Interaction logic for NewProjectDialog.xaml
    /// </summary>
    public partial class NewProjectDialog : MWindow
    {
        public NewProjectDialog()
        {
            InitializeComponent();
            //ProjectList.ItemsSource =
        }

        public string ProjectTitle
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        public ProjectTypes ProjectType { get; set; }
        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ChooseProjectType(object sender, SelectionChangedEventArgs e)
        {
        }

        private void ChooseType(object sender, MouseButtonEventArgs e)
        {
            ProjectType = (ProjectTypes)Enum.Parse(typeof(ProjectTypes), (sender as StackPanel).Tag.ToString());
        }
    }
}
