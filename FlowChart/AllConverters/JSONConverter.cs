using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace FlowChart.AllConverters
{
    class JSONConverter : AbstractConverter
    {
        private JObject Serialize(IEnumerable<Block> elements, List<Link> links)
        {
            var jsonObject = new JObject();
            jsonObject["Conditions"] = new JArray();
            jsonObject["Process"] = new JArray();
            jsonObject["Input"] = new JArray();
            jsonObject["Output"] = new JArray();
            jsonObject["Start"] = new JObject();
            jsonObject["End"] = new JObject();
            foreach (var block in elements)
            {
                var e = new JObject();
                e["Id"] = block.Id;
                switch (block.Type)
                {
                    case BlockTypes.Condition:
                        e["Content"] = block.Content;
                        e["TruePath"] = links.Find(x => x.Brunch == BrunchTypes.True && x.From == block.Id).To;
                        e["FalsePath"] = links.Find(x => x.Brunch == BrunchTypes.False && x.From == block.Id).To;
                        ((JArray)jsonObject["Conditions"]).Add(e);
                        break;
                    case BlockTypes.Process:
                        e["Content"] = block.Content;
                        e["NextElement"] = links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Process"]).Add(e);
                        break;
                    case BlockTypes.Input:
                        e["Content"] = block.Content;
                        e["NextElement"] = links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Input"]).Add(e);
                        break;
                    case BlockTypes.Output:
                        e["Content"] = block.Content;
                        e["NextElement"] = links.Find(x => x.From == block.Id).To;
                        ((JArray)jsonObject["Output"]).Add(e);
                        break;
                    case BlockTypes.Start:
                        e["NextElement"] = links.Find(x => x.From == block.Id).To;
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
                        Brunch = BrunchTypes.True
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((string) element["Id"]),
                        To = ulong.Parse((string) element["FalsePath"]),
                        Brunch = BrunchTypes.False
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
                        Brunch = BrunchTypes.Null
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
                        Brunch = BrunchTypes.Null
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
                        Brunch = BrunchTypes.Null
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
                    Brunch = BrunchTypes.Null
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

        public override void SaveToFile(List<Block> elements, List<Link> links, string filePath)
        {
            File.WriteAllText(filePath, Serialize(elements,links).ToString());
        }

        public override string GetAsString(List<Block> elements, List<Link> links)
        {
            return Serialize(elements, links).ToString();
        }
    }
}
