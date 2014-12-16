using System;
using System.IO;
using Core.Atoms;
using System.Xml.Serialization;

namespace Core.Converters
{
    internal class XMLConverter : AbstractConverter
    {
        internal override T DataToGraph<T>(string data)
        {
            return default(T);
        }

        internal override string GraphToData<T>(T graph)
        {
            var textWriter = new StringWriter();
            (new XmlSerializer(typeof(T))).Serialize(textWriter, graph);
            return textWriter.ToString();
        }
    }
}
