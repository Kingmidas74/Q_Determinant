using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DefaultControlsPack;
using VisualCore;
using System.Collections.Generic;

namespace BasicComponentsPack
{
    public partial class WorkplaceTabs : UserControl
    {
        private readonly Dictionary<string, Func<FileInfo, EnclosedTabItem>> _fileRevealers;

        private List<string> _openedFiles; 

        #region ErrorException
        public static readonly RoutedEvent ErrorExceptionEvent = EventManager.RegisterRoutedEvent("ErrorException",
            RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(WorkplaceTabs));

        public event RoutedEventHandler ErrorException
        {
            add { AddHandler(ErrorExceptionEvent, value); }
            remove { RemoveHandler(ErrorExceptionEvent, value); }
        }
        #endregion

        public WorkplaceTabs()
        {
            InitializeComponent();
            AddHandler(EnclosedTabControl.CloseTabEvent, new RoutedEventHandler(CloseTab));
            _fileRevealers = new Dictionary<string, Func<FileInfo, EnclosedTabItem>>
                        {
                            { ".fc", this.AddFlowChart },
                            { ".ip", this.AddImplementationPlan },
                            { ".qd", this.AddQDeterminant },
                        };
            _openedFiles = new List<string>();
        }

        private EnclosedTabItem AddQDeterminant(FileInfo file)
        {
            var tabItem = new EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var textEditor = new TextEditor();
            textEditor.SetContent(file);
            tabItem.Content = textEditor;
            return tabItem;
        }

        private EnclosedTabItem AddFlowChart(FileInfo file)
        {
            var tabItem = new EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var textEditor = new TextEditor();
            textEditor.SetContent(file);
            tabItem.Content = textEditor;
            return tabItem;
        }
        private EnclosedTabItem AddImplementationPlan(FileInfo file)
        {
            var tabItem = new EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var textEditor = new TextEditor();
            textEditor.SetContent(file);
            tabItem.Content = textEditor;
            return tabItem;
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            var currentTab = (e.OriginalSource as EnclosedTabItem);
            if (currentTab.Content is ISaveable)
            {
                (currentTab.Content as ISaveable).Save();
            }
            _openedFiles.Remove(currentTab.Tag.ToString());
        }

        public void SelectedFileListener(object sender, RoutedEventArgs e)
        {
            AddTab(e.OriginalSource.ToString());
        }

        private EnclosedTabItem PerformRevealer(FileInfo file)
        {
            if (!_fileRevealers.ContainsKey(file.Extension))
                throw new ArgumentException(string.Format("Invalid Format File"));
            return _fileRevealers[file.Extension](file);
        }

        public void DefineRevealer(string extention, Func<FileInfo, EnclosedTabItem> revealer)
        {
            try
            {
                if (_fileRevealers.ContainsKey(extention))
                {
                    _fileRevealers[extention] = revealer;
                }
                else
                {
                    _fileRevealers.Add(extention, revealer);
                }
            }
            catch (Exception e)
            {
                RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, e.Message));
            }
        }

        public void AddTab(string filePath)
        {
            try
            {
                if (!FileIsOpen(filePath))
                {
                    var tabItem = PerformRevealer(new FileInfo(filePath));
                    WorkplaceTabControl.Items.Add(tabItem);
                    WorkplaceTabControl.SelectedItem = tabItem;
                    _openedFiles.Add(filePath);
                }
                else
                {
                    WorkplaceTabControl.SelectedIndex = GetIndexOfTab(filePath);
                }
            }
            catch (Exception e)
            {
                RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, e.Message));
            }
        }

        private int GetIndexOfTab(string filepath)
        {
            return WorkplaceTabControl.Items.Cast<EnclosedTabItem>().Where(item => item.Tag.ToString().Equals(filepath)).Select(item => WorkplaceTabControl.Items.IndexOf(item)).FirstOrDefault();
        }

        private bool FileIsOpen(string filePath)
        {
            return _openedFiles.Exists(x => x.Equals(filePath));
        }

        public void SaveAllListener(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("SA");
            foreach (var content in (from object item in WorkplaceTabControl.Items select (item as EnclosedTabItem).Content).OfType<ISaveable>())
            {
                content.Save();
            }
        }

        public void CloseSolutionListener(object sender, RoutedEventArgs e)
        {
            SaveAllListener(sender, e);
            WorkplaceTabControl.Items.Clear();
            _openedFiles.Clear();
        }

        public void BeforeCompilerListener(object sender, RoutedEventArgs e)
        {
            SaveAllListener(sender,e);
        }

        public void AfterCompilerListener(object sender, RoutedEventArgs e)
        {
            foreach (var content in (from object item in WorkplaceTabControl.Items select (item as EnclosedTabItem).Content).OfType<ITabContent>())
            {
                content.ReLoad();
            }
        }
    }
}
