using System;
using System.Collections.Generic;
using System.IO;
using BasicComponentsPack;
using DefaultControlsPack;
using PluginController;
using PluginController.Enums;

namespace ImplementationPlanViewer
{
    public class Plugin:IPlugin
    {

        public string Title
        {
            get { return "ImplementationPlanViewer"; }
        }

        public string Description
        {
            get { throw new NotImplementedException(); }
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

        public void Initialize(List<object> containers)
        {
            var WorkPlaceTabs = containers[0] as WorkplaceTabs;
            WorkPlaceTabs.DefineRevealer(".ip", AddIPView);
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
            return tabItem;
        }
    }
}