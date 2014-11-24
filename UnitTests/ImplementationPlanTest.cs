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
            IPlan plan = new Plan(new List<QTerm>{new QTerm(){Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))"}},new List<Function>(){new Function(){Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"}, new Function(){Parameters = 2,Priority = FunctionPriorities.Third,Signature = "+"}},AvoidDuplicationTypes.Term);
        }
    }
}
