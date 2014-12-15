using System.Collections.ObjectModel;
using System.ComponentModel;

namespace BasicComponentsPack.InternalClasses
{
    public class ConsoleContent : INotifyPropertyChanged
    {
        string _consoleInput = string.Empty;
        ObservableCollection<string> _consoleOutput = new ObservableCollection<string>();

        public string ConsoleInput
        {
            get
            {
                return _consoleInput;
            }
            set
            {
                _consoleInput = value;
                OnPropertyChanged("ConsoleInput");
            }
        }

        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return _consoleOutput;
            }
            set
            {
                _consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
