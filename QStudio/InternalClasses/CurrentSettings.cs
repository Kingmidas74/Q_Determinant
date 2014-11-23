namespace QStudio.InternalClasses
{
    public sealed class CurrentSettings
    {

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
