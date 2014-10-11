using System;
using System.IO;
using Core;
using Newtonsoft.Json.Linq;

namespace Converters.OperationsConverters
{
    class JSONConverter : AbstractConverterOperations
    {
        private JObject Serialize()
        {
            return new JObject();
        }
        protected override bool Validation(string data)
        {
            return true; //JObject.Parse(data).IsValid(JsonSchema.Parse(File.ReadAllText(@"SchemasOfFormats/JSONScheme.json")));
        }

        protected override void Parse(string data)
        {
            if (Validation(data))
            {
                var jsonObject = JArray.Parse(data);
                foreach (var element in jsonObject)
                {
                    var priority = int.Parse((string)element["Priority"]);
                    var signature = (string) element["Signature"];
                    var isUnary = bool.Parse((string) element["isUnary"]);
                    Blocks.Add(new Operation(priority,signature,isUnary)); 
                }
            }
            else
            {
                throw new Exception("Unvalid JSON");
            }
        }

        public override void SaveToFile(string filePath)
        {
            File.WriteAllText(filePath, Serialize().ToString());
        }

        public override string GetAsString()
        {
            return Serialize().ToString();
        }
    }
}
