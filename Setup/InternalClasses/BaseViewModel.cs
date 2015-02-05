using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Setup.Annotations;

namespace Setup.InternalClasses
{
    internal class BaseViewModel:INotifyPropertyChanged
    {
        private static readonly Lazy<BaseViewModel> InstanceField = new Lazy<BaseViewModel>(() => new BaseViewModel());

        private BaseViewModel()
        {
            
        }

        internal static BaseViewModel Instance
        {
            get { return InstanceField.Value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _createIcon = ProjectInstall.CreateIcon;

        public bool CreateIcon
        {
            get { return _createIcon; }
            set
            {
                _createIcon=value;
                OnPropertyChanged();
            }
        }

        private bool _createFolder = ProjectInstall.CreateFolder;

        public bool CreateFolder
        {
            get { return _createFolder; }
            set
            {
                _createFolder = value;
                OnPropertyChanged();
            }
        }

        private string _startupPath = ProjectInstall.GetFullPathToStartupDirectory();

        public string StartupPath
        {
            get { return _startupPath; }
            set
            {
                _startupPath = value;
                OnPropertyChanged();
            }
        }

        private bool _setupUpdateService = ProjectInstall.SetupUpdateService;

        public bool SetupUpdateService
        {
            get { return _setupUpdateService; }
            set
            {
                _setupUpdateService = value;
                OnPropertyChanged();
            }
        }


        private List<CustomKeyValuePair<string, bool>> _associateFiles =
            ProjectInstall.AssociateFiles.Select(
                associateFile =>
                    new CustomKeyValuePair<string, bool>() {Key = associateFile.Key, Value = associateFile.Value})
                .ToList();

        public List<CustomKeyValuePair<string, bool>> AssociateFiles
        {
            get { return _associateFiles; }
            set
            {
                _associateFiles = value;
                OnPropertyChanged();
            }
        }

        public void UpdateModel()
        {
            ProjectInstall.CreateIcon = _createIcon;
            ProjectInstall.CreateFolder = _createFolder;
            ProjectInstall.SetStartupDirectory(StartupPath);
        }



    }
}
