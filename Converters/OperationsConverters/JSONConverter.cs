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
                    Blocks.Add(new Operation(int.Parse((string) element["Priority"]), (string) element["Signature"]));
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
