using Converters.ActionListsConverters;
using Converters.FlowchartsConverters;
using Converters.ImplementationPlanConverters;
using Converters.OperationsConverters;

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

        public static IOperationsConverter CreateOperationConverter(ConverterTypes type)
        {
            switch (type)
            {
                case ConverterTypes.XML:
                    return new OperationsConverters.XMLConverter();
                case ConverterTypes.MessagePack:
                    return new OperationsConverters.MessagePackConverter();
                case ConverterTypes.Bin:
                    return new OperationsConverters.BinConverter();
                case ConverterTypes.SOAP:
                    return new OperationsConverters.SOAPConverter();
                case ConverterTypes.GPB:
                    return new OperationsConverters.GPBConverter();
                case ConverterTypes.JSON:
                    return new OperationsConverters.JSONConverter();
                default:
                    return null;
            }
        }

        public static IImplementationPlanConverter CreateImplementationPlanConverter(ConverterTypes type)
        {
            switch (type)
            {
                case ConverterTypes.XML:
                    return new ImplementationPlanConverters.XMLConverter();
                case ConverterTypes.MessagePack:
                    return new ImplementationPlanConverters.MessagePackConverter();
                case ConverterTypes.Bin:
                    return new ImplementationPlanConverters.BinConverter();
                case ConverterTypes.SOAP:
                    return new ImplementationPlanConverters.SOAPConverter();
                case ConverterTypes.GPB:
                    return new ImplementationPlanConverters.GPBConverter();
                case ConverterTypes.JSON:
                    return new ImplementationPlanConverters.JSONConverter();
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
