using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core.Converters;
using VisualCore;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Viewer : UserControl,ISaveable,ITabContent
    {
        public Viewer()
        {
            InitializeComponent();
        }



        public void SetContent(FileInfo file)
        {
            var graph = Converter.DataToGraph(File.ReadAllText(file.FullName), ConverterFormats.JSON);
            ViewerContent.Width = 500;
            ViewerContent.Height = 500;
            ViewerContent.Background = FindResource("BaseFontBrush") as SolidColorBrush;
        }

        public bool IsChange
        {
            get { return false; }
        }

        public void Save()
        {
            
        }
    }
}
