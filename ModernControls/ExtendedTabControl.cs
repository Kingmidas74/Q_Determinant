using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Core;
using ModernControls.InternalClasses;

namespace ModernControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:ModernControls;assembly=ModernControls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ExtendedTabControl/>
    ///
    /// </summary>
    public class ExtendedTabControl : TabControl, INotifyPropertyChanged
    {
        private CollectionViewSource Tabs { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<ExtendedTabItem> _tabsList;
        private WriteLogsDelegate _logsDelegate;

        public void SetLogsDelegate(WriteLogsDelegate writeDelegate)
        {
            _logsDelegate = writeDelegate;
        }
        public ObservableCollection<ExtendedTabItem> TabsList
        {
            get { return _tabsList; }
            set
            {
                _tabsList = value;
                OnPropertyChanged("TabList");
            }
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ExtendedTabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTabControl), new FrameworkPropertyMetadata(typeof(ExtendedTabControl)));
            AddHandler(ExtendedTabItem.CloseTabEvent, new RoutedEventHandler(CloseTab));
            TabsList = new ObservableCollection<ExtendedTabItem>();
        }

        private void CloseTab(object source, RoutedEventArgs args)
        {
            var item = args.OriginalSource as ExtendedTabItem;
            TabsList.Remove(item);
            ItemsSource = TabsList;
        }
        
        public void AddTab(ExtendedTreeViewItem item)
        {
            try
            {
                if (item.Type == SolutionItemTypes.FlowChart || item.Type == SolutionItemTypes.Qdeterminant ||
                    item.Type == SolutionItemTypes.ImplementationPlan)
                {
                    var tab = new ExtendedTabItem();
                    tab.Header = item.Header.ToString();
                    tab.Tag = item.Tag.ToString();
                    if (CheckExistTabInItems(tab))
                    {
                        if (item.Type != SolutionItemTypes.FlowChart)
                        {
                            var tb = new TextBox();
                            tb.Text = File.ReadAllText(tab.Tag.ToString(), Encoding.UTF8);
                            tb.FontSize = 20;
                            tb.HorizontalAlignment = HorizontalAlignment.Stretch;
                            tb.VerticalAlignment = VerticalAlignment.Stretch;
                            tab.Content = tb;
                            TabsList.Add(tab);
                        }
                        else
                        {
                            var content = new DrawCanvas();
                            content.CurrentBlockType = BlockTypes.Process;
                            tab.Content = content;
                            /*content.Width = (TemplatedParent as Workplace).ActualWidth;
                            content.Height = (TemplatedParent as Workplace).ActualHeight;*/
                            
                            TabsList.Add(tab);
                        }

                        ItemsSource = TabsList;
                        SelectedIndex = TabsList.Count - 1;
                    }
                    else
                    {
                        int Index = 0;
                        foreach (var _tab in TabsList)
                        {
                            if (_tab.Tag.ToString().Equals(tab.Tag.ToString()))
                            {
                                SelectedIndex = Index;
                                break;
                            }
                            Index++;
                        }
                    }
                }
                else
                {
                    throw new Exception("Вкладка иного типа");
                }
            }
            catch (Exception e)
            {
                _logsDelegate(string.Concat("Ошибка открытия вкладки: ", e.Message), LogType.Error);
            }
        }

        private bool CheckExistTabInItems(ExtendedTabItem tab)
        {
            return TabsList.FirstOrDefault(x => x.Tag.ToString().Equals(tab.Tag.ToString())) == null;    
        }


        
    }
}
