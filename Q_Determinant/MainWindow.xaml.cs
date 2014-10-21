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
using Core;
using FlowChart;
using ImplementationPlan;
using ModernControls;

namespace Q_Determinant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MWindow
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
            SolutionExplorer.ItemsSource = Adapter.ConvertTreeFolderTo(@"D:\tempforQ\QSOL");
            
        }

        private void ChooseAlgorithmFile(object sender, MouseButtonEventArgs e)
        {

            if (!checkExistFileInList((sender as TextBlock).Tag.ToString())/*(sender as TextBlock).Text.EndsWith(".qd")*/)
            {
                var tb = new TextBlock();
                tb.Text = File.ReadAllText(((sender as TextBlock).Tag.ToString()));
                tb.FontSize = 20;
                tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                tb.VerticalAlignment = VerticalAlignment.Stretch;
                var tabContent = new TabContentWithText();
                tabContent.Content = tb;
                tabContent.Text = (sender as TextBlock).Text;
                tabContent.Name = (sender as TextBlock).Text;
                tabContent.Visible = Visibility.Visible;
                tabContent.Path = (sender as TextBlock).Tag.ToString();
                TabsList.Add(tabContent);
                Tabs = FindResource("Tabs") as CollectionViewSource;
                Tabs.Source = null;                                 
                Tabs.Source = TabsList;
            }
        }

        private bool checkExistFileInList(string fileName)
        {
            return TabsList.Count(x => x.Path.Equals(fileName)) > 0;
        }
    }
}
