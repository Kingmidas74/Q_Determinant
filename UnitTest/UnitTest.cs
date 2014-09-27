using Converters;
using Core;
using FlowChart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private const string Jsonpathfolder = @"C:\tempforQ\";

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
    }
}
