using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace FlowChart
{
    public class Analyzer
    {
        public Analyzer(JObject json)
        {
            Console.WriteLine("JSON");
        }

        public Analyzer(String JSONFilePath)
        {
            Console.WriteLine("JSONDOC");
        }

        private bool ValidateJSON(JObject json)
        {
            //JsonSchema schema1 = JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json"));
            return json.IsValid(JsonSchema.Parse(File.ReadAllText(@"JSONScheme.json")));
        }

        public bool An(JObject json)
        {
            return ValidateJSON(json);
        }
    }
}
