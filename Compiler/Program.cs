using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

        static private ulong countNode = 0;
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
                        }
                        if (args[i].Equals("-s"))
                        {
                            source = Source.Solution;
                        }
                    }
                    if (args[i].Equals("-o"))
                    {
                        var Oconverter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
                        Oconverter.ParseDocument(args[i+1]);
                        Opertaions = Oconverter.GetBlocks();
                    }
                }
                if (Opertaions == null)
                {
                    throw new Exception("Не удалось найти список операций");
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
                Console.ReadLine();
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
            File.WriteAllText(Path.GetDirectoryName(sourcePath)+@"\QDeterminant.qd",result.ToString());
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

        static void CompileProject(TargetState toState, string sourcePath)
        {
            
            var directory = Path.GetDirectoryName(sourcePath);
            Console.WriteLine(directory);
            if (File.Exists(directory + @"\ImplemetationPlan.ip"))
            {
                Console.WriteLine("RemoveIP");
                File.Delete(directory + @"\ImplemetationPlan.ip");
            }
            if (File.Exists(directory + @"\QDeterminant.qd"))
            {
                Console.WriteLine("Remover QD");
                File.Delete(directory + @"\QDeterminant.qd");
            }
            if (File.Exists(directory + @"\FlowChart.fc"))
            {
                Console.WriteLine("Compile fc in project");
                if (toState == TargetState.QDeterminant)
                {
                    CompileFcToQd(directory + @"\FlowChart.fc");
                }
                else
                {
                    if (toState == TargetState.ImplementationPlan)
                    {
                        CompileFcToIp(directory + @"\FlowChart.fc");
                    }
                }
            }
        }


        static void CompileSolution(TargetState toState, string sourcePath)
        {
            Console.WriteLine("Compile Solution");
            var directory_path = Path.GetDirectoryName(sourcePath);
            var doc = new XmlDocument();
            doc.Load(sourcePath);
            foreach (XmlNode project in doc.SelectNodes("//Project"))
            {
                Console.WriteLine("Compile Project");
                CompileProject(toState,Path.Combine(directory_path,project.Attributes["Path"].InnerText));
            }
        }
    }
}
