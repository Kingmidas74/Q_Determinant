using System;
using System.Collections.Generic;
using System.Windows;

namespace ModernControls.InternalClasses
{
    internal class MenuItemDataTemplate
    {
        public string Name { get; set; }

        public event RoutedEventHandler Clicked;

        

        public List<MenuItemDataTemplate> SubItems { get; set; }

        public MenuItemDataTemplate()
        {
            SubItems=new List<MenuItemDataTemplate>();
        }

        public void ItemClicked(object sender, RoutedEventArgs e)
        {
            Clicked(this, e);
        }
    }
}
