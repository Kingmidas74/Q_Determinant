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
        public MainWindow()
        {
            InitializeComponent();
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.75);
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.75);
        }

        private void ClickBt(object sender, RoutedEventArgs e)
        {
        }

        private void OnLoad(object sender, RoutedEventArgs e)
        {
            FlowChartElements.ItemsSource = Adapter.TransformBlock();
            SolutionExplorer.ItemsSource = Adapter.ConvertTreeFolderTo(@"D:\tempforQ\QSOL");
        }

        private void ChooseAlgorithmFile(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show((sender as TextBlock).Text);
        }
    }
}
