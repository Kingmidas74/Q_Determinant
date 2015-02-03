using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PluginController
{
    public class PluginHost
    {
        public static List<IPlugin> GetPlugins(string pathToPluginsFolder)
        {
            var plugins = new List<IPlugin>();
            foreach (var file in from file in Directory.GetFiles(pathToPluginsFolder, "*.dll") let pluginDll = Assembly.LoadFile(file) select file)
            {
                plugins.AddRange(from type in Assembly.LoadFile(file).GetTypes() where type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("PluginController.IPlugin")) select (IPlugin)Activator.CreateInstance(type));
            }
            return plugins;
        }

        public static List<FileInfo> AvailableDLLs(string pathToDLLsFolder, Type targetInterface)
        {
            var DLLs = new List<FileInfo>();
            Debug.WriteLine(targetInterface.ToString());
            Debug.WriteLine(pathToDLLsFolder);
            foreach (var file in Directory.GetFiles(pathToDLLsFolder, "*.dll"))
            {
                var pluginDll = Assembly.LoadFile(file);
                foreach (var type in pluginDll.GetTypes())
                {
                    foreach (var currentInterface in type.GetInterfaces())
                    {
                        if (currentInterface.ToString().Equals(targetInterface.ToString()))
                        {
                            DLLs.Add(new FileInfo(file));
                            break;
                        }
                    }
                }
            }
            return DLLs;
        }

    }
}
