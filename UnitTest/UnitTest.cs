using FlowChart;
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
            flowChart.SetJsonFile(Jsonpathfolder+@"test1.json");
            Assert.AreEqual(9, flowChart.GetBlocksCount());
            Assert.AreEqual("i++",flowChart.GetContentBlock(7));
            flowChart.ChangeContentBlock(7,"i--");
            Assert.AreEqual("i--", flowChart.GetContentBlock(7));
            flowChart.RemoveBlock(8);
            flowChart.AddBlock(BlockTypes.process, 0, "new block");
            flowChart.SaveToFile(Jsonpathfolder+@"resultTest.json");

        }
    }
}
