using FlowChart;
using FlowChart.AllConverters;
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
            var flowChart = new Chart();
            flowChart.ChartFromFile(ConverterTypes.JSON, Jsonpathfolder + @"test1.json");
            Assert.AreEqual(flowChart.GetCountLinks(), flowChart.GetCountBlocks());
            flowChart.SaveToFile(ConverterTypes.JSON, Jsonpathfolder + @"output.json");
           /* flowChart.SetJsonFile(Jsonpathfolder+@"test1.json");
            Assert.AreEqual(9, flowChart.GetBlocksCount());
            Assert.AreEqual("i++",flowChart.GetContentBlock(7));
            flowChart.ChangeContentBlock(7,"i--");
            Assert.AreEqual("i--", flowChart.GetContentBlock(7));
            flowChart.RemoveBlock(8);
            flowChart.AddBlock(BlockTypes.process, 0, "new block");
            flowChart.SaveToFile(Jsonpathfolder+@"resultTest.json");*/

        }
    }
}
