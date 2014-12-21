using System;
using System.CodeDom;
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
using File = Core.Serializers.SerializationModels.ProjectModels.File;

namespace Compiler
{
    class Program
    {
        private static XDocument _config;
        private static Adapter<IDeterminant, IPlan> _adapter;
        private static Solution _solution;
        private static Dictionary<string, Core.Serializers.SerializationModels.ProjectModels.Project> _stackOfProjects = new Dictionary<string, Core.Serializers.SerializationModels.ProjectModels.Project>(); 
        

        private static readonly Dictionary<string, FunctionPriorities> _functionPrioritieses = new Dictionary
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
            Debug.WriteLine("GET FUNCTIONS FOR ", project.Title);
            var functions = new List<Function>();
            foreach (var reference in project.References)
            {
                var functionProject = Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(reference.ProjectPath);
                CompileProject(reference.ProjectPath);
                Graph implementationFunction = null;
                foreach (File file in functionProject.Files)
                {
                    if (file.Path.Equals("ImplementationPlan.ip"))
                    {
                        //Debug.WriteLine("GIVE IP FOR ", System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(reference.ProjectPath), file.Path)));
                        implementationFunction = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(reference.ProjectPath), file.Path)), ConverterFormats.JSON);
                        break;
                    }
                }
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
            Debug.WriteLine("COMPILE PROJECT ",projectPath);
            if (System.IO.Path.IsPathRooted(projectPath) && projectPath.Contains(@"\BasicFunctions\")) return;
            if(_stackOfProjects.ContainsKey(projectPath)) return;
            var currentProject =
                Core.Serializers.SerializationModels.ProjectModels.Project.Deserialize(
                System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path),projectPath)
                );
            Debug.WriteLine("NEED COMPILE ", currentProject.Title);
            _adapter.FlowChart = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(
                System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path),projectPath)
                    ),
                    currentProject.Files.FirstOrDefault(x => x.Path.EndsWith(".fc")).Path
                    )),
                    ConverterFormats.JSON);
            _adapter.FunctionsList=GetFunctions(currentProject);
            _adapter.CalculateDeterminant();
            _adapter.SetVariables(currentProject.SignificantVariables);
            _adapter.FindPlan();
            var result = _adapter.GetPlan();
            Debug.WriteLine(result, "PLAN ");
            
            var data = Converter.GraphToData(result, ConverterFormats.JSON);
            
            System.IO.File.WriteAllText(Path.Combine(System.IO.Path.GetDirectoryName(
                    System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path), projectPath)
                    ), "ImplementationPlan.ip"),
                data);
            _stackOfProjects.Add(projectPath,currentProject);
            try
            {
                currentProject.AddFile(new Core.Serializers.SerializationModels.ProjectModels.File
                {
                    Path = "ImplementationPlan.ip"
                });
            }
            catch
            {
            }
            finally
            {
                currentProject.Serialize(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(_solution.Path), projectPath));
            }
            Debug.WriteLine("FINISH COMPILE FOR ", projectPath);
        }

        private static void CreateAdapter()
        {
            var qDeterminantFile = Assembly.LoadFile(_config.Element("Settings").Element("QDeterminant").Attribute("Path").Value);
            IDeterminant qDeterminant = null;
            foreach (var type in qDeterminantFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IDeterminant"))))
            {
                qDeterminant = (IDeterminant)Activator.CreateInstance(type);
            }
            if (qDeterminant == null) throw new FileLoadException("QDeterminant don't load");
            var implementationPlanFile = Assembly.LoadFile(_config.Element("Settings").Element("ImplementationPlan").Attribute("Path").Value);
            IPlan implementationPlan = null;
            foreach (var type in implementationPlanFile.GetTypes().Where(type => type.GetInterfaces().Any(currentInterface => currentInterface.ToString().Equals("Core.Interfaces.IPlan"))))
            {
                implementationPlan = (IPlan)Activator.CreateInstance(type);
            }
            if (implementationPlan == null) throw new FileLoadException("ImplementationPlan don't load");
            _adapter = new Adapter<IDeterminant, IPlan>(qDeterminant, implementationPlan);
        }

        static void CompileSolution()
        {
            Console.WriteLine("Start compile solution {0}", _solution.Title);
            foreach (var project in _solution.Projects.Where(x => x.Type == Core.Serializers.SerializationModels.ProjectTypes.Algorithm))
            {
                CompileProject(project.Path);
            }
        }

        static int Main(string[] args)
        {
          /*  try
            {*/
                //Set path to DLL's
                AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
                AppDomain.CurrentDomain.AppendPrivatePath(@"core");

                //Init 
                var status = new StringBuilder(string.Empty);
                if (args.Length == 0) throw new ArgumentNullException("args");
                _solution = Solution.Deserialize(args[0]);
                _solution.Path = args[0];
                if (args.Length == 2) _solution.Properties.MaxCPU = ulong.Parse(args[1]);
                _config = XDocument.Load(@"config.xml");

                //Process
                CreateAdapter();
                CompileSolution();
                 Console.ReadLine();
                return 0;
      /*      }
            catch (ArgumentNullException exception)
            {
                Console.WriteLine("Solution not found! {0}", exception.Message);
                return 1;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Some error!");
                return 1;
            }
            finally
            {
                Console.ReadLine();
            }*/

        }
    }
}
