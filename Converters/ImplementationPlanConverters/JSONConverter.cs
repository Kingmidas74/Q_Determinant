using System;
using System.IO;
using System.Linq;
using Core;
using Newtonsoft.Json.Linq;

namespace Converters.ImplementationPlanConverters
{
    class JSONConverter : AbstractConverterImplementationPlan
    {
        private JArray Serialize()
        {
            var jsonArray = new JArray();
            foreach (var block in Blocks)
            {
                var jsonObject = new JObject();
                jsonObject["Id"] = block.Id;
                jsonObject["Level"] = block.Level;
                jsonObject["Content"] = block.Content;
                ulong next = 0;
                if (Links.Count > 0)
                {
                    var b = Links.FirstOrDefault(x => x.From == block.Id);
                    if (b != null)
                    {
                        next = b.To;
                    };
                }
                jsonObject["NextElement"] = next;
                jsonArray.Add(jsonObject);
            }
            /*
            jsonObject["Conditions"] = new JArray();
            jsonObject["Process"] = new JArray();
            jsonObject["Input"] = new JArray();
            jsonObject["Output"] = new JArray();
            jsonObject["Start"] = new JObject();
            jsonObject["End"] = new JObject();
            foreach (var block in Blocks)
            {
                var e = new JObject();
                e["Id"] = block.Id;
                switch (block.Type)
                {
                    case BlockTypes.Condition:
                        e["Content"] = block.Content;
                        e["TruePath"] = Links.Find(x => x.Type == LinkTypes.True && x.From == block.Id).To;
                        e["FalsePath"] = Links.Find(x => x.Type == LinkTypes.False && x.From == block.Id).To;
                        ((JArray)jsonObject["Conditions"]).Add(e);
                        break;
                    case BlockTypes.Process:
                        e["Content"] = block.Content;
                        e["NextElement"] = Links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Process"]).Add(e);
                        break;
                    case BlockTypes.Input:
                        e["Content"] = block.Content;
                        e["NextElement"] = Links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Input"]).Add(e);
                        break;
                    case BlockTypes.Output:
                        e["Content"] = block.Content;
                        e["NextElement"] = Links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Output"]).Add(e);
                        break;
                    case BlockTypes.Start:
                        e["NextElement"] = Links.Find(x => x.From == block.Id).To;
                        jsonObject["Start"] = e;
                        break;
                    case BlockTypes.End:
                        jsonObject["End"] = e;
                        break;
                }
            }*/
            return jsonArray;
        }
        protected override bool Validation(string data)
        {
            return true; //JObject.Parse(data).IsValid(JsonSchema.Parse(File.ReadAllText(@"SchemasOfFormats/JSONScheme.json")));
        }

        protected override void Parse(string data)
        {
            
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
