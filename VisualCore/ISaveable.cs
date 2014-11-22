using System;

namespace VisualCore
{
    public interface ISaveable
    {
        Boolean IsChange { get; }
        void Save();
    }
}