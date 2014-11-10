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
            var CountBlocksInLevel = new Dictionary<ulong, ulong>();
            for (ulong i = 0; i <= graph.GetMaxLevel(); i++)
            {
                CountBlocksInLevel.Add(i, 0);
            }
            var grids = new List<Grid>();
            foreach (var block in blocks)
            {
                var currentX = startX + (2*startX*CountBlocksInLevel[block.Level] + 20);
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
                        var line = CreateAPolyline(Canvas.GetLeft(g) + g.Width/2, Canvas.GetTop(g) + g.Height,
                            x1 + g.Width/2, currentY);
                        ImageCanvas.Children.Add(line);
                    }
                    grid = CreateGrid(currentY, x1, block.Id);
                }

                CountBlocksInLevel[block.Level]++;
                grid.Children.Add(CreateEllipse());
                grid.Children.Add(CreateTb(block.Content));
                grids.Add(grid);
            }
            foreach (var g in grids)
            {
                ImageCanvas.Children.Add(g);
            }
            ImageCanvas.Width = 2*startX + (2 * startX * CountBlocksInLevel[0] + 20);
            ImageCanvas.Height = (graph.GetMaxLevel() * startY + 4*startY + 40 * graph.GetMaxLevel());
            
        }

        private Polyline CreateAPolyline(double x1, double y1, double x2, double y2)
        {
            var arrow = new Polyline();
            arrow.Stroke = FindResource("WindowBackgroundBrush") as SolidColorBrush;
            arrow.StrokeThickness = 0.5;
            
            var polygonPoints = new PointCollection();
            polygonPoints.Add(new Point(x1, y1));
            polygonPoints.Add(new Point(x2, y2));

            arrow.Points = polygonPoints;

            return arrow;
        } 

        private Grid CreateGrid(double top, double left, ulong id=0)
        {
            var grid = new Grid { Width = 40, Height = 40, Tag = id};
            Canvas.SetTop(grid, top);
            Canvas.SetLeft(grid, left);
            return grid;
        }

        private Ellipse CreateEllipse()
        {
            return new Ellipse
            {
                Width = 40,
                Height = 40,
                Stroke = FindResource("WindowBackgroundBrush") as SolidColorBrush,
                StrokeThickness = 0.5
            };
        }

        private TextBlock CreateTb(string content)
        {
            return new TextBlock
            {
                FontSize = 14,
                Foreground = FindResource("WindowBackgroundBrush") as SolidColorBrush,
                Text = content,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
        }

        private void drawFlowChart(string filepath)
        {
            MessageBox.Show("Временно недоступно");
        }
    }
}
