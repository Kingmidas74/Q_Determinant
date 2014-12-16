﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Core.Adapter;
using Core.Converters;
using Core.Interfaces;
using Core.Serializers;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Compiler
{
    class Program
    {
        private static Adapter<IDeterminant, IPlan> _adapter;
        private static string _solutionPath;
        private static ulong _maxCPU;

        private static Dictionary<string, FunctionPriorities> _functionPrioritieses = new Dictionary
            <string, FunctionPriorities>
        {
            {"||",FunctionPriorities.First},{"&&",FunctionPriorities.First},{"!",FunctionPriorities.First},
            {">",FunctionPriorities.Second},{"<",FunctionPriorities.Second},{">=",FunctionPriorities.Second},{"<=",FunctionPriorities.Second},
            {"==",FunctionPriorities.Second},{"!=",FunctionPriorities.Second},
            {"+",FunctionPriorities.Third},{"-",FunctionPriorities.Third},
            {"*",FunctionPriorities.Fourth},{"/",FunctionPriorities.Fourth},
        };

        private static List<Function> GetFunctions(Core.Serializers.SerializationModels.ProjectModels.Project project)
        {
            Console.WriteLine("NEED FUNCTIONS TO PROJECT: {0}",project.Title);
                var functions = new List<Function>();
                foreach (var reference in project.References)
                {
                    Console.WriteLine("Reference: {0}", reference.ProjectPath);
                    Core.Serializers.SerializationModels.ProjectModels.Project functionProject;
                    SerializersFactory.GetSerializer().DeserializeProject(reference.ProjectPath, out functionProject);
                    CompileProject(reference.ProjectPath);
                    var implementationFunction = (from file in functionProject.Files where Path.GetExtension(file.Path).Equals(".ip") select Converter.DataToGraph(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(reference.ProjectPath), file.Path)), ConverterFormats.JSON)).FirstOrDefault();
                    functions.Add(new Function { 
                        Signature = functionProject.Title, 
                        Parameters = (ulong) implementationFunction.Vertices.LongCount(x => x.Level == 0),
                        Priority = _functionPrioritieses.ContainsKey(functionProject.Title)
                            ? _functionPrioritieses[functionProject.Title]
                            : FunctionPriorities.Fifth
                    });
                }
            return functions;
        }

        private static void CompileProject(string projectPath)
        {
            if (projectPath.Contains(@"\BasicFunctions\")) return;
            projectPath = Path.Combine(Path.GetDirectoryName(_solutionPath), projectPath);
            Console.WriteLine("Compile project: {0}", projectPath);
            var configs = XDocument.Load(@"config.xml");
            Console.WriteLine("CCreate" + projectPath);
            Core.Serializers.SerializationModels.ProjectModels.Project currentProject;
            SerializersFactory.GetSerializer().DeserializeProject(projectPath, out currentProject);
            Console.WriteLine("CCreate" + projectPath);
            CreateAdapter(configs, currentProject);
            var flowchartFilePath = Path.Combine(Path.GetDirectoryName(projectPath),
                currentProject.Files.First(x => Path.GetExtension(x.Path).Equals(".fc")).Path);
            _adapter.FlowChart = Converter.DataToGraph(System.IO.File.ReadAllText(flowchartFilePath),
                ConverterFormats.JSON);
            Console.WriteLine("Adapter created");
            _adapter.CalculateDeterminant();
            _adapter.SetVariables(currentProject.SignificantVariables);
            _adapter.FindPlan();
            Console.WriteLine("OPT");
            _adapter.OptimizePlan(_maxCPU);
            Console.WriteLine("OPT");
            var result = _adapter.GetPlan();
            var data = Converter.GraphToData(result, ConverterFormats.JSON);
            System.IO.File.WriteAllText(Path.Combine(Path.GetDirectoryName(projectPath), "ImplementationPlan.ip"),
                data);
            currentProject.Files.Clear();
            currentProject.Files.Add(new Core.Serializers.SerializationModels.ProjectModels.File
            {
                Path = Path.Combine(Path.GetDirectoryName(projectPath), "FlowChart.fc")
            });
            currentProject.Files.Add(new Core.Serializers.SerializationModels.ProjectModels.File
            {
                Path = Path.Combine(Path.GetDirectoryName(projectPath), "ImplementationPlan.ip")
            });
            SerializersFactory.GetSerializer().SerializeProject(projectPath, currentProject);
        }

        private static void CreateAdapter(XContainer configXml, Core.Serializers.SerializationModels.ProjectModels.Project project)
        {
            Console.WriteLine("PATH TO Q"+configXml.Element("Settings").Element("QDeterminant").Attribute("Path").Value);
            var qDeterminantFile = Assembly.LoadFile(configXml.Element("Settings").Element("QDeterminant").Attribute("Path").Value);
            IDeterminant qDeterminant = null;
            foreach (var type in qDeterminantFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IDeterminant"))))
            {
                qDeterminant = (IDeterminant)Activator.CreateInstance(type);
            }
            Console.WriteLine("QCreate");
            var implementationPlanFile = Assembly.LoadFile(configXml.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value);
            IPlan implementationPlan = null;
            foreach (var type in implementationPlanFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IPlan"))))
            {
                implementationPlan = (IPlan)Activator.CreateInstance(type);
            }
            Console.WriteLine("IPCreate");
            var functions = GetFunctions(project);
            _adapter = new Adapter<IDeterminant, IPlan>(qDeterminant, implementationPlan, functions);
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
            Console.WriteLine("COMPILER!");
            var status = new StringBuilder("");
            try
            {
                _solutionPath = args[0];
                _maxCPU = ulong.Parse(args[1]);
                /*_solutionPath = @"D:\tempforQ\02122014\SV.qsln";
                _maxCPU = 2;*/
                Core.Serializers.SerializationModels.SolutionModels.Solution solution;
                SerializersFactory.GetSerializer().DeserializeSolution(_solutionPath, out solution);
                foreach (var project in solution.Projects.Where(x=>x.Type==Core.Serializers.SerializationModels.ProjectTypes.Algorithm))
                {
                    CompileProject(project.Path);
                }

            }
            catch (Exception e)
            {
                status.Append(e);
            }
            Console.WriteLine(status.ToString());
           // Console.ReadLine();
        }
    }
}
