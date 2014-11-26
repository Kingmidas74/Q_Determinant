﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace PluginController
{
    public class PluginHost : IPluginHost
    {
        private string _folderPath;
        private List<IPlugin> _plugins; 
        public string FolderPath
        {
            set {
                createDirectory(value);
                _folderPath = value;
                LoadPlugins();
            }
        }

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

        private void createDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void LoadPlugins()
        {
            _plugins = new List<IPlugin>();
            var directory = new DirectoryInfo(_folderPath);
            foreach (var file in Directory.GetFiles(directory.FullName, "*.dll"))
            {
                var pluginDll = Assembly.LoadFile(file);
                foreach (var type in pluginDll.GetTypes())
                {
                    foreach (var currentInterface in type.GetInterfaces())
                    {
                        if (currentInterface == Type.GetType("PluginController.IPlugin"))
                        {
                            var plugin = (IPlugin)Activator.CreateInstance(type);
                            _plugins.Add(plugin);
                            break;
                        }
                    }
                }
            }
        }
    }
}
