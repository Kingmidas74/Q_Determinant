using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private bool _isSelected;

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

        private ulong _top;

        public ulong Top
        {
            get { return _top; }
            set
            {
                _top = value;
                OnPropertyChanged();
            }
        }

        private ulong _left;

        public ulong Left
        {
            get { return _left; }
            set
            {
                _left = value;
                OnPropertyChanged();
            }
        }
    }
}
