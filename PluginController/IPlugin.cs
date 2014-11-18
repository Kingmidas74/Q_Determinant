using System;
using System.Collections.Generic;
using PluginController.Enums;

namespace PluginController
{
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        string Author { get; }
        Guid Guid { get; }
        PluginTypes Type { get; }
        Positions Position { get; }
        string ContainerType { get; }
        void Initialize(List<object> containers);
    }
}