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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CodeGeneration.Enums;
using CodeGeneration.InternalClasses;
using DefaultControlsPack;

namespace CodeGeneration
{
    /// <summary>
    /// Interaction logic for SettingsProjectTabItem.xaml
    /// </summary>
    public partial class SettingsProjectTabItem : UserControl
    {
        public SettingsProjectTabItem()
        {
            InitializeComponent();
        }

        public void SetContent(List<string> variables)
        {
            VariableTypes.ItemsSource = null;
            var _variables = new List<VariableTypesGridViewItem>();
            foreach (var variable in variables)
            {
                _variables.Add(new VariableTypesGridViewItem {Title = variable, Type = Enums.VariableTypes.Integer});
            }
            MessageBox.Show(_variables.Count.ToString());
            VariableTypes.ItemsSource = _variables;
        }

        public List<VariableTypesGridViewItem> GetContent()
        {
            return (from object item in VariableTypes.Items
                select new VariableTypesGridViewItem
                {
                    Title = (item as VariableTypesGridViewItem).Title, Type = (item as VariableTypesGridViewItem).Type
                }).ToList();
        }
    }
}
