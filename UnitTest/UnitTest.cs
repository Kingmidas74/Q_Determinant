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
            Assert.AreEqual(7, flowChart.GetBlocksCount());
            
            Assert.AreEqual("i++",flowChart.GetContentBlock(7));
            flowChart.ChangeContentBlock(7,"i--");
            Assert.AreEqual("i--", flowChart.GetContentBlock(7));
            flowChart.RemoveBlock(7);
            flowChart.SaveToFile(Jsonpathfolder+@"resultTest.json");
        }
    }
}
