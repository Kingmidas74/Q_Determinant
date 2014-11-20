using BasicComponentsPack;
using VisualCore;

namespace QStudio.InternalClasses
{
    public sealed class CurrentSettings
    {
        public IWorkPlaceTabs WorkplaceTabs { get; set; }

        private static readonly CurrentSettings _instance = new CurrentSettings();
        public static CurrentSettings Instance
        {
            get
            {
                return _instance;
            }
        }
        
    }
}
