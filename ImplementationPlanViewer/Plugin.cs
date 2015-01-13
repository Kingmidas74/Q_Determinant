using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using BasicComponentsPack;
using DefaultControlsPack;
using ImplementationPlanViewer.InternalClasses;
using PluginController;
using PluginController.Enums;
using VisualCore.Events;

namespace ImplementationPlanViewer
{
    public class Plugin:IPlugin,ICompile
    {

        public string Title
        {
            get { return "ImplementationPlanViewer"; }
        }

        public string Description
        {
            get { return  "Данный плагин позволяет просматривать план реализации в виде графа для удобства восприятия"; }
        }

        public string Author
        {
            get { throw new NotImplementedException(); }
        }

        public Guid Guid
        {
            get { throw new NotImplementedException(); }
        }

        public PluginTypes Type
        {
            get { throw new NotImplementedException(); }
        }

        public Positions Position
        {
            get { throw new NotImplementedException(); }
        }

        public string ContainerType
        {
            get { throw new NotImplementedException(); }
        }

        public List<string> InitializeObjects {
            get { return new List<string> {"WorkplaceTabs"}; }
        }

        public void Initialize(List<object> containers)
        {
            CreateMenu(containers[0] as MenuItem);
            var workPlaceTabs = containers[2] as WorkplaceTabs;
            workPlaceTabs.DefineRevealer(".ip", AddIPView);
        }

        private void CreateMenu(ItemsControl item)
        {
            var aboutItem = new MenuItem
            {
                Header = "About " + Title
            };
            aboutItem.Click += ShowAboutInfo;
            item.Items.Add(aboutItem);
        }

        private void ShowAboutInfo(object sender, RoutedEventArgs e)
        {
            var aboutInfo = new AboutWindow();
            aboutInfo.Text = Description;
            aboutInfo.ShowDialog();
        }

        private EnclosedTabItem AddIPView(FileInfo file)
        {
            var tabItem = new EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var viewver = new Viewer();
            (viewver.DataContext as ViewerVM).SetContent(file);
            tabItem.Content = viewver;
            return tabItem;
        }

        public void BeforeCompilerListener(object sender, RoutedEventArgs e)
        {
        }

        public void AfterCompilerListener(object sender, RoutedEventArgs e)
        {
        }
    }
}