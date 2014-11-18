using System;

namespace PluginController
{
    public interface IPlugin
    {
        string Title { get; }
        string Description { get; }
        string Author { get; }
        Guid Guid { get; }
        string ContainerType { get; }
        void Initialize(object container);
        void Dispose(); 
    }
}