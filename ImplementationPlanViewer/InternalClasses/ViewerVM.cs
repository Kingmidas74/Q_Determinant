using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BasicComponentsPack.Annotations;
using Core.Atoms;
using Core.Converters;

namespace ImplementationPlanViewer.InternalClasses
{
    internal class ViewerVM:INotifyPropertyChanged
    {

        public Viewer CurrentViewer;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Link> _links = new ObservableCollection<Link>();

        public ObservableCollection<Link> Links
        {
            get { return _links; }
            set { 
                _links = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<EllipseIP> _blocks = new ObservableCollection<EllipseIP>();

        public ObservableCollection<EllipseIP> Blocks
        {
            get { return _blocks; }
            set
            {
                _blocks = value;
                Level = GetLevel();
                MaxLevelToVertex = GetMaxVertexToLevel();
                OnPropertyChanged();
            }
        }

        private ulong _level = 1;

        public ulong Level
        {
            get { return _level; }
            set
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        private ulong _maxLevelToVertex = 1;

        public ulong MaxLevelToVertex
        {
            get { return _maxLevelToVertex; }
            set
            {
                _maxLevelToVertex = value;
                OnPropertyChanged();
            }
        }

        private ulong GetLevel()
        {
            return Blocks.Max(x => (x.DataContext as VisualBlockVM).Block.Level) + 1;
        }

        private ulong GetMaxVertexToLevel()
        {
            return (ulong)(Blocks.LongCount(x => (x.DataContext as VisualBlockVM).Block.Level == 0));
        }


        internal void SetContent(System.IO.FileInfo file)
        {
            Blocks.Clear();
            Links.Clear();
            const ulong startX = 30;
            const ulong startY = 30;
            var graph = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(file.FullName), ConverterFormats.JSON);
            var countBlocksInLevel = new Dictionary<ulong, ulong>();
            for (ulong i = 0; i <= graph.GetMaxLevel(); i++)
            {
                countBlocksInLevel.Add(i, 0);
            }
            foreach (var vertex in graph.Vertices)
            {
                
                var currentX = startX + (2 * startX * countBlocksInLevel[vertex.Level] + 20);
                var currentY = (vertex.Level * startY + startY + 40 * vertex.Level);
                var _ellipse = new EllipseIP();
                var context = (_ellipse.DataContext as VisualBlockVM);
                context.Block = vertex;
                context.Top = currentY;
                context.Left = currentX;
                context.Content = vertex.Content;
               /* if (vertex.Level > 0)
                {
                    var localBlock = vertex;
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
                }*/
                countBlocksInLevel[vertex.Level]++;
                Blocks.Add(_ellipse);
                Level = GetLevel();
                MaxLevelToVertex = GetMaxVertexToLevel();
            }
        }
    }
}
