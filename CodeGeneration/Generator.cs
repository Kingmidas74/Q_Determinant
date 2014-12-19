using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using CodeGeneration.InternalClasses;
using Core.Atoms;
using Core.Converters;
using Core.Serializers;
using Core.Serializers.SerializationModels;
using File = Core.Serializers.SerializationModels.ProjectModels.File;

namespace CodeGeneration
{
    internal class Generator
    {
        private static readonly string AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static void GenerateByProject(string currentProjectPath)
        {
            Core.Serializers.SerializationModels.ProjectModels.Project project;
            SerializersFactory.GetSerializer().DeserializeProject(currentProjectPath,out project);
            var pathToFile = new List<string>();
            foreach (File x in project.Files)
            {
                if (System.IO.Path.GetExtension(x.Path).Equals(".ip")) pathToFile.Add(GenerateByPlan(x.Path));
            }
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
            var tempGraph = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(currentPlan), ConverterFormats.JSON);
            var functionAliases = new Dictionary<string, string>
            {
                {"||","OR_ALIAS"},{"&&","AND_ALIAS"},{"!","NEGATIVE_ALIAS"},
                {">","MORE_ALIAS"},{"<","LESS_ALIAS"},{">=","GTE_ALIAS"},{"<=","LTE_ALIAS"},
                {"==","EQUAL_ALIAS"},{"!=","NOTEQUAL_ALIAS"},
                {"+","ADD_ALIAS"},{"-","SUB_ALIAS"},
                {"*","MUL_ALIAS"},{"/","DIV_ALIAS"},
            };
            var xmlDocument =
                Converter.GraphToData(
                    new CGGraph(tempGraph.Vertices.Select(vertex => new CGBlock()
                    {
                        Alias = functionAliases.ContainsKey(vertex.Content) ? functionAliases[vertex.Content] : vertex.Content, Content = vertex.Content, Id = vertex.Id, Level = vertex.Level, Type = vertex.Type
                    }).ToList(), tempGraph.Edges),
                    ConverterFormats.XML);
            using (var sr = new StringReader(xmlDocument))
            {
                using (var xr = XmlReader.Create(sr))
                {
                    using (var sw = new StringWriter())
                    {
                        var xslt = new XslCompiledTransform();
                        xslt.Load(XmlReader.Create(new StringReader(System.IO.File.ReadAllText(System.IO.Path.Combine(AssemblyFolder, @"CodeGeneration\MetaLanguage.xslt")))));
                        xslt.Transform(xr, null, sw);
                        System.IO.File.WriteAllText(outputPath, sw.ToString());
                    }
                }
            }
            return outputPath;
        }

        internal static string ConvertWithTemplate(string pathToXsl, Dictionary<string, string> variableTypes, File implementationPlan)
        {
            var result = new StringBuilder("");
            var contentFile = System.IO.File.ReadAllText(implementationPlan.Path);
            var tempGraph = Converter.DataToGraph<Graph>(contentFile, ConverterFormats.JSON);
            var functionAliases = new Dictionary<string, string>
            {
                {"||","OR_ALIAS"},{"&&","AND_ALIAS"},{"!","NEGATIVE_ALIAS"},
                {">","MORE_ALIAS"},{"<","LESS_ALIAS"},{">=","GTE_ALIAS"},{"<=","LTE_ALIAS"},
                {"==","EQUAL_ALIAS"},{"!=","NOTEQUAL_ALIAS"},
                {"+","ADD_ALIAS"},{"-","SUB_ALIAS"},
                {"*","MUL_ALIAS"},{"/","DIV_ALIAS"},
            };
            var xmlDocument =
                Converter.GraphToData(
                    new CGGraph(tempGraph.Vertices.Select(vertex => new CGBlock()
                    {
                        Alias = functionAliases.ContainsKey(vertex.Content) ? functionAliases[vertex.Content] : vertex.Content,
                        Content = vertex.Content,
                        Id = vertex.Id,
                        Level = vertex.Level,
                        Type = vertex.Type
                    }).ToList(), tempGraph.Edges),
                    ConverterFormats.XML);
            using (var sr = new StringReader(xmlDocument))
            {
                using (var xr = XmlReader.Create(sr))
                {
                    using (var sw = new StringWriter())
                    {
                        var xslt = new XslCompiledTransform();
                        xslt.Load(XmlReader.Create(new StringReader(System.IO.File.ReadAllText(System.IO.Path.Combine(AssemblyFolder, @"CodeGeneration\MetaLanguage.xslt")))));
                        xslt.Transform(xr, null, sw);
                        result.Append(sw);
                    }
                }
            }
            using (var sr = new StringReader(result.ToString()))
            {
                using (var xr = XmlReader.Create(sr))
                {
                    using (var sw = new StringWriter())
                    {
                        var xslt = new XslCompiledTransform();
                        xslt.Load(XmlReader.Create(new StringReader(System.IO.File.ReadAllText(pathToXsl))));
                        xslt.Transform(xr, null, sw);
                        result.Clear().Append(sw);
                    }
                }
            }
            return result.ToString();
        }
    }
}
