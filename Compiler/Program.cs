using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using ActionList;
using Converters;
using Core;
using ImplementationPlan;

namespace Compiler
{
    class Program
    {
        private enum TargetState
        {
            QDeterminant,
            ImplementationPlan,
            Execute
        }

        private enum Source
        {
            File,
            Project,
            Solution
        }

        private static ulong countNode;
        private static List<Operation> Opertaions; 

        private static QDet QDeterminant;
        static void Main(string[] args)
        {
            try
            {
                var targetState = TargetState.Execute;
                var sourcePath = new StringBuilder("");
                var source = Source.File;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i].Equals("-n"))
                    {
                        countNode = ulong.Parse(args[i + 1]);
                    }
                    if (args[i].Equals("-ip"))
                    {
                        targetState = TargetState.ImplementationPlan;
                    }
                    if (args[i].Equals("-qd"))
                    {
                        targetState = TargetState.QDeterminant;
                    }
                    if (args[i].Equals("-f") || args[i].Equals("-p") || args[i].Equals("-s"))
                    {
                        sourcePath.Append(args[i + 1]);
                        if (args[i].Equals("-p"))
                        {
                            source = Source.Project;
                            if (args[i].Equals("-o"))
                            {
                                var Oconverter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
                                Oconverter.ParseDocument(args[i + 1]);
                                Opertaions = Oconverter.GetBlocks();
                            }
                        }
                        if (args[i].Equals("-s"))
                        {
                            source = Source.Solution;
                        }
                    }
                    if (args[i].Equals("-o"))
                    {
                        var Oconverter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
                        Oconverter.ParseDocument(args[i + 1]);
                        Opertaions = Oconverter.GetBlocks();
                    }
                }
                if (source == Source.File)
                {
                    if (targetState == TargetState.QDeterminant)
                    {
                        CompileFcToQd(sourcePath.ToString());
                    }
                    else
                    {
                        if (targetState == TargetState.ImplementationPlan)
                        {
                            CompileFcToIp(sourcePath.ToString());
                        }
                    }
                }
                else
                {
                    if (source == Source.Project)
                    {
                        CompileProject(targetState, sourcePath.ToString());
                    }
                    else
                    {
                        CompileSolution(targetState, sourcePath.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void CompileFcToQd(string sourcePath)
        {
            QDeterminant = new QDet();
            var FCconverter = Manufactory.CreateFlowChartConverter(ConverterTypes.XML);
            FCconverter.ParseDocument(sourcePath);
            var actionList = new AList(FCconverter.GetBlocks(), FCconverter.GetLinks(), Opertaions);
            QDeterminant = actionList.getqdet();
            var result = new StringBuilder("");
            if (QDeterminant.QDeterminant.Count > 0)
            {
                result.Append("{");
                foreach (var qterm in QDeterminant.QDeterminant)
                {
                    result.Append("(");
                    if (!String.IsNullOrEmpty(qterm.Logical))
                    {
                        result.Append(qterm.Logical).Append(",");
                    }
                    result.Append(qterm.Definitive).Append(");");
                }
                result.Remove(result.Length - 1, 1).Append("}");
            }
            Console.WriteLine("Save QD");
            File.WriteAllText(Path.GetDirectoryName(sourcePath)+@"\Qdeterminant.qd",result.ToString());
        }

        static void CompileFcToIp(string sourcePath)
        {
            CompileFcToQd(sourcePath);
            var implementationPlan = new Plan(Opertaions, QDeterminant);
            if (countNode > 0)
            {
                implementationPlan.OptimizeForMaxEffective(countNode);
            }
            var IPConverter = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            IPConverter.SetBlocks(implementationPlan.GetVertexGraph());
            IPConverter.SetLinks(implementationPlan.GetEdgesGraph());
            Console.WriteLine("Save IP");
            IPConverter.SaveToFile(Path.GetDirectoryName(sourcePath) + @"\ImplementationPlan.ip");
        }

        private static void CompileProject(TargetState toState, string sourcePath)
        {

            var directory = Path.GetDirectoryName(sourcePath);
            Console.WriteLine(directory);
            XmlDocument PrXmlDoc = new XmlDocument();
            PrXmlDoc.Load(sourcePath);
            if (File.Exists(directory + @"\ImplemetationPlan.ip"))
            {
                Console.WriteLine("RemoveIP");
                File.Delete(directory + @"\ImplemetationPlan.ip");
            }
            if (File.Exists(directory + @"\Qdeterminant.qd"))
            {
                Console.WriteLine("Remover QD");
                File.Delete(directory + @"\Qdeterminant.qd");
            }
            XmlNode newListFile = PrXmlDoc.CreateNode(XmlNodeType.Element, "Files", null);
            if (File.Exists(directory + @"\FlowChart.fc"))
            {
                Console.WriteLine("Compile fc in project");
                XmlNode fNode = PrXmlDoc.CreateNode(XmlNodeType.Element, "File", null);
                XmlAttribute path = PrXmlDoc.CreateAttribute("Path");
                path.InnerText = "FlowChart.fc";
                fNode.Attributes.Append(path);
                XmlAttribute type = PrXmlDoc.CreateAttribute("Type");
                type.InnerText = "FlowChart";
                fNode.Attributes.Append(type);
                newListFile.AppendChild(fNode);
                if (toState == TargetState.QDeterminant)
                {
                    XmlNode qNode = PrXmlDoc.CreateNode(XmlNodeType.Element, "File", null);
                    XmlAttribute qpath = PrXmlDoc.CreateAttribute("Path");
                    qpath.InnerText = "Qdeterminant.qd";
                    qNode.Attributes.Append(qpath);
                    XmlAttribute qtype = PrXmlDoc.CreateAttribute("Type");
                    qtype.InnerText = "Qdeterminant";
                    qNode.Attributes.Append(qtype);
                    newListFile.AppendChild(qNode);
                    CompileFcToQd(directory + @"\FlowChart.fc");
                }
                else
                {
                    if (toState == TargetState.ImplementationPlan)
                    {
                        XmlNode qNode = PrXmlDoc.CreateNode(XmlNodeType.Element, "File", null);
                        XmlAttribute qpath = PrXmlDoc.CreateAttribute("Path");
                        qpath.InnerText = "Qdeterminant.qd";
                        qNode.Attributes.Append(qpath);
                        XmlAttribute qtype = PrXmlDoc.CreateAttribute("Type");
                        qtype.InnerText = "Qdeterminant";
                        qNode.Attributes.Append(qtype);
                        newListFile.AppendChild(qNode);
                        XmlNode iNode = PrXmlDoc.CreateNode(XmlNodeType.Element, "File", null);
                        XmlAttribute ipath = PrXmlDoc.CreateAttribute("Path");
                        ipath.InnerText = "ImplementationPlan.ip";
                        iNode.Attributes.Append(ipath);
                        XmlAttribute itype = PrXmlDoc.CreateAttribute("Type");
                        itype.InnerText = "ImplementationPlan";
                        iNode.Attributes.Append(itype);
                        newListFile.AppendChild(iNode);
                        CompileFcToIp(directory + @"\FlowChart.fc");
                    }
                }
            }
            XmlNode rChild = PrXmlDoc.SelectSingleNode("//Files");
            PrXmlDoc.SelectSingleNode("//Project").RemoveChild(rChild);
            PrXmlDoc.SelectSingleNode("//Project").AppendChild(newListFile);
            PrXmlDoc.Save(sourcePath);
        }

        static void CompileSolution(TargetState toState, string sourcePath)
        {
            Console.WriteLine("Compile Solution");
            var directory_path = Path.GetDirectoryName(sourcePath);
            var doc = new XmlDocument();
            doc.Load(sourcePath);
            var Oconverter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
            Oconverter.ParseDocument(Path.Combine(Path.GetDirectoryName(sourcePath),doc.SelectSingleNode("//Operations").Attributes["Path"].InnerText));
            if (countNode == null)
            {
                countNode = ulong.Parse(doc.SelectSingleNode("//maxCPU").InnerText);
            }
            Opertaions = Oconverter.GetBlocks();
            foreach (XmlNode project in doc.SelectNodes("//Project"))
            {
                Console.WriteLine("Compile Project");
                CompileProject(toState,Path.Combine(directory_path,project.Attributes["Path"].InnerText));
            }
        }
    }
}
