using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PluginController
{
    public class PluginHost : IPluginHost
    {
        private readonly string _folderPath;
        private List<IPlugin> _plugins;
        private Dictionary<string,string> _libraries;
        private readonly string _interface;
        

        public List<IPlugin> Plugins
        {
            get
            {
                if (_plugins != null)
                {
                    return _plugins;
                }
                throw new NullReferenceException("Plugins not loaded");
            }
        }

        public Dictionary<string, string> Libraries
        {
            get
            {
                if (_libraries != null)
                {
                    return _libraries;
                }
                throw new NullReferenceException("Libraries not loaded");
            }
        }



        private void createDirectory()
        {
            if (!Directory.Exists(_folderPath))
            {
                Directory.CreateDirectory(_folderPath);
            }
        }

        public PluginHost(string directory, string type)
        {
            _folderPath = new DirectoryInfo(directory).FullName;
            _interface = type;
            LoadPlugins();
        }

        private void LoadPlugins()
        {
            createDirectory();

            if (_interface.Equals("PluginController.IPlugin"))
            {
                _plugins = new List<IPlugin>();
                Debug.WriteLine(_folderPath, "ASDASDASD");
                foreach (var file in Directory.GetFiles(_folderPath, "*.dll"))
                {
                    Debug.WriteLine(file, "PLUGIN");
                    var pluginDll = Assembly.LoadFile(file);
                    Debug.WriteLine(pluginDll.GetTypes(), "DLL");
                    foreach (var type in pluginDll.GetTypes())
                    {
                        foreach (var currentInterface in type.GetInterfaces())
                        {
                            if (currentInterface.ToString().Equals(_interface))
                            {
                                var plugin = (IPlugin) Activator.CreateInstance(type);
                                _plugins.Add(plugin);
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                _libraries = new Dictionary<string, string>();
                foreach (var file in Directory.GetFiles(_folderPath, "*.dll"))
                {
                    var pluginDll = Assembly.LoadFile(file);
                    foreach (var type in pluginDll.GetTypes())
                    {
                        foreach (var currentInterface in type.GetInterfaces())
                        {
                            if (currentInterface.ToString().Equals(_interface))
                            {
                                _libraries.Add(Path.GetFileNameWithoutExtension(file),file);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
