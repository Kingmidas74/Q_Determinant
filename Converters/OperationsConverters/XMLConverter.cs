using System;
using System.Xml;
using Core;

namespace Converters.OperationsConverters
{
    class XMLConverter : AbstractConverterOperations
    {
        public override void SaveToFile(string filePath)
        {
            throw new NotImplementedException();
        }

        protected override bool Validation(string data)
        {
            return true;
        }

        protected override void Parse(string data)
        {
            if (Validation(data))
            {
                var doc = new XmlDocument();
                doc.LoadXml(data);
                var xmlNodeList = doc.SelectNodes("//Conditions/Block");
                if (xmlNodeList == null) return;
                foreach (var b in xmlNodeList)
                {
                    var xmlAttributeCollection = ((XmlNode)b).Attributes;
                    if (xmlAttributeCollection == null) continue;
                    Blocks.Add(new Block
                    {
                        Content = ((XmlNode) b).InnerText,
                        Id = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        Type = BlockTypes.Condition
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        To = ulong.Parse((xmlAttributeCollection["TruePath"].Value)),
                        Type = LinkTypes.True
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        To = ulong.Parse((xmlAttributeCollection["FalsePath"].Value)),
                        Type = LinkTypes.False
                    });
                }
                xmlNodeList = doc.SelectNodes("//Process/Block");
                if (xmlNodeList == null) return;
                foreach (var b in xmlNodeList)
                {
                    var xmlAttributeCollection = ((XmlNode)b).Attributes;
                    if (xmlAttributeCollection == null) continue;
                    Blocks.Add(new Block
                    {
                        Content = ((XmlNode)b).InnerText,
                        Id = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        Type = BlockTypes.Process
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        To = ulong.Parse((xmlAttributeCollection["NextElement"].Value))
                    });
                }
                xmlNodeList = doc.SelectNodes("//Input/Block");
                if (xmlNodeList == null) return;
                foreach (var b in xmlNodeList)
                {
                    var xmlAttributeCollection = ((XmlNode)b).Attributes;
                    if (xmlAttributeCollection == null) continue;
                    Blocks.Add(new Block
                    {
                        Content = ((XmlNode)b).InnerText,
                        Id = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        Type = BlockTypes.Input
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        To = ulong.Parse((xmlAttributeCollection["NextElement"].Value))
                    });
                }
                xmlNodeList = doc.SelectNodes("//Output/Block");
                if (xmlNodeList == null) return;
                foreach (var b in xmlNodeList)
                {
                    var xmlAttributeCollection = ((XmlNode)b).Attributes;
                    if (xmlAttributeCollection == null) continue;
                    Blocks.Add(new Block
                    {
                        Content = ((XmlNode)b).InnerText,
                        Id = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        Type = BlockTypes.Output
                    });
                    Links.Add(new Link
                    {
                        From = ulong.Parse((xmlAttributeCollection["Id"].Value)),
                        To = ulong.Parse((xmlAttributeCollection["NextElement"].Value))
                    });
                }
                var selectSingleNode = doc.SelectSingleNode("//Start");
                if (selectSingleNode == null) return;
                if (selectSingleNode.Attributes == null) return;
                Blocks.Add(new Block
                {
                    Content = "Start",
                    Id = ulong.Parse(selectSingleNode.Attributes["Id"].Value),
                    Type = BlockTypes.Start
                });
                Links.Add(new Link
                {
                    From = ulong.Parse(selectSingleNode.Attributes["Id"].Value),
                    To = ulong.Parse(selectSingleNode.Attributes["NextElement"].Value)
                });
                selectSingleNode = doc.SelectSingleNode("//End");
                if (selectSingleNode == null) return;
                if (selectSingleNode.Attributes == null) return;
                Blocks.Add(new Block
                {
                    Content = "End",
                    Id = ulong.Parse(selectSingleNode.Attributes["Id"].Value),
                    Type = BlockTypes.End
                });
                Links.Add(new Link
                {
                    From = ulong.Parse(selectSingleNode.Attributes["Id"].Value),
                    To = 0
                });
            }
            else
            {
                throw new Exception("Unvalid XML");
            }
        }

        public override string GetAsString()
        {
            return null;
        }
    }
}
