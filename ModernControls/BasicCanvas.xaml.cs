using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Converters;
using ImplementationPlan;
using ModernControls.InternalClasses;

namespace ModernControls
{
    /// <summary>
    /// Interaction logic for BasicCanvas.xaml
    /// </summary>
    public partial class BasicCanvas : UserControl
    {
        public BasicCanvas()
        {
            InitializeComponent();
        }

        public void SetFile(string filepath, SolutionItemTypes type)
        {
            switch (type)
            {
                    case SolutionItemTypes.FlowChart:
                    drawFlowChart(filepath);
                    break;
                    case SolutionItemTypes.ImplementationPlan:
                    DrawImplementationPlan(filepath);
                    break;
            }    
        }

        private void DrawImplementationPlan(string filepath)
        {
            var converter = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            converter.ParseDocument(filepath);
            var blocks = converter.GetBlocks();
            var links = converter.GetLinks();
            var graph = new Graph(blocks, links);
            ulong startX = 30;
            ulong startY = 30;
            var countBlocksInLevel = new Dictionary<ulong, ulong>();
            for (ulong i = 0; i <= graph.GetMaxLevel(); i++)
            {
                countBlocksInLevel.Add(i, 0);
            }
            var grids = new List<EllipseIP>();
            foreach (var block in blocks)
            {
                var currentX = startX + (2*startX*countBlocksInLevel[block.Level] + 20);
                var currentY = (block.Level*startY + startY + 40*block.Level);
                var grid = CreateGrid(currentY, currentX, block.Id);
                if (block.Level > 0)
                {
                    var currentLinks = links.Where(x => x.To == block.Id);
                    var currentGrids = currentLinks.Select(l => grids.FirstOrDefault(x => ulong.Parse(x.Tag.ToString()) == l.From)).ToList();
                    double x1 = 0;
                    double y1 = 0;
                    foreach (var g in currentGrids)
                    {
                        x1 += Canvas.GetLeft(g);
                        y1 = Canvas.GetTop(g);
                    }
                    x1 /= currentGrids.Count;
                    y1 += currentGrids[0].Height;
                    foreach (var g in currentGrids)
                    {
                        var line = CreateAPolyline(Canvas.GetLeft(g) + g.GetWidth()/2, Canvas.GetTop(g) + g.GetHeight(),
                            x1 + g.GetWidth()/2, currentY);
                        ImageCanvas.Children.Add(line);
                    }
                    grid = CreateGrid(currentY, x1, block.Id, block.Level);
                }
                countBlocksInLevel[block.Level]++;
                grid.SetContent(block.Content);
                grids.Add(grid);
            }
            foreach (var g in grids)
            {
                ImageCanvas.Children.Add(g);
            }
            ImageCanvas.Width = 2*startX + (2 * startX * countBlocksInLevel[0] + 20);
            ImageCanvas.Height = (graph.GetMaxLevel() * startY + 4*startY + 40 * graph.GetMaxLevel());
            
        }

        private Polyline CreateAPolyline(double x1, double y1, double x2, double y2)
        {
            var arrow = new Polyline();
            arrow.Stroke = FindResource("BasicFontColorBrush") as SolidColorBrush;
            arrow.StrokeThickness = 0.5;
            
            var polygonPoints = new PointCollection();
            polygonPoints.Add(new Point(x1, y1));
            polygonPoints.Add(new Point(x2, y2));

            arrow.Points = polygonPoints;

            return arrow;
        } 

        private EllipseIP CreateGrid(double top, double left, ulong id=0, ulong level=0)
        {
            var grid = new EllipseIP {Tag = id};
            Canvas.SetTop(grid, top);
            Canvas.SetLeft(grid, left);
            grid.SetLevel(level);
            return grid;
        }


        private void drawFlowChart(string filepath)
        {
            MessageBox.Show("Временно недоступно");
        }
    }
}
