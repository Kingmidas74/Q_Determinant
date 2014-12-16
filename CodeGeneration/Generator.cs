using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Xsl;
using CodeGeneration.InternalClasses;
using Core.Atoms;
using Core.Converters;
using Core.Serializers;
using Core.Serializers.SerializationModels;

namespace CodeGeneration
{
    internal class Generator
    {
        private static readonly string AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static void GenerateByProject(string currentProjectPath)
        {
            Core.Serializers.SerializationModels.ProjectModels.Project project;
            SerializersFactory.GetSerializer().DeserializeProject(currentProjectPath,out project);
            var pathToFile = project.Files.Where(x => System.IO.Path.GetExtension(x.Path).Equals(".ip") || System.IO.Path.GetExtension(x.Path).Equals(".tip")).Select(plan => GenerateByPlan(plan.Path)).ToList();
            foreach (var path in pathToFile)
            {
                project.Files.Add(new Core.Serializers.SerializationModels.ProjectModels.File{Path = path});
            }
            SerializersFactory.GetSerializer().SerializeProject(currentProjectPath, project);
        }

        internal static void GenerateBySolution(string currentSolutionPath)
        {
            Core.Serializers.SerializationModels.SolutionModels.Solution solution;
            SerializersFactory.GetSerializer().DeserializeSolution(currentSolutionPath, out solution);
            foreach (var project in solution.Projects.Where(x=>x.Type==ProjectTypes.Algorithm))
            {
                GenerateByProject(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentSolutionPath), project.Path));
            }
        }

        private static string GenerateByPlan(string currentPlan)
        {
            var outputPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(currentPlan),
                System.IO.Path.GetFileNameWithoutExtension(currentPlan) + ".gc");
            Debug.WriteLine(currentPlan);
            var tempGraph = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(currentPlan), ConverterFormats.JSON);
            Debug.WriteLine(tempGraph);
            var newVertices = new List<CGBlock>();
            foreach (var vertex in tempGraph.Vertices)
            {
                var _v = new CGBlock()
                {
                    Alias = "123",
                    Content = vertex.Content,
                    Id = vertex.Id,
                    Level = vertex.Level,
                    Type = vertex.Type
                };
                newVertices.Add(new CGBlock()
                {
                    Alias = "123",
                    Content = vertex.Content,
                    Id = vertex.Id,
                    Level = vertex.Level,
                    Type = vertex.Type
                });
            }
            var _tempGraph = new CGGraph(newVertices, tempGraph.Edges);
            var xmlDocument =
                Converter.GraphToData(
                    _tempGraph,
                    ConverterFormats.XML);
            using (var sr = new StringReader(xmlDocument))
            {
                using (var xr = XmlReader.Create(sr))
                {
                    using (var sw = new StringWriter())
                    {
                        var xslt = new XslCompiledTransform();
                        xslt.Load(XmlReader.Create(new StringReader(System.IO.File.ReadAllText(System.IO.Path.Combine(AssemblyFolder, @"CodeGeneration\XSLT\CSharp.xslt")))));
                        xslt.Transform(xr, null, sw);
                        System.IO.File.WriteAllText(outputPath, sw.ToString());
                    }
                }
            }
            return outputPath;
        }
    }
}
