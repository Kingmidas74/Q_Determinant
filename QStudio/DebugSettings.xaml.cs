using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using DefaultControlsPack;
using PluginController;

namespace QStudio
{
    /// <summary>
    /// Interaction logic for DebugSettings.xaml
    /// </summary>
    public partial class DebugSettings : ModernWindow
    {
        private readonly XDocument _settings;
        private const string Path = @"config.xml";
        public DebugSettings()
        {
            InitializeComponent();
            try
            {
                if (File.Exists(Path))
                {
                    _settings = XDocument.Load(Path);
                }
                else
                {
                    _settings=new XDocument();
                    _settings.Add(new XElement("Settings"));
                }
            }
            catch
            {}
        }

        private void LoadDeterminantLibraries(object sender, RoutedEventArgs e)
        {
            var pluginController = new PluginHost("libs", "Core.Interfaces.IDeterminant");
            var libs = pluginController.Libraries;
            var parent = (sender as ComboBox);
            parent.Items.Clear();
            foreach (var lib in libs)
            {
                var item = new ComboBoxItem();
                item.Tag = lib.Value;
                item.Content = lib.Key;
                parent.Items.Add(item);
            }
            XElement currentLib = null;
            try
            {
                currentLib = _settings.Element("Settings").Element("QDeterminant");
            }
            catch
            {
            }
            var index = 0;
            if (currentLib != null)
            {
                for (var i=0; i<parent.Items.Count; i++)
                {
                    var item = parent.Items[i] as ComboBoxItem;
                    if (!item.Tag.ToString().Equals(currentLib.Attribute("Path").Value)) continue;
                    index = i;
                    break;
                }
            }
            parent.SelectedIndex = index;
        }

        private void LoadImplementationPlanLibraries(object sender, RoutedEventArgs e)
        {
            var pluginController = new PluginHost("libs", "Core.Interfaces.IPlan");
            var libs = pluginController.Libraries;
            var parent = (sender as ComboBox);
            parent.Items.Clear();
            foreach (var lib in libs)
            {
                var item = new ComboBoxItem();
                item.Tag = lib.Value;
                item.Content = lib.Key;
                parent.Items.Add(item);
            }
            XElement currentLib = null;
            try
            {
                currentLib = _settings.Element("Settings").Element("ImplementationPlan");
            }
            catch
            {
            }
            var index = 0;
            if (currentLib != null)
            {
                for (var i = 0; i < parent.Items.Count; i++)
                {
                    var item = parent.Items[i] as ComboBoxItem;
                    if (!item.Tag.ToString().Equals(currentLib.Attribute("Path").Value)) continue;
                    index = i;
                    break;
                }
            }
            parent.SelectedIndex = index;
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void OKClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _settings.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value = (ImplementationPlanLibraries.SelectedItem as ComboBoxItem).Tag.ToString();
            }
            catch (Exception exception)
            {
                var element = new XElement("ImplementationPlan");
                var attribute = new XAttribute("Path", (ImplementationPlanLibraries.SelectedItem as ComboBoxItem).Tag.ToString());
                element.Add(attribute);
                _settings.Element("Settings").Add(element);
            }
            try
            {
                _settings.Element("Settings").Element("QDeterminant").Attribute("Path").Value = (QDeterminantLibraries.SelectedItem as ComboBoxItem).Tag.ToString();
            }
            catch (Exception exception)
            {
                var element = new XElement("QDeterminant");
                var attribute = new XAttribute("Path", (QDeterminantLibraries.SelectedItem as ComboBoxItem).Tag.ToString());
                element.Add(attribute);
                _settings.Element("Settings").Add(element);
            }
            _settings.Save(Path);
            this.Close();
        }
    }
}
