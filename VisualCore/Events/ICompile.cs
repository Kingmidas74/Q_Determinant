using System.Windows;

namespace VisualCore.Events
{
    public interface ICompile
    {
        void BeforeCompilerListener(object sender, RoutedEventArgs e);
        void AfterCompilerListener(object sender, RoutedEventArgs e);
    }
}