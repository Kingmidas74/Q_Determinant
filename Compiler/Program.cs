using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Core.Adapter;
using Core.Atoms;
using Core.Converters;
using Core.Interfaces;
using Core.Serializers;
using Core.Serializers.SerializationModels.ProjectModels;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Compiler
{
    class Program
    {
        private static Adapter<IDeterminant, IPlan> _adapter;
        private static string _solutionPath;

        private static List<Function> GetFunctions(Solution solution)
        {
            var functions = new List<Function>
                {
                    new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                    new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
                };
            return functions;
        } 

        private static void CreateAdapter(XContainer configXml,Solution solution)
        {
            var qDeterminantFile = Assembly.LoadFile(configXml.Element("Settings").Element("QDeterminant").Attribute("Path").Value);
            IDeterminant qDeterminant = null;
            foreach (var type in qDeterminantFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IDeterminant"))))
            {
                qDeterminant = (IDeterminant)Activator.CreateInstance(type);
            }
            var implementationPlanFile = Assembly.LoadFile(configXml.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value);
            IPlan implementationPlan = null;
            foreach (var type in implementationPlanFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IPlan"))))
            {
                implementationPlan = (IPlan)Activator.CreateInstance(type);
            }
            var functions = GetFunctions(solution);
            _adapter = new Adapter<IDeterminant, IPlan>(qDeterminant, implementationPlan, functions);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("COMPILER!");
            var status = new StringBuilder("");
            try
            {
                var configs = XDocument.Load(@"config.xml");
                _solutionPath = args[0];
                Solution solution;
                SerializersFactory.GetSerializer().DeserializeSolution(_solutionPath, out solution);
                CreateAdapter(configs,solution);
                _adapter.CalculateDeterminant();
                _adapter.FindPlan();
                var result = _adapter.GetPlan();
                var data = Converter.GraphToData(result, ConverterFormats.JSON);
                System.IO.File.WriteAllText(@"C:\Users\Денис\SkyDrive\Old\Q_Determinant\TestSolutionDefault\NewProject\ImplementationPlan.ip", data);
                Console.WriteLine(_adapter.CountTacts);
                Console.WriteLine(_adapter.CountCPU);

            }
            catch (Exception e)
            {
                status.Append(e);
            }
            Console.WriteLine(status.ToString());
        }
    }
}
