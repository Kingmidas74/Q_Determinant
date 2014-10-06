using System.Collections.Generic;
using Converters;
using Core;
using FlowChart;
using ImplementationPlan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private const string Jsonpathfolder = @"D:\tempforQ\";

        [TestMethod]
        public void TestFlowChart()
        {
            var converter = Manufactory.CreateFlowChartConverter(ConverterTypes.JSON);
            converter.ParseDocument(Jsonpathfolder+@"test1.json");
            var flowChart = new Chart(converter.GetBlocks(),converter.GetLinks());
            Assert.AreEqual(9, flowChart.GetCountBlocks());
            flowChart.RemoveLink(2,3);
            flowChart.RemoveBlock(3);
            Assert.AreEqual(8, flowChart.GetCountBlocks());
            flowChart.AddBlock(BlockTypes.Process, "c=2");
            flowChart.ChangePropertiesLink(3,4,10,4);
            flowChart.AddLink(2,10,LinkTypes.Null);
            converter.SetBlocks(flowChart.GetBlocks());
            converter.SetLinks(flowChart.GetLinks());
            converter.SaveToFile(Jsonpathfolder+"output_new.json");
            Assert.AreEqual(9, flowChart.GetCountBlocks());

        }

        [TestMethod]
        public void TestImplementationPlan()
        {
            var qDet = new QDet()
            {
                QDeterminant = new List<QTerm>()
            };
            qDet.QDeterminant.Add(new QTerm() { Definitive = "(8+2*5)/(1+3*2-4)", Logical = "dX>=(5*a+2*(b-1))" });
            qDet.QDeterminant.Add(new QTerm() { Definitive = "x1*y1+x2*y2+x3*y3", Logical = "" });
            var converter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
            converter.ParseDocument(Jsonpathfolder+@"test3.json");
            var plan = new Plan(converter.GetBlocks(), qDet);
            var saver = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            saver.SetBlocks(plan.GetVertexGraph());
            saver.SetLinks(plan.GetEdgesGraph());
            saver.SaveToFile(Jsonpathfolder + @"output_plan.json");
            Assert.AreEqual((ulong)7,plan.CountProcessors);
            Assert.AreEqual((ulong)4,plan.CountTacts);
        }
    }
}
