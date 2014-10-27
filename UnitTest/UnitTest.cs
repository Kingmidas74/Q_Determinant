using System.Collections.Generic;
using ActionList;
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
        public void TestActionList()
        {
            var converter = Manufactory.CreateFlowChartConverter(ConverterTypes.JSON);
            converter.ParseDocument(@"C:\test\test1.json");
            var flowChart = new Chart(converter.GetBlocks(), converter.GetLinks());
            var converter2 = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
            converter2.ParseDocument(@"C:\test\op.json");
            var actionList = new AList(flowChart.GetBlocks(), flowChart.GetLinks(), converter2.GetBlocks());
            Assert.AreEqual("dx>=(5*a+2*(b-1))", actionList.getqdet().QDeterminant[0].Logical);
            Assert.AreEqual("8+2", actionList.getqdet().QDeterminant[0].Definitive);
            Assert.AreEqual("dx<=(5*a+2*(b-1))", actionList.getqdet().QDeterminant[0].Logical);
            Assert.AreEqual("3+a", actionList.getqdet().QDeterminant[0].Definitive);
            

        }

        [TestMethod]
        public void TestImplementationPlan()
        {
            var qDet = new QDet
            {
                QDeterminant = new List<QTerm>()
            };
            qDet.QDeterminant.Add(new QTerm { Definitive = "sin(x1+5*(x1+x2))", Logical = "x2>=x1" });
            var converter = Manufactory.CreateOperationConverter(ConverterTypes.JSON);
            converter.ParseDocument(Jsonpathfolder+@"test3.json");
            var plan = new Plan(converter.GetBlocks(), qDet);
            Assert.AreEqual((ulong)2, plan.CountProcessors);
            Assert.AreEqual((ulong)4, plan.CountTacts);
            var saver = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            saver.SetBlocks(plan.GetVertexGraph());
            saver.SetLinks(plan.GetEdgesGraph());
            saver.SaveToFile(Jsonpathfolder + @"output_plan.json"); 
            
        }
    }
}
