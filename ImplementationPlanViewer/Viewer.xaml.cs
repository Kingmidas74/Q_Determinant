using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Core.Atoms;
using Core.Converters;

namespace ImplementationPlanViewer
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Viewer : VisualCore.ISaveable,VisualCore.ITabContent
    {
        private string _originalFileName;
        public Viewer()
        {
            InitializeComponent();
        }



        public void SetContent(FileInfo file)
        {
            _originalFileName = file.FullName;
            ViewerContent.Children.Clear();
            var graph = Converter.DataToGraph<Graph>(File.ReadAllText(file.FullName), ConverterFormats.JSON);
            const ulong startX = 30;
            const ulong startY = 30;
            var countBlocksInLevel = new Dictionary<ulong, ulong>();
            for (ulong i = 0; i <= graph.GetMaxLevel(); i++)
            {
                countBlocksInLevel.Add(i, 0);
            }
            var grids = new List<EllipseIP>();
            foreach (var block in graph.Vertices)
            {
                var currentX = startX + (2 * startX * countBlocksInLevel[block.Level] + 20);
                var currentY = (block.Level * startY + startY + 40 * block.Level);
                var grid = CreateGrid(currentY, currentX, block.Id);
                if (block.Level > 0)
                {
                    var localBlock = block;
                    var currentLinks = graph.Edges.Where(x => x.To == localBlock.Id);
                    var currentGrids = currentLinks.Select(l => grids.FirstOrDefault(x => ulong.Parse(x.Tag.ToString()) == l.From)).ToList();
                    //double y1 = 0;
                    double x1 = currentGrids.Sum(g => Canvas.GetLeft(g));
                    x1 /= currentGrids.Count;
                    //y1 += currentGrids[0].Height;
                    foreach (var line in currentGrids.Select(g => CreateAPolyline(Canvas.GetLeft(g) + g.GetWidth() / 2, Canvas.GetTop(g) + g.GetHeight(),
                        x1 + g.GetWidth() / 2, currentY)))
                    {
                        ViewerContent.Children.Add(line);
                    }
                    grid = CreateGrid(currentY, x1, block.Id, block.Level);
                }
                countBlocksInLevel[block.Level]++;
                grid.SetContent(block.Content);
                grids.Add(grid);
            }
            foreach (var g in grids)
            {
                ViewerContent.Children.Add(g);
            }
            ViewerContent.Width = 2 * startX + (2 * startX * countBlocksInLevel[0] + 20);
            ViewerContent.Height = (graph.GetMaxLevel() * startY + 4 * startY + 40 * graph.GetMaxLevel());
        }

        private Polyline CreateAPolyline(double x1, double y1, double x2, double y2)
        {
            var arrow = new Polyline();
            arrow.Stroke = FindResource("BaseFontBrush") as SolidColorBrush;
            arrow.StrokeThickness = 0.5;

            var polygonPoints = new PointCollection {new Point(x1, y1), new Point(x2, y2)};

            arrow.Points = polygonPoints;

            return arrow;
        }

        private EllipseIP CreateGrid(double top, double left, ulong id = 0, ulong level = 0)
        {
            var grid = new EllipseIP { Tag = id };
            Canvas.SetTop(grid, top);
            Canvas.SetLeft(grid, left);
            grid.SetLevel(level);
            return grid;
        }

        public bool IsChange
        {
            get { return false; }
        }

        public void Save()
        {
            
        }

        public void ReLoad()
        {
            var file = new FileInfo(_originalFileName);
            SetContent(file);
        }
    }
}
