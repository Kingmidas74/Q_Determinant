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
            get { return new List<string> { "WorkplaceTabs", "SolutionExplorer", "RightBottomSegment" }; }
        }

        private List<object> _containers; 

        public void Initialize(List<object> containers)
        {
            _containers = containers;
            Methods.SE = (containers[3] as SolutionExplorer);
            CreateMenu(containers[0] as MenuItem);
          //  ((containers[1] as ToolBar).Parent as ToolBarTray).ToolBars.Add(new PluginToolbar().PluginToolBar);
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
            EnclosedTabItem propertyTab;
            if (Methods.PropertyGridInstance == null)
            {
                propertyTab = CreatePropertyGrid(file);
                (_containers[4] as EnclosedTabControl).Items.Add(propertyTab);
            }
            else
            {
                (Methods.PropertyGridInstance.Content as PropertyGrid).SetFilePath(file);
            }
            return tabItem;
        }

        private EnclosedTabItem CreatePropertyGrid(FileInfo file)
        {
            Methods.PropertyGridInstance = new EnclosedTabItem();
            Methods.PropertyGridInstance.Header = file.Name + " property";
            Methods.PropertyGridInstance.Tag =
                System.IO.Path.GetFileNameWithoutExtension((_containers[3] as SolutionExplorer).CurrentProjectPath) +
                file.Name;
            var propertyGrid = new PropertyGrid();
            propertyGrid.SetFilePath(file);
            Methods.PropertyGridInstance.Content = propertyGrid;
            return Methods.PropertyGridInstance;
        }

        public void BeforeCompilerListener(object sender, RoutedEventArgs e)
        {
        }

        public void AfterCompilerListener(object sender, RoutedEventArgs e)
        {
        }
    }
}