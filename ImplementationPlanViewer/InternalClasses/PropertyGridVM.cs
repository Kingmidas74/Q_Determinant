using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using BasicComponentsPack.Annotations;
using Core.Atoms;
using Core.Converters;

namespace ImplementationPlanViewer.InternalClasses
{
    internal class PropertyGridVM:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void GetProperties()
        {
            var graph = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(_file.FullName), ConverterFormats.JSON);
            CountTacts = graph.GetMaxLevel();
            CountCPU = graph.GetMaxOperationsInLevel();
            Acceleration = (ulong)graph.Vertices.LongCount(x => x.Level > 0)/CountTacts;
            Effectiveness = Acceleration/CountCPU;
        }


        private System.IO.FileInfo _file;
        public System.IO.FileInfo CurrentFile { 
            get { return _file; }
            set
            {
                _file = value;
                GetProperties();
            }
        }

        private ulong _countTacts=0;
        public ulong CountTacts
        {
            get { return _countTacts; }
            set
            {
                _countTacts = value;
                OnPropertyChanged();
            }
        }

        private ulong _countCPU = 0;
        public ulong CountCPU
        {
            get { return _countCPU; }
            set
            {
                _countCPU = value;
                OnPropertyChanged();
            }
        }

        private double _acceleration = 0;
        public double Acceleration
        {
            get { return _acceleration; }
            set
            {
                _acceleration = value;
                OnPropertyChanged();
            }
        }

        private double _effectiveness = 0;
        public double Effectiveness
        {
            get { return _effectiveness; }
            set
            {
                _effectiveness = value;
                OnPropertyChanged();
            }
        }
    }
}
