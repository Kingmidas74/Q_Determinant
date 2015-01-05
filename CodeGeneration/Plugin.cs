using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BasicComponentsPack;
using BasicComponentsPack.InternalClasses;
using PluginController;
using VisualCore;
using VisualCore.Events;

namespace CodeGeneration
{
    public class Plugin:IPlugin,ISetProjectAndSolution
    {
        private GenerationButton _generateButton;
        public string Title
        {
            get { return "Code generation"; }
        }

        public string Description
        {
            get { return "Возможность генерации кода из плана выполнения"; }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Guid
        {
            get { throw new NotImplementedException(); }
        }

        public PluginController.Enums.PluginTypes Type
        {
            get { throw new NotImplementedException(); }
        }

        public PluginController.Enums.Positions Position
        {
            get { throw new NotImplementedException(); }
        }

        public string ContainerType
        {
            get { throw new NotImplementedException(); }
        }

        public List<string> InitializeObjects
        {
            get { return new List<string> { "SolutionExplorer", "WorkplaceTabs" }; }
        }

        private List<object> _containers; 

        public void Initialize(List<object> containers)
        {
            _containers = containers;
            CreateMenu(_containers[0] as MenuItem);
            var toolBar = _containers[1] as ToolBar;
            _generateButton = new GenerationButton();
            _generateButton.SE = _containers[2] as SolutionExplorer;
            toolBar.Items.Add(_generateButton);
            (_containers[3] as WorkplaceTabs).DefineRevealer(".gc", AddCodeEditor);
            (_containers[2] as SolutionExplorer).DefineRevealer(".gc", CreateCodeGenerationItem);
        }

        private BasicComponentsPack.InternalClasses.SolutionTreeItem CreateCodeGenerationItem(string filePath, string title)
        {
            return new SolutionTreeItem
            {
                FilePath = filePath,
                Title = title,
                Icon = Helpers.XamlClone((new GenerationButton()).FindName("CGIcon") as System.Windows.Shapes.Path)
            };
        }

        private DefaultControlsPack.EnclosedTabItem AddCodeEditor(System.IO.FileInfo file)
        {
            var tabItem = new DefaultControlsPack.EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var viewver = new TextEditor();
            viewver.SetContent(file);
            tabItem.Content = viewver;
            return tabItem;
        }

        private void CreateMenu(ItemsControl item)
        {
            var aboutItem = new MenuItem
            {
                Header = "About " + Title
            };
            aboutItem.Click += ShowAboutInfo;
            item.Items.Add(aboutItem);

            var runItem = new MenuItem
            {
                Header = "Run " + Title
            };
            runItem.Click += ShowGenerationWindow;
            item.Items.Add(runItem);
        }

        private void ShowGenerationWindow(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new CodeGenerationSettings();
            settingsWindow.SetSolutionPath((_containers[2] as SolutionExplorer));
            settingsWindow.Show();
        }

        private void ShowAboutInfo(object sender, RoutedEventArgs e)
        {
            var aboutInfo = new AboutWindow {Text = Description};
            aboutInfo.ShowDialog();
        }

        public void SetProjectListener(object sender, RoutedEventArgs e)
        {
            _generateButton.G_Button.IsEnabled = !String.IsNullOrEmpty(e.OriginalSource.ToString());
        }

        public void SetSolutionListener(object sender, RoutedEventArgs e)
        {
        }
    }
}
