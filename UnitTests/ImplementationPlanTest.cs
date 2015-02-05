using System.Collections.Generic;
using System.Diagnostics;
using Core.Adapter;
using Core.Atoms;
using Core.Converters;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan;
using ImplementationPlan.InternalClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QDeterminant;

namespace UnitTests
{
    [TestClass]
    public class ImplementationPlanTest
    {

        [TestMethod]
        public void AdapterTest()
        {
            var Q = new Determinant();
            var P = new Plan();
            var _adapter = new Adapter<IDeterminant, IPlan>(Q, P);
            _adapter.FunctionsList = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "="},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "!="}
            };
            _adapter.FlowChart=new Graph();
            _adapter.CalculateDeterminant();
            _adapter.FindPlan();
            var a = _adapter.GetPlan();
            Assert.AreEqual("a",a.GetMaxLevel());



        }
        [TestMethod]
        public void DefineFirstLexem()
        {

            var LA = new LexemAnalyze(new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "="},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "!="}
            });
            var GB = new GraphBuilder(new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "="},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "!="}
            });

            var _qDeterminantModern = new List<QTerm>
            {
                /*new QTerm
                {
                    Logical="",
                    Definitive = "-(-(/(d,a),*(/(c,a),/(-(*(-(*(m,a),*(d,i)),-(*(f,a),*(b,e))),*(-(*(h,a),*(d,e)),-(*(k,a),*(b,i)))),-(*(-(*(l,a),*(c,e)),-(*(f,a),*(b,e))),*(-(*(j,a),*(c,i)),-(*(k,a),*(b,i))))))),*(/(b,a),-(/(-(*(h,a),*(d,e)),-(*(f,a),*(b,e))),*(/(-(*(j,a),*(c,e)),-(*(f,a),*(b,e))),/(-(*(-(*(m,a),*(d,i)),-(*(f,a),*(b,e))),*(-(*(h,a),*(d,e)),-(*(k,a),*(b,i)))),-(*(-(*(l,a),*(c,e)),-(*(f,a),*(b,e))),*(-(*(j,a),*(c,i)),-(*(k,a),*(b,i)))))))))"
                },
                new QTerm
                {
                    Logical = "",
                    Definitive = "-(/(-(*(h,a),*(d,e)),-(*(f,a),*(b,e))),*(/(-(*(j,a),*(c,e)),-(*(f,a),*(b,e))),/(-(*(-(*(m,a),*(d,i)),-(*(f,a),*(b,e))),*(-(*(h,a),*(d,e)),-(*(k,a),*(b,i)))),-(*(-(*(l,a),*(c,e)),-(*(f,a),*(b,e))),*(-(*(j,a),*(c,i)),-(*(k,a),*(b,i)))))))"
                },*/
                new QTerm
                {
                    Logical="!=(a,0)",
                    Definitive = "-(-(/(d,a),*(/(c,a),/(-(*(-(*(m,a),*(d,i)),-(*(f,a),*(b,e))),*(-(*(h,a),*(d,e)),-(*(k,a),*(b,i)))),-(*(-(*(l,a),*(c,e)),-(*(f,a),*(b,e))),*(-(*(j,a),*(c,i)),-(*(k,a),*(b,i))))))),*(/(b,a),-(/(-(*(h,a),*(d,e)),-(*(f,a),*(b,e))),*(/(-(*(j,a),*(c,e)),-(*(f,a),*(b,e))),/(-(*(-(*(m,a),*(d,i)),-(*(f,a),*(b,e))),*(-(*(h,a),*(d,e)),-(*(k,a),*(b,i)))),-(*(-(*(l,a),*(c,e)),-(*(f,a),*(b,e))),*(-(*(j,a),*(c,i)),-(*(k,a),*(b,i)))))))))"
                }/*,
                new QTerm
                {
                    Definitive = "+(/(+(+(5,N),8),-(*(N,5),7)),*(N,5))",
                    Logical = ">=(+(+(9,a),+(5,5)),*(8,+(+(5,5),+(7,1))))",
                    Index = 1
                }*/
            };
            ulong startId = 0;
            var _implementationPlan = new List<Graph>();
            foreach (var qTerm in _qDeterminantModern)
            {
                if (!qTerm.Logical.Equals(string.Empty))
                {
                    var graph = GB.BuildGraph(LA.AnalyzeTerm(qTerm.Logical), startId);
                    startId = graph.GetMaxId() + 1;
                    _implementationPlan.Add(graph);
                }
                
                if (!qTerm.Definitive.Equals(string.Empty))
                {
                    var graph = GB.BuildGraph(LA.AnalyzeTerm(qTerm.Definitive), startId);
                    startId = graph.GetMaxId() + 1;
                    _implementationPlan.Add(graph);
                }
            }
            
            Debug.WriteLine(Converter.GraphToData(_implementationPlan, ConverterFormats.JSON));
            
            Assert.AreEqual(">=", _implementationPlan[0].GetMaxId());
            
        }

        [TestMethod]
        public void OneLogicalQTermWithoutOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
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
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
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
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
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
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
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
            plan.QTerms = new List<QTerm> { new QTerm
                {
                    Definitive = "+(/(+(5,+(3,8)),-(*(3,5),7)),*(3,5))",
                    Logical = ">=(+(+(5,5),+(5,5)),*(8,+(+(5,5),+(7,1))))"
                }
            };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
            };
            plan.FindPlan();
            Assert.AreEqual((ulong)4, plan.CountCPU);
            Assert.AreEqual((ulong)4, plan.CountTacts);
        }

        [TestMethod]
        public void OneFullQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm
                {
                    Definitive = "+(/(+(5,+(3,8)),-(*(3,5),7)),*(3,5))",
                    Logical = ">=(+(+(5,5),+(5,5)),*(8,+(+(5,5),+(7,1))))"
                }
            };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
            };
            plan.FindPlan();
            plan.OptimizePlan(1);
            Assert.AreEqual((ulong)1, plan.CountCPU);
            Assert.AreEqual((ulong)12, plan.CountTacts);
        }

        [TestMethod]
        public void RandomQTermWithOptimization()
        {
            IPlan plan = new Plan();
            plan.QTerms = new List<QTerm> { new QTerm { Logical = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))", Definitive = "+(*(8,+(+(5,5),+(7,1))),+(+(5,5),+(5,5)))" } };
            plan.Functions = new List<Function>
            {
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "*"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "+"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = "-"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Fourth, Signature = "/"},
                new Function {Parameters = 2, Priority = FunctionPriorities.Third, Signature = ">="}
            };
            plan.FindPlan();
            plan.OptimizePlan(1);
            Assert.AreEqual((ulong)1, plan.CountCPU);
            Assert.AreEqual((ulong)12, plan.CountTacts);
        }

    }
}
