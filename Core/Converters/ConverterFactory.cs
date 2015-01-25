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
                case ConverterFormats.MessagePack:
                    return new MessagePackConverter();
                default:
                    return null;
            }
        }
    }

    
}
