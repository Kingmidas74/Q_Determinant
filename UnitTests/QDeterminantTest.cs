using System;
using Core.Atoms;
using Core.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Converters;
using QDeterminant;

namespace UnitTests
{
    [TestClass]
    public class QDeterminantTest
    {
        [TestMethod]
        public void QdeterminantTest()
        {
            AppDomain.CurrentDomain.AppendPrivatePath(@"vendors");
            AppDomain.CurrentDomain.AppendPrivatePath(@"core");
            IDeterminant determinant = new QDet();
            determinant.FlowChart = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(@"C:\test\test1.json"), ConverterFormats.JSON);
            determinant.CalculateDeterminant();
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[0].Logical);

        }
    }
}
