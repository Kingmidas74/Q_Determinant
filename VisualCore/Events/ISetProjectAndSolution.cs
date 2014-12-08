using System.Windows;

namespace VisualCore.Events
{
    public interface ISetProjectAndSolution
    {
        void SetProjectListener(object sender, RoutedEventArgs e);
        void SetSolutionListener(object sender, RoutedEventArgs e);
    }
}