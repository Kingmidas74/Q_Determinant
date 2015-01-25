using System;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Adapter;
using Core.Atoms;
using Core.Converters;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QDeterminant;

namespace UnitTests
{
    [TestClass]
    public class CoreTest
    {
        [TestMethod]
        public void TestConvertJSON()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> {new QTerm {Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))"}};
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            var serializeraph = plan.GetPlan();
            var data = Converter.GraphToData(serializeraph, ConverterFormats.JSON);
            var deserializeGraph = Converter.DataToGraph<Graph>(data, ConverterFormats.JSON);
            Assert.AreEqual(serializeraph.Edges.Count, deserializeGraph.Edges.Count);
            Assert.AreEqual(serializeraph.Vertices.Count, deserializeGraph.Vertices.Count);
        }

        [TestMethod]
        public void TestConvertMessagePack()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
            AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
            var Q = new Determinant();
            var P = new Plan();
            var _adapter = new Adapter<IDeterminant, IPlan>(Q, P);
            _adapter.FunctionsList = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "="},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "!="},
                new Function {Parameters = 2, Priority = FunctionPriorities.First, Signature = "&&"}
            };
            _adapter.FlowChart = new Graph();
            _adapter.CalculateDeterminant();
            _adapter.FindPlan();
            var a = _adapter.GetPlan();
            Debug.WriteLine(a.GetMaxLevel(),"GMLB");
            var c = Converter.GraphToData(a, ConverterFormats.MessagePack);
            a = Converter.DataToGraph<Graph>(c, ConverterFormats.MessagePack);
            Debug.WriteLine(a.GetMaxLevel(), "GMLA");
        }
    }
}
