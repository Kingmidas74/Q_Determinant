using System.Collections.Generic;
using Core;
using Core.Atoms;
using Core.Converters;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void TestConvertJSON()
        {
            IPlan plan = new Plan(new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } }, new List<Function> { new Function { Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*" }, new Function { Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+" } }, AvoidDuplicationTypes.Term);
            var serializeraph = plan.GetPlan();
            var data = Converter.GraphToData(serializeraph, ConverterFormats.JSON);
            var deserializeGraph = Converter.DataToGraph(data, ConverterFormats.JSON);
            Assert.AreEqual(serializeraph.Edges.Count, deserializeGraph.Edges.Count);
            Assert.AreEqual(serializeraph.Vertices.Count, deserializeGraph.Vertices.Count);
        }
    }
}
