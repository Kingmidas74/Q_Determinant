using System;
using System.IO;
using Core.Atoms;
using System.Xml.Serialization;

namespace Core.Converters
{
    internal class XMLConverter : AbstractConverter
    {
        internal override Graph DataToGraph(string data)
        {
            return null;
        }

        internal override string GraphToData(Graph graph)
        {
            var textWriter = new StringWriter();
            (new XmlSerializer(typeof(Graph))).Serialize(textWriter, graph);
            return textWriter.ToString();
        }
    }
}
