﻿using System.Collections.Generic;
using Core.Atoms;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ImplementationPlanTest
    {
        [TestMethod]
        public void OneLogicalQTermWithoutOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            Assert.AreEqual((ulong)2, plan.CountCPU);
            Assert.AreEqual((ulong)4, plan.CountTacts);
        }

        [TestMethod]
        public void OneLogicalQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            plan.OptimizePlan(1);
            Assert.AreEqual((ulong)1, plan.CountCPU);
            Assert.AreEqual((ulong)6, plan.CountTacts);
        }

        [TestMethod]
        public void OneDefenitiveQTermWithoutOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            Assert.AreEqual((ulong)2, plan.CountCPU);
            Assert.AreEqual((ulong)4, plan.CountTacts);
        }

        [TestMethod]
        public void OneDefenitiveQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            plan.OptimizePlan(1);
            Assert.AreEqual((ulong)1, plan.CountCPU);
            Assert.AreEqual((ulong)6, plan.CountTacts);
        }

        [TestMethod]
        public void OneFullQTermWithoutOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))", Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.FindPlan();
            Assert.AreEqual((ulong)4, plan.CountCPU);
            Assert.AreEqual((ulong)4, plan.CountTacts);
        }

        [TestMethod]
        public void OneFullQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))", Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.OptimizePlan(2);
            Assert.AreEqual((ulong)2, plan.CountCPU);
            Assert.AreEqual((ulong)6, plan.CountTacts);
        }

        [TestMethod]
        public void RandomQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))", Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"}
            };
            plan.OptimizePlan(1);
            Assert.AreEqual((ulong)1, plan.CountCPU);
            Assert.AreEqual((ulong)12, plan.CountTacts);
        }

    }
}
