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
        private static ExtendedTabControl _instance=null;
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
            _instance = this;
        }

        private void CloseTab(object source, RoutedEventArgs args)
        {
            var item = args.OriginalSource as ExtendedTabItem;
            TabsList.Remove(item);
            ItemsSource = TabsList;
        }

    

        private bool CheckExistTabInItems(ExtendedTabItem tab, string prefix=null)
        {
            if (!String.IsNullOrEmpty(prefix))
            {
                tab.Tag = (new StringBuilder(prefix).Append(tab.Tag));
            }
            return TabsList.FirstOrDefault(x => x.Tag.ToString().Equals(tab.Tag.ToString())) == null;
        }

        private bool CheckExistTabByNameInItems(string name)
        {
            return true;//TabsList.FirstOrDefault(x => x.Tag.ToString().Equals(tab.Tag.ToString())) == null;
        }

        public void SetLogsDelegate(WriteLogsDelegate writeDelegate)
        {
            _logsDelegate = writeDelegate;
        }

        public static void SAIFile(string filepath, SolutionItemTypes type)
        {
            if (_instance != null)
            {
                var prefix = "[Image] ";
                var tab = new ExtendedTabItem();
                tab.Header = (new StringBuilder(prefix).Append(Path.GetFileName(filepath))).ToString();
                tab.Tag = filepath;
                if (_instance.CheckExistTabInItems(tab, prefix))
                {
                    var imageCanvas = new BasicCanvas();
                    imageCanvas.SetFile(filepath,type);
                    tab.Content = imageCanvas;
                    _instance.TabsList.Add(tab);
                    _instance.ItemsSource = _instance.TabsList;
                    _instance.SelectedIndex = _instance.TabsList.Count - 1;
                }
                else
                {
                    var index = 0;
                    foreach (var currentTab in _instance.TabsList)
                    {
                        if (currentTab.Tag.ToString().Equals(tab.Tag.ToString()))
                        {
                            _instance.SelectedIndex = index;
                            break;
                        }
                        index++;
                    }
                }
            }
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
                        var textEditor = new BasicTextEditor();
                        textEditor.SetText(tab.Tag.ToString());
                        tab.Content = textEditor;
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
