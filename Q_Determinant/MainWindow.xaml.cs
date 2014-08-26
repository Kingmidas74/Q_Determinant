using System;
using System.Collections.Generic;
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

namespace Q_Determinant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClickBt(object sender, RoutedEventArgs e)
        {
            /*
             * JObject.Parse(@"{
    'start': {
        'id': 0,
        'outputLink': [2]
    },
    'end': {
        'id': 1,
        'inputLink': [2]
    },
    'blocks': [
        {
            'id': 2,
            'type': 'process',
            'content': 'i: =2',
            'inputLink': [0],
            'outputLink': [3]
        },
{
            'id': 3,
            'type': 'process',
            'content': 'k: =0',
            'inputLink': [2],
            'outputLink': [4]
        },
{
            'id': 4,
            'type': 'condition',
            'content': true,
            'falsePath': 7,
            'truePath': 5,
            'inputLink': [3],
            'outputLink': [5,7]
        },
{
            'id': 5,
            'type': 'process',
            'content': 'k++',
            'inputLink': [4],
            'outputLink': [6]
        },
{
            'id': 6,
            'type': 'process',
            'content': 'i++',
            'inputLink': [5],
            'outputLink': [4]
        },
{
            'id': 3,
            'type': 'InputOutput',
            'content': 'k',
            'inputLink': [4],
            'outputLink': [1]
        },
    ]
}")
             * */
           
        }
    }
}
