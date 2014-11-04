using Core;

namespace ModernControls.InternalClasses
{
    public sealed class Singleton
    {
        static readonly Singleton MyInstance = new Singleton();

        public static BlockTypes CurrentBlockType { get; set; }

        static Singleton() { }

        Singleton() { }

        public static Singleton myInstance
        {
            get
            {
                return MyInstance;
            }
        }
    }  
}
