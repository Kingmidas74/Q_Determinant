namespace FlowChart.AllConverters
{
    class ConvertersFactory
    {
        public static AbstractConverter CreateConverter(ConverterTypes type)
        {
            switch (type)
            {
                case ConverterTypes.XML:
                    return CreateXMLConverter();
                case ConverterTypes.MessagePack:
                    return CreateMessagePackConverter();
                case ConverterTypes.Bin:
                    return CreateBinConverter();
                case ConverterTypes.SOAP:
                    return CreateSOAPConverter();
                case ConverterTypes.GPB:
                    return CreateGPBConverter();
                case ConverterTypes.JSON:
                    return CreateJSONConverter();
                default:
                    return null;
            }
        }

        public static AbstractConverter CreateXMLConverter()
        {
            return new XMLConverter();
        }

        public static AbstractConverter CreateMessagePackConverter()
        {
            return new MessagePackConverter();
        }

        public static AbstractConverter CreateGPBConverter()
        {
            return new GPBConverter();
        }
        public static AbstractConverter CreateJSONConverter()
        {
            return new JSONConverter();
        }

        public static AbstractConverter CreateSOAPConverter()
        {
            return new SOAPConverter();
        }

        public static AbstractConverter CreateBinConverter()
        {
            return new BinConverter();
        }
    }
}
