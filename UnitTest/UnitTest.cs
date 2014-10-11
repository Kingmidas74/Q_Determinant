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
            qDet.QDeterminant.Add(new QTerm() { Definitive = "sin(x1+5*(x1+x2))", Logical = "x2>=x1"/*Definitive = "(8+2*5)/(1+3*2-4)", Logical = "dX>=(5*a+2*(b-1))"*/ });
          //  qDet.QDeterminant.Add(new QTerm() { Definitive = "x1*y1+x2*y2+x3*y3", Logical = "" });
            var converter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
            converter.ParseDocument(Jsonpathfolder+@"test3.json");
            var plan = new Plan(converter.GetBlocks(), qDet);
            Assert.AreEqual((ulong)2, plan.CountProcessors);
            Assert.AreEqual((ulong)4, plan.CountTacts);
            //plan.OptimizeForMaxEffective(3);
            /*plan = new Plan(converter.GetBlocks(), qDet,3);
            Assert.AreEqual((ulong)3, plan.CountProcessors);
            Assert.AreEqual((ulong)5, plan.CountTacts);
          /*  plan.OptimizeForMaxEffective(2);
            Assert.AreEqual((ulong)2, plan.CountProcessors);
            Assert.AreEqual((ulong)6, plan.CountTacts);
            plan.OptimizeForMaxEffective(1);
            Assert.AreEqual((ulong)1, plan.CountProcessors);
            Assert.AreEqual((ulong)11, plan.CountTacts);*/
            var saver = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            saver.SetBlocks(plan.GetVertexGraph());
            saver.SetLinks(plan.GetEdgesGraph());
            saver.SaveToFile(Jsonpathfolder + @"output_plan.json");
            
        }
    }
}
