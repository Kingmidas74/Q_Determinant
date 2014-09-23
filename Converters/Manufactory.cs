using Converters.ActionListsConverters;
using Converters.FlowchartsConverters;

namespace Converters
{
    public class Manufactory
    {
        public static IFlowchartsConverter CreateFlowChartConverter(ConverterTypes type)
        {
            switch (type)
            {
                case ConverterTypes.XML:
                    return  new FlowchartsConverters.XMLConverter();
                case ConverterTypes.MessagePack:
                    return new FlowchartsConverters.MessagePackConverter();
                case ConverterTypes.Bin:
                    return new FlowchartsConverters.BinConverter();
                case ConverterTypes.SOAP:
                    return new FlowchartsConverters.SOAPConverter();
                case ConverterTypes.GPB:
                    return new FlowchartsConverters.GPBConverter();
                case ConverterTypes.JSON:
                    return new FlowchartsConverters.JSONConverter();
                default:
                    return null;
            }
        }

        public static IActionListsConverter CreateActionListConverter(ConverterTypes type)
        {
            switch (type)
            {
                case ConverterTypes.XML:
                    return new ActionListsConverters.XMLConverter();
                case ConverterTypes.MessagePack:
                    return new ActionListsConverters.MessagePackConverter();
                case ConverterTypes.Bin:
                    return new ActionListsConverters.BinConverter();
                case ConverterTypes.SOAP:
                    return new ActionListsConverters.SOAPConverter();
                case ConverterTypes.GPB:
                    return new ActionListsConverters.GPBConverter();
                case ConverterTypes.JSON:
                    return new ActionListsConverters.JSONConverter();
                default:
                    return null;
            }
        }
    }
}
