using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using BasicComponentsPack.Annotations;
using Core.Atoms;

namespace ImplementationPlanViewer.InternalClasses
{
    internal class VisualBlockVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Block Block;

        private bool _isSelected=false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        private double _top;

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                OnPropertyChanged();
            }
        }

        private double _left;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }

        private ulong _level;

        public ulong Level
        {
            get { return Block.Level; }
            set
            {
                Block.Level = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Link> _inLinks;

        public ObservableCollection<Link> InLinks
        {
            get { return _inLinks; }
            set
            {
                _inLinks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Link> _outLinks;

        public ObservableCollection<Link> OutLinks
        {
            get { return _outLinks; }
            set
            {
                _outLinks = value;
                OnPropertyChanged();
            }
        }

        public void ChangeSelected()
        {
            IsSelected = !IsSelected;
        }
    }
}
