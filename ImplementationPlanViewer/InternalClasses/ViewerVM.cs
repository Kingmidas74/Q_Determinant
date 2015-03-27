﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using BasicComponentsPack.Annotations;
using Core.Atoms;
using Core.Converters;

namespace ImplementationPlanViewer.InternalClasses
{
    internal class ViewerVM:INotifyPropertyChanged
    {

        public Viewer CurrentViewer;

        private FileInfo _file;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ViewerVM()
        {
            AllElements.Add(new CollectionContainer {Collection = Blocks});
            AllElements.Add(new CollectionContainer { Collection = Links });
        }

        private ObservableCollection<LineIP> _links = new ObservableCollection<LineIP>();

        public ObservableCollection<LineIP> Links
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

        private CompositeCollection _allElements = new CompositeCollection();

        public CompositeCollection AllElements
        {
            get { return _allElements; }
            set { 
                _allElements = value; 
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
            _file = file;
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
                if (vertex.Level > 0)
                {
                    var localBlock = vertex;
                    var prevIds = graph.Edges.Where(x => x.To == localBlock.Id).Select(currentLink => currentLink.From).ToList();
                    var previousBlocks = Blocks.Where(x => prevIds.Contains((x.DataContext as VisualBlockVM).Block.Id)).ToList();
                    var x1 = previousBlocks.Aggregate<EllipseIP, double>(0, (current, previousBlock) => current + (previousBlock.DataContext as VisualBlockVM).Left)/previousBlocks.Count;
                    context.Left = x1;

                    foreach (var previousBlock in previousBlocks)
                    {
                        var _line = new LineIP();
                        var _lineContext = _line.DataContext as VisualLinkVM;
                        _lineContext.X1 = (previousBlock.DataContext as VisualBlockVM).Left + previousBlock.GetWidth()/2;
                        _lineContext.Y1 = (previousBlock.DataContext as VisualBlockVM).Top + previousBlock.GetHeight();
                        _lineContext.X2 = x1 + previousBlock.GetWidth() / 2;
                        _lineContext.Y2 = currentY;
                        Links.Add(_line);
                    }
                }
                countBlocksInLevel[vertex.Level]++;
                Blocks.Add(_ellipse);
                Level = GetLevel();
                MaxLevelToVertex = GetMaxVertexToLevel();
            }
            AllElements.Clear();
            AllElements.Add(new CollectionContainer { Collection = Blocks });
            AllElements.Add(new CollectionContainer { Collection = Links });
        }

        internal void Reload()
        {
            SetContent(_file);
        }

    }
}