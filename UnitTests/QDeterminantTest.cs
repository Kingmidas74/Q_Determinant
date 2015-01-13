using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            IDeterminant determinant = new Determinant();
            determinant.FlowChart = Converter.DataToGraph<Graph>(System.IO.File.ReadAllText(@"C:\test\test1.json"), ConverterFormats.JSON);
            determinant.CalculateDeterminant();
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[0].Definitive);
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[0].Logical);
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[1].Definitive);
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[1].Logical);
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[2].Definitive);
            Assert.AreEqual("", determinant.GetOptimizationDeterminant()[2].Logical);
        }
    }
}
