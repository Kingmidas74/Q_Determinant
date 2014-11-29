using Core.Enums;

namespace Core.Interfaces
{
    public interface IModule
    {
        StatusTypes Status { get; }
        string StatusMessage { get; }
    }
}