using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Core;
using Newtonsoft.Json.Linq;

namespace Converters.FlowchartsConverters
{
    class JSONConverter : AbstractConverterFlowcharts
    {
        private JObject Serialize()
        {
            var jsonObject = new JObject();
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
            }
            return jsonObject;
        }
        protected override bool Validation(string data)
        {
            return true; //JObject.Parse(data).IsValid(JsonSchema.Parse(File.ReadAllText(@"SchemasOfFormats/JSONScheme.json")));
        }

        protected override void Parse(string data)
        {
            if (Validation(data))
            {
                var jsonObject = JObject.Parse(data);
                foreach (var element in (JArray)jsonObject["Conditions"])
                {
                    Blocks.Add(new Block
                    {
                        Content = (string) element["Content"],
                        Id = ulong.Parse((string) element["Id"]),
                        Type = BlockTypes.Condition
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["TruePath"]),
                        Type = LinkTypes.True
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["FalsePath"]),
                        Type = LinkTypes.False
                    });
                }
                foreach (var element in (JArray)jsonObject["Process"])
                {
                    Blocks.Add(new Block
                    {
                        Content = (string) element["Content"],
                        Id = ulong.Parse((string) element["Id"]),
                        Type = BlockTypes.Process
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["NextElement"]),
                        Type = LinkTypes.Null
                    });
                }
                foreach (var element in (JArray)jsonObject["Input"])
                {
                    Blocks.Add(new Block
                    {
                        Content = (string) element["Content"],
                        Id = ulong.Parse((string) element["Id"]),
                        Type = BlockTypes.Input
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["NextElement"]),
                        Type = LinkTypes.Null
                    });
                }
                foreach (var element in (JArray)jsonObject["Output"])
                {
                    Blocks.Add(new Block
                    {
                        Content = (string) element["Content"],
                        Id = ulong.Parse((string) element["Id"]),
                        Type = BlockTypes.Output
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["NextElement"]),
                        Type = LinkTypes.Null
                    });
                }
                Blocks.Add(new Block
                {
                    Content = "Start",
                    Id = ulong.Parse((string) jsonObject["Start"]["Id"]),
                    Type = BlockTypes.Start
                });
                Links.Add(new Link
                {
                    From = ulong.Parse((string) jsonObject["Start"]["Id"]),
                    To = ulong.Parse((string) jsonObject["Start"]["NextElement"]),
                    Type = LinkTypes.Null
                });
                Blocks.Add(new Block
                {
                    Content = "End",
                    Id = ulong.Parse((string) jsonObject["End"]["Id"]),
                    Type = BlockTypes.End
                });
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
