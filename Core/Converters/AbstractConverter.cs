using Core.Atoms;

namespace Core.Converters
{
    internal abstract class AbstractConverter
    {
        internal abstract T DataToGraph<T>(string data);

        internal abstract string GraphToData<T>(T graph);
    }
}
