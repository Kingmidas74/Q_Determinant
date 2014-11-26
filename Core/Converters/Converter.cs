using Core.Atoms;

namespace Core.Converters
{
    public class Converter
    {
        public static Graph DataToGraph(string data, ConverterFormats format)
        {
            return ConverterFactory.GetConverter(format).DataToGraph(data);
        }

        public static string GraphToData(Graph graph, ConverterFormats format)
        {
            return ConverterFactory.GetConverter(format).GraphToData(graph);
        }
    }
}
