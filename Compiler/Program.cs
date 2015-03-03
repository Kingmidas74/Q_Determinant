using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using Core.Adapter;
using Core.Atoms;
using Core.Converters;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Compiler
{
    class Program
    {
        private static XDocument _config;
        private static Solution _solution;
        private static readonly Dictionary<string, Core.Serializers.SerializationModels.ProjectModels.Project> StackOfProjects = new Dictionary<string, Core.Serializers.SerializationModels.ProjectModels.Project>(); 
        

        private static readonly Dictionary<string, FunctionPriorities> FunctionPrioritieses = new Dictionary
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
            var functions = new List<Function>();
            foreach (var reference in project.References)
            {
                CompileProject(reference.ProjectPath);
                var functionProject = Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(reference.ProjectPath);
                var implementationFunction = (from file in functionProject.Files where file.Path.Equals("ImplementationPlan.ip") select Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(reference.ProjectPath), file.Path)), ConverterFormats.JSON)).FirstOrDefault();
                functions.Add(new Function { 
                    Signature = functionProject.Title, 
                    Parameters = (ulong) implementationFunction.Vertices.LongCount(x => x.Level == 0),
                    Priority = FunctionPrioritieses.ContainsKey(functionProject.Title)
                        ? FunctionPrioritieses[functionProject.Title]
                        : FunctionPriorities.Fifth
                });
            }
            return functions;
        }


        private static void CompileProject(string projectPath)
        {
            var adapter = CreateAdapter();
            if (System.IO.Path.IsPathRooted(projectPath) && projectPath.Contains(@"\BasicFunctions\")) return;
            if(StackOfProjects.ContainsKey(projectPath)) return;
            var currentProject =
                Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path),projectPath)
                );
            Console.WriteLine("Set FlowChart");
            adapter.FlowChart = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path),projectPath)
                    ),
                    currentProject.Files.FirstOrDefault(x => x.Path.EndsWith(".fc")).Path
                    )),
                    ConverterFormats.JSON);
            Console.WriteLine("Inject dependency");
            adapter.FunctionsList=GetFunctions(currentProject);
            Console.WriteLine("Calculate Determinant");
            adapter.CalculateDeterminant();
            if (adapter.GetVariables().Count > 0)
            {
                Console.WriteLine("VAR EXIST");
                if (currentProject.SignificantVariables.Count(x => string.IsNullOrEmpty(x.Value)) > 0)
                {
                    throw new Exception("NV");    
                }
                adapter.SetVariables(currentProject.SignificantVariables);
                adapter.CalculateDeterminant();
            }
            Console.WriteLine("Find Plan");
            adapter.FindPlan();
            
            //_adapter.OptimizePlan(_solution.Properties.MaxCPU);
            //_adapter.OptimizePlan(1);
            Console.WriteLine("Get Plan");
            var result = adapter.GetPlan();
            Console.WriteLine("Save Plan");
            var data = Converter.GraphToData(result, ConverterFormats.JSON);
            
            System.IO.File.WriteAllText(Path.Combine(System.IO.Path.GetDirectoryName(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path), projectPath)
                    ), "ImplementationPlan.ip"),
                data);
            
            if (currentProject.Files.Count(x => x.Path.Equals("ImplementationPlan.ip")) == 0)
            {
                currentProject.AddFile(new Core.Serializers.SerializationModels.ProjectModels.File
                {
                    Path = "ImplementationPlan.ip"
                });
                Debug.WriteLine("NEED SER", System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path), projectPath));
                currentProject.Serialize(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path), projectPath));
            }
            StackOfProjects.Add(projectPath, currentProject);
        }

        private static Adapter<IDeterminant, IPlan> CreateAdapter()
        {
            var qDeterminantFile = Assembly.LoadFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            @"QStudio","libs", _config.Element("Settings").Element("QDeterminant").Attribute("Path").Value));
            IDeterminant qDeterminant = null;
            foreach (var type in qDeterminantFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IDeterminant"))))
            {
                qDeterminant = (IDeterminant)Activator.CreateInstance(type);
            }
            if (qDeterminant == null) throw new FileLoadException("QDeterminant don't load");
            var implementationPlanFile = Assembly.LoadFile(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            @"QStudio", "libs", _config.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value));
            IPlan implementationPlan = null;
            foreach (var type in implementationPlanFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IPlan"))))
            {
                implementationPlan = (IPlan)Activator.CreateInstance(type);
            }
            if (implementationPlan == null) throw new FileLoadException("ImplementationPlan don't load");
            return  new Adapter<IDeterminant, IPlan>(qDeterminant, implementationPlan);
        }

        static void CompileSolution()
        {
            Console.WriteLine("Start compile solution {0}", _solution.Title);
            foreach (var project in _solution.Projects.Where(x => x.Type == Core.Serializers.SerializationModels.ProjectTypes.Algorithm))
            {
                Console.WriteLine("Start compile project {0}", project.Title);
                CompileProject(project.Path);
                Console.WriteLine("Finish compile project {0}", project.Title);
            }
        }

        static int Main(string[] args)
        {
            try
            {
                //Set path to DLL's
                AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
                AppDomain.CurrentDomain.AppendPrivatePath(@"core");

                //Init 
                var status = new StringBuilder(string.Empty);
                if (args.Length == 0) throw new ArgumentNullException("args");
                _solution = Solution.Deserialize(args[0]);
                _solution.Path = args[0];
                if (args.Length == 2) _solution.Properties.MaxCPU = ulong.Parse(args[1]);
                _config = XDocument.Load(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
            @"QStudio", "config.xml"));

                //Process
                CompileSolution();
                return 0;
            }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine("Solution not found! {0}", exception.Message);
                return 1;
            }
            catch (Exception exception)
            {
                if (exception.Message.Equals("NV"))
                {
                    return 2;
                }
                Console.WriteLine(exception.Message);
                return 1;
            }
        }
    }
}
