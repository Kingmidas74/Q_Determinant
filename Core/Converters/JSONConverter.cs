using Newtonsoft.Json;

namespace Core.Converters
{
    internal class JSONConverter:AbstractConverter
    {

        internal override T DataToGraph<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        internal override string GraphToData<T>(T graph)
        {
            return JsonConvert.SerializeObject(graph);
        }
    }
}
