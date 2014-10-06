using System;
using System.Xml;
using Core;

namespace Converters.OperationsConverters
{
    class XMLConverter : AbstractConverterOperations
    {
        public override void SaveToFile(string filePath)
        {
            throw new NotImplementedException();
        }

        protected override bool Validation(string data)
        {
            return true;
        }

        protected override void Parse(string data)
        {
            
        }

        public override string GetAsString()
        {
            return null;
        }
    }
}
