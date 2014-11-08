using System;
using System.IO;
using ModernControls.InternalClasses;
using System.Windows;
using System.Windows.Controls;

namespace ModernControls
{
    public class ExtendedTreeViewItem : TreeViewItem
    {
        public static readonly RoutedEvent OpenDocumentEvent = EventManager.RegisterRoutedEvent("OpenDocument",
           RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtendedTabItem));
        public event RoutedEventHandler OpenDocument
        {
            add { AddHandler(OpenDocumentEvent, value); }
            remove { RemoveHandler(OpenDocumentEvent, value); }
        }
        public static readonly DependencyProperty TypeProperty =
    DependencyProperty.Register("Type", typeof(SolutionItemTypes), typeof(ExtendedTreeViewItem), new FrameworkPropertyMetadata());
        public SolutionItemTypes Type
        {
            get { return (SolutionItemTypes)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty ProjectPathProperty =
    DependencyProperty.Register("ProjectPath", typeof(string), typeof(ExtendedTreeViewItem), new FrameworkPropertyMetadata());
        public string ProjectPath
        {
            get { return (string)GetValue(ProjectPathProperty); }
            set { SetValue(ProjectPathProperty, value); }
        }
        static ExtendedTreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExtendedTreeViewItem), new FrameworkPropertyMetadata(typeof(ExtendedTreeViewItem)));
        }
        public override void OnApplyTemplate()
        {
            (GetTemplateChild("ItemButton") as Button).Click += OpenDocumentClick;
            base.OnApplyTemplate();
        }

        private void OpenDocumentClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OpenDocumentEvent, this));
        }


    }
}
