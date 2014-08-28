﻿using FlowChart;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        private const string Json = @"
{   
    'blocks': [
        {
            'id': 2,
            'type': 'process',
            'content': 'i: =0',
            'previousBlock': [0],
            'nextBlock': 3
        },
        {
            'id': 3,
            'type': 'process',
            'content': 'k: =0',
            'previousBlock': [2],
            'nextBlock': 4
        },
         {
            'id': 4,
            'type': 'process',
            'content': 's: =0',
            'previousBlock': [3],
            'nextBlock': 5
        },
        {
            'id': 5,
            'type': 'condition',
            'content': 'i<4',
            'falsePath': [
                {
                    'id': 8,
                    'type': 'InputOutput',
                    'content': 'k',
                    'previousBlock': [5],
                    'nextBlock': 1
                }
            ],
            'truePath': [
                {
                    'id': 6,
                    'type': 'process',
                    'content': 'k++',
                    'previousBlock': [5],
                    'nextBlock': 7
                },
                {
                    'id': 7,
                    'type': 'process',
                    'content': 'i++',
                    'previousBlock': [6],
                    'nextBlock': 5
                }
            ],
            'previousBlock': [4,7]
        }
    ]
}
";

        [TestMethod]
        public void TestFlowChart()
        {
            var flowChart = new Chart();
            flowChart.SetJson(Json);
            Assert.AreEqual(7, flowChart.GetTotalBlocksCount());
            Assert.AreEqual("i++",flowChart.GetContentBlock(7));
            flowChart.ChangeContentBlock(7,"i--");
            Assert.AreEqual("i--", flowChart.GetContentBlock(7));
            Assert.AreNotEqual("[2]",flowChart.GetPreviousBlock(3));
            Assert.AreEqual("4", flowChart.GetNextBlock(3));
            flowChart.RemoveBlock(3);
        }
    }
}
