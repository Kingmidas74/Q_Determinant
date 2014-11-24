using System;
using System.Collections.Generic;
using Core;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ImplementationPlanTest
    {
        [TestMethod]
        public void TestAll()
        {
            IPlan plan = new Plan(new List<QTerm>{new QTerm(){Logical = "*(+(+(5,3),*(7,2,5)),4,+(1,+(+(1,2),2)))"}},new List<Function>(){new Function(){MinimumParameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"}, new Function(){MinimumParameters = 2,Priority = FunctionPriorities.Third,Signature = "+"}});
        }
    }
}
