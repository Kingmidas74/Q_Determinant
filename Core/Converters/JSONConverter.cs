using System.Diagnostics;
using Core.Atoms;
using Newtonsoft.Json;

namespace Core.Converters
{
    internal class JSONConverter:AbstractConverter
    {

        internal override Graph DataToGraph(string data)
        {
            return JsonConvert.DeserializeObject<Graph>(data);
        }

        internal override string GraphToData(Graph graph)
        {
            return JsonConvert.SerializeObject(graph);
        }
    }
}
