using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Core.Adapter;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("COMPILER!");
            var status = new StringBuilder("");
            var configs = XDocument.Load(@"config.xml");
            try
            {
                var qDeterminantFile = Assembly.LoadFile(configs.Element("Settings").Element("QDeterminant").Attribute("Path").Value);
                IDeterminant qDeterminant = null;
                foreach (var type in qDeterminantFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IDeterminant"))))
                {
                    qDeterminant = (IDeterminant)Activator.CreateInstance(type);
                }
                var implementationPlanFile = Assembly.LoadFile(configs.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value);
                IPlan implementationPlan = null;
                foreach (var type in implementationPlanFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IPlan"))))
                {
                    implementationPlan = (IPlan)Activator.CreateInstance(type);
                }
                var adapter = new Adapter<IDeterminant, IPlan>(qDeterminant, implementationPlan);
                adapter.SetFunctions(new List<Function>
                    {
                        new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                        new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
                    }
                );
            }
            catch (Exception e)
            {
                status.Append(e);
            }
            Console.WriteLine(status.ToString());
            Console.ReadLine();
        }
    }
}
