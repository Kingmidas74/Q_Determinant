using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
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
using Converters;
using Microsoft.Win32;
using ModernControls;

namespace Q_Determinant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
       /* private CollectionViewSource Tabs { get; set; }
        private List<TabContent> TabsList { get; set; }

        private string currentSolutionPath = null;
        private string currentProjectPath = null;*/
        public MainWindow()
        {
            InitializeComponent();
            /*Height = (SystemParameters.PrimaryScreenHeight * 0.75);
            Width = (SystemParameters.PrimaryScreenWidth * 0.75);*/
        }

        public  void UpdateStatus(string content)
        {
            StatusContent = content;
        }
        /*private void OnLoad(object sender, RoutedEventArgs e)
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
                var currentProjectDirectory = System.IO.Path.GetDirectoryName((sender as TextBlock).Tag.ToString());
                currentProjectPath = System.IO.Path.Combine(currentProjectDirectory, currentProjectDirectory.Substring(currentProjectDirectory.LastIndexOf('\\') + 1) + ".qpr");
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
                currentSolutionPath = solutionFilePath;
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

        private void AddFlowChartClick(object sender, RoutedEventArgs e)
        {
            AddFlowchart("Flowchart2");
        }

        private void AddFlowchart(string flowchartName)
        {
            var QPrDoc = new XmlDocument();
            QPrDoc.Load(currentProjectPath);
            var flag = false;
            foreach (XmlElement e in from XmlElement e in QPrDoc.SelectNodes("//File") where e.Attributes["Name"].InnerText.EndsWith(".fc") select e)
            {
                flag = true;
            }
            if (!flag)
            {
                var converter = Manufactory.CreateFlowChartConverter(ConverterTypes.XML);
                converter.SaveToFile(System.IO.Path.GetDirectoryName(currentProjectPath) + "\\"+flowchartName+".fc");
                XmlNode node = QPrDoc.CreateNode(XmlNodeType.Element, "File", null);
                XmlAttribute attr = QPrDoc.CreateAttribute("Name");
                attr.InnerText = flowchartName + ".fc";
                node.Attributes.Append(attr);
                QPrDoc.SelectSingleNode("//Files").AppendChild(node);
                QPrDoc.Save(currentProjectPath);
                OpenSolution(currentSolutionPath);

            }
            else
            {
                MessageBox.Show("Flowcht exist in project!");
            }
        }

        private void AddProject(string projectName)
        {
            var QSolDoc = new XmlDocument();
            QSolDoc.Load(currentSolutionPath);
            var flag = false;
            foreach (XmlElement e in from XmlElement e in QSolDoc.SelectNodes("//Project") where e.Attributes["Name"].InnerText.Equals(projectName) select e)
            {
                flag = true;
            }
            if (!flag)
            {
                var newProject = new XmlDocument();
                XmlNode node = newProject.CreateNode(XmlNodeType.Element, "Project", null);
                XmlAttribute attr = newProject.CreateAttribute("Name");
                attr.InnerText = projectName;
                node.Attributes.Append(attr);
                node.AppendChild(newProject.CreateNode(XmlNodeType.Element, "Files", null));
                newProject.AppendChild(node);
                Directory.CreateDirectory(System.IO.Path.GetDirectoryName(currentSolutionPath) + @"\" + projectName);
                newProject.Save(System.IO.Path.GetDirectoryName(currentSolutionPath)+@"\"+projectName+@"\"+projectName+".qpr");
                node = QSolDoc.CreateNode(XmlNodeType.Element, "Project", null);
                attr = QSolDoc.CreateAttribute("Name");
                attr.InnerText = projectName;
                XmlAttribute attr2 = QSolDoc.CreateAttribute("Path");
                attr2.InnerText = projectName+"\\"+projectName+".qpr";
                node.Attributes.Append(attr);
                node.Attributes.Append(attr2);
                QSolDoc.SelectSingleNode("//Projects").AppendChild(node);
                QSolDoc.Save(currentSolutionPath);
                currentProjectPath = System.IO.Path.GetDirectoryName(currentSolutionPath) + @"\" + projectName + @"\" +
                                     projectName + ".qpr";
                AddFlowchart("TestFlowchart");
            }
            else
            {
                MessageBox.Show("Project exist in solution!");
            }
        }

        private void NewProjectClick(object sender, RoutedEventArgs e)
        {
            AddProject("TestProject");
        }

        private void TestCompileClick(object sender, RoutedEventArgs e)
        {
            var p = new System.Diagnostics.Process();
            var startupstring = new StringBuilder("");
            startupstring.Append(" -s ")
                .Append(currentSolutionPath)
                .Append(" -ip -o ")
                .Append(System.IO.Path.GetDirectoryName(currentSolutionPath))
                .Append(@"\Operations.json");
            MessageBox.Show(startupstring.ToString());
            p.StartInfo.FileName = "Compiler.exe";
            p.StartInfo.Arguments = startupstring.ToString();
            p.Start();
        }*/
    }
}
