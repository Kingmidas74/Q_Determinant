using System.Windows;
using Core.Serializers;
using Core.Serializers.SerializationModels.SolutionModels;

namespace QStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompilerClick(object sender, RoutedEventArgs e)
        {
            var solution = new Solution() {Title = "Testsolution", Properties = new Core.Serializers.SerializationModels.SolutionModels.Properties() {MaxCPU = 6}};
            solution.Projects.Add(new Project() {Path = "D:\t1\t1.qpr", Title="TestProject"});
            solution.Projects.Add(new Project() { Path = "D:\t1\t2.qpr", Title = "TestProject2" });
            solution.Projects.Add(new Project() { Path = "D:\t1\t3.qpr", Title = "TestProject3" });

            var s = Factory.GeSerializer();
            s.SerializeSolution(@"D:\tempforQ\testserialization.qsln",solution);
            s.DeserializeSolution(@"D:\tempforQ\testserialization.qsln",out solution);
            MessageBox.Show(solution.Projects[1].Title);
            


        }
    }
}
