using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using CodeGeneration.Enums;
using CodeGeneration.InternalClasses;
using Core.Atoms;
using Core.Converters;
using File = Core.Serializers.SerializationModels.ProjectModels.File;

namespace CodeGeneration
{
    internal class Generator
    {
        private static readonly string AssemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        

        internal static string ConvertWithTemplate(string pathToXsl, ObservableCollection<Variable> variableTypes, File implementationPlan)
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
            var list = tempGraph.Vertices.Select(vertex => new CGBlock
            {
                Alias = functionAliases.ContainsKey(vertex.Content) ? functionAliases[vertex.Content] : vertex.Content, Content = vertex.Content, Id = vertex.Id, Level = vertex.Level, CGType = variableTypes.FirstOrDefault(x => x.Title.Equals(vertex.Content)) != null ? variableTypes.FirstOrDefault(x => x.Title.Equals(vertex.Content)).Type : VariableTypes.String
            }).ToList();
            var xmlDocument =
                Converter.GraphToData(
                    new CGGraph(list, tempGraph.Edges),
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
