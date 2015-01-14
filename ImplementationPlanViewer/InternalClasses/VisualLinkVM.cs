using System.ComponentModel;
using System.Runtime.CompilerServices;
using BasicComponentsPack.Annotations;
using Core.Atoms;

namespace ImplementationPlanViewer.InternalClasses
{
    internal class VisualLinkVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public Link Link;

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

        private double _y2;

        public double Y2
        {
            get { return _y2; }
            set
            {
                _y2 = value;
                OnPropertyChanged();
            }
        }

        private double _x1;

        public double X1
        {
            get { return _x1; }
            set
            {
                _x1 = value;
                OnPropertyChanged();
            }
        }

        private double _x2;

        public double X2
        {
            get { return _x2; }
            set
            {
                _x2 = value;
                OnPropertyChanged();
            }
        }

        private double _y1;

        public double Y1
        {
            get { return _y1; }
            set
            {
                _y1 = value;
                OnPropertyChanged();
            }
        }
    }
}
