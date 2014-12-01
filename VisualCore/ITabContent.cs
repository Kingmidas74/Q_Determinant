using System.IO;

namespace VisualCore
{
    public interface ITabContent
    {
        void SetContent(FileInfo file);
        void ReLoad();
    }
}