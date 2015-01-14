using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            get { return new List<string> { "WorkplaceTabs", "SolutionExplorer" }; }
        }

        public void Initialize(List<object> containers)
        {
            Methods.SE = (containers[3] as SolutionExplorer);
            CreateMenu(containers[0] as MenuItem);
            var workPlaceTabs = containers[2] as WorkplaceTabs;
            workPlaceTabs.DefineRevealer(".ip", AddIPView);
        }

        private void CreateMenu(ItemsControl item)
        {
            var pMI = new PluginMenuItem();
            var items = pMI.Items.Cast<MenuItem>().ToList();
            pMI.Items.Clear();
            foreach (MenuItem mi in items)
            {
                item.Items.Add(mi);
            }
        }

        private EnclosedTabItem AddIPView(FileInfo file)
        {
            var tabItem = new EnclosedTabItem
            {
                Header = file.Name,
                Tag = file.FullName
            };
            var viewver = new Viewer();
            viewver.SetContent(file);
            tabItem.Content = viewver;
            Methods.CurrentViewer = viewver;
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