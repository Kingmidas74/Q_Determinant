using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Core;
using FlowChart;
using ImplementationPlan;
using Microsoft.Win32;
using ModernControls;

namespace Q_Determinant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private CollectionViewSource Tabs { get; set; }
        private List<TabContent> TabsList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Height = (SystemParameters.PrimaryScreenHeight * 0.75);
            Width = (SystemParameters.PrimaryScreenWidth * 0.75);
        }
        private void OnLoad(object sender, RoutedEventArgs e)
        {
            TabsList = new List<TabContent>();
            FlowChartElements.ItemsSource = Adapter.TransformBlock();
            OpenSolution(@"D:\tempforQ\QSOL\QSOL.qsln");
        }

        private void ChooseAlgorithmFile(object sender, MouseButtonEventArgs e)
        {

            if (!CheckExistFileInList((sender as TextBlock).Tag.ToString()))
            {
                if((sender as TextBlock).Text.EndsWith(".fc"))
                {
                    var tb = new TextBox();
                    tb.Text = File.ReadAllText(((sender as TextBlock).Tag.ToString()), Encoding.UTF8);
                    tb.FontSize = 20;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.VerticalAlignment = VerticalAlignment.Stretch;
                    var tabContent = new TabContentWithText();
                    tabContent.Content = tb;
                    tabContent.Name = (sender as TextBlock).Text;
                    tabContent.Visible = Visibility.Visible;
                    tabContent.Path = (sender as TextBlock).Tag.ToString();
                    TabsList.Add(tabContent);
                    Tabs = FindResource("Tabs") as CollectionViewSource;
                    Tabs.Source = null;
                    Tabs.Source = TabsList;
                }
                else
                {
                    var tb = new TextBlock();
                    tb.Text = File.ReadAllText(((sender as TextBlock).Tag.ToString()), Encoding.UTF8);
                    tb.FontSize = 20;
                    tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                    tb.VerticalAlignment = VerticalAlignment.Stretch;
                    var tabContent = new TabContentWithCanvas();
                    tabContent.Content = tb;
                    tabContent.Name = (sender as TextBlock).Text;
                    tabContent.Visible = Visibility.Visible;
                    tabContent.Path = (sender as TextBlock).Tag.ToString();
                    TabsList.Add(tabContent);
                    Tabs = FindResource("Tabs") as CollectionViewSource;
                    Tabs.Source = null;
                    Tabs.Source = TabsList;
                }
            }
        }
        
        private bool CheckExistFileInList(string fileName)
        {
            return TabsList.Count(x => x.Path.Equals(fileName)) > 0;
        }

        private void OpenSolutionClick(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.DefaultExt = ".qsln";
            dlg.Filter = "SolutionFiles (*.qsln)|*.qsln";
            var result = dlg.ShowDialog();
            if (result == true)
            {
                OpenSolution(dlg.FileName);
            }
        }

        private void OpenSolution(string solutionFilePath)
        {
            try
            {
                var qSlnDocument = new XmlDocument();
                qSlnDocument.Load(solutionFilePath);
                var pathToSolution = System.IO.Path.GetDirectoryName(solutionFilePath);
                var treeViewItems = new List<AlgorithmFolder>();
                foreach (XmlElement project in qSlnDocument.SelectNodes("//Project"))
                {
                    var qPrDocument = new XmlDocument();
                    qPrDocument.Load(System.IO.Path.Combine(pathToSolution, project.Attributes["Path"].InnerText));
                    var currentAlgorithmProject = new AlgorithmFolder();
                    currentAlgorithmProject.Name = qPrDocument.SelectSingleNode("//Project").Attributes["Name"].InnerText;
                    foreach (XmlElement file in qPrDocument.SelectNodes("//File"))
                    {
                        var currentAlgorithmFile = new AlgorithmFile();
                        currentAlgorithmFile.Name = file.Attributes["Name"].InnerText;
                        currentAlgorithmFile.Path = System.IO.Path.Combine(pathToSolution, System.IO.Path.GetDirectoryName(project.Attributes["Path"].InnerText), currentAlgorithmFile.Name);
                        currentAlgorithmProject.Files.Add(currentAlgorithmFile);
                    }
                    treeViewItems.Add(currentAlgorithmProject);
                }
                SolutionExplorer.Header = qSlnDocument.SelectSingleNode("//Solution").Attributes["Name"].InnerText;
                SolutionExplorer.ItemsSource = treeViewItems;
            }
            catch (Exception e)
            {
                MessageBox.Show("EXCEPTION!!!!! INFO: " + e.Message, "EXCEPTION!");
            }
        }

        private void CloseProgramClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
