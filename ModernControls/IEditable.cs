namespace ModernControls
{
    public interface IEditable
    {
        bool IsChange { get; }
        void SaveFile();
    }
}