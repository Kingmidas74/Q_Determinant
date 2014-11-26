using Core.Atoms;

namespace Core.Converters
{
    internal abstract class AbstractConverter
    {
        internal abstract Graph DataToGraph(string data);

        internal abstract string GraphToData(Graph graph);
    }
}
