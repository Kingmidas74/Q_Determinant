namespace Core.Converters
{
    internal class ConverterFactory
    {
        internal static AbstractConverter GetConverter(ConverterFormats format)
        {
            switch (format)
            {
                case ConverterFormats.JSON:
                    return new JSONConverter();
                case ConverterFormats.XML:
                    return new XMLConverter();
                default:
                    return null;
            }
        }
    }

    
}
