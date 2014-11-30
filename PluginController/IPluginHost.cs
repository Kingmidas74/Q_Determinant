using System.Collections.Generic;

namespace PluginController
{
    public interface IPluginHost
    {
        List<IPlugin> Plugins { get; }
        Dictionary<string, string> Libraries { get; } 
    }
}