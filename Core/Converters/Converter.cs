using Core.Atoms;

namespace Core.Converters
{
    public class Converter
    {
        public static T DataToGraph<T>(string data, ConverterFormats format)
        {
            return ConverterFactory.GetConverter(format).DataToGraph<T>(data);
        }

        public static string GraphToData<T>(T graph, ConverterFormats format)
        {
            return ConverterFactory.GetConverter(format).GraphToData(graph);
        }
    }
}
