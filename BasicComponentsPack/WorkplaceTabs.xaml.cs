using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DefaultControlsPack;
using VisualCore;
using System.Collections.Generic;

namespace BasicComponentsPack
{
    public partial class WorkplaceTabs : UserControl
    {
        #region ErrorException
        public static readonly RoutedEvent ErrorExceptionEvent = EventManager.RegisterRoutedEvent("ErrorException",
            RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(WorkplaceTabs));

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
        }

        private void AddFlowChart(FileInfo file)
        {
            MessageBox.Show("FC");
        }
        private void AddImplementationPlan(FileInfo file)
        {
            MessageBox.Show("IP");
        }

        private void CloseTab(object sender, RoutedEventArgs e)
        {
            var currentTab = (e.OriginalSource as EnclosedTabItem);
            if (currentTab.Content is ISaveable)
            {
                (currentTab.Content as ISaveable).Save();
            }
        }

        public void SelectedFileListener(object sender, RoutedEventArgs e)
        {
            AddTab(e.OriginalSource.ToString());
        }

        public void AddTab(string filePath)
        {
            try
            {
                var PathToFile = new StringBuilder(filePath);
                var file = new FileInfo(filePath);
                switch (file.Extension)
                {
                    case ".fc":AddFlowChart(file);
                        break;
                    case ".ip": AddFlowChart(file);
                        break;
                    default: throw new Exception("Invalid file type");
                }
            }
            catch (Exception e)
            {
                RaiseEvent(new RoutedEventArgs(ErrorExceptionEvent, "Invalid file type"));
            }
        }

        public void SaveAllListener(object sender, RoutedEventArgs e)
        {
            foreach (var content in (from object item in WorkplaceTabControl.Items select (item as EnclosedTabItem).Content).OfType<ISaveable>())
            {
                content.Save();
            }
        }

        public void CloseSolutionListener(object sender, RoutedEventArgs e)
        {
            SaveAllListener(sender, e);
            WorkplaceTabControl.Items.Clear();
        }
    }
}
