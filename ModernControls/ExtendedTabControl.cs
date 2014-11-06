using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ModernControls.InternalClasses;

namespace ModernControls
{
    public class ExtendedTabControl : TabControl, INotifyPropertyChanged
    {
        private WriteLogsDelegate _logsDelegate;
        private ObservableCollection<ExtendedTabItem> _tabsList;
        public ObservableCollection<ExtendedTabItem> TabsList
        {
            get { return _tabsList; }
            set
            {
                _tabsList = value;
                OnPropertyChanged("TabList");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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

    

        private bool CheckExistTabInItems(ExtendedTabItem tab)
        {
            return TabsList.FirstOrDefault(x => x.Tag.ToString().Equals(tab.Tag.ToString())) == null;
        }

        public void SetLogsDelegate(WriteLogsDelegate writeDelegate)
        {
            _logsDelegate = writeDelegate;
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
                        if (item.Type != SolutionItemTypes.ImplementationPlan)
                        {
                            var textEditor = new BasicTextEditor();
                            textEditor.SetText(tab.Tag.ToString());
                            tab.Content = textEditor;
                        }
                        else
                        {
                            var content = new Canvas();
                            tab.Content = content;
                        }
                        TabsList.Add(tab);
                        ItemsSource = TabsList;
                        SelectedIndex = TabsList.Count - 1;
                    }
                    else
                    {
                        var index = 0;
                        foreach (var currentTab in TabsList)
                        {
                            if (currentTab.Tag.ToString().Equals(tab.Tag.ToString()))
                            {
                                SelectedIndex = index;
                                break;
                            }
                            index++;
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
                _logsDelegate(string.Concat("Ошибка открытия вкладки: ", e.Message, e.Data.ToString(), e.ToString()), LogType.Error);
            }
        }
    }
}
