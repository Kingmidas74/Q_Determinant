using Core.Enums;

namespace Core
{
    public interface IModule
    {
        StatusTypes Status { get; }
        string StatusMessage { get; }
    }
}