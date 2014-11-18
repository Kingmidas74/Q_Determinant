using System.Collections.Generic;

namespace PluginController
{
    public interface IPluginHost
    {
        string FolderPath { set; }
        List<IPlugin> Plugins { get; } 
    }
}