using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using ImplementationPlan.InternalClasses;

namespace ImplementationPlan
{
    public class Plan:IPlan
    {
        public ulong CountCPU { get; private set; }

        public ulong CountTacts { get; private set; }

        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private List<Graph> _implementationPlan;
        

        private IEnumerable<QTerm> _qTerms;

        public IEnumerable<QTerm> QTerms
        {
            set { _qTerms = value; }
            get { return _qTerms; }
        }
        private List<Function> _functions;

        public List<Function> Functions
        {
            set { _functions = value; }
            get { return _functions; }
        } 

        public Plan()
        {
            _implementationPlan = new List<Graph>();
            Status=StatusTypes.Success;
        }

        public void FindPlan()
        {
            var LexemAnalyze = new LexemAnalyze(Functions);
            var GraphBuilder = new GraphBuilder(Functions);
            ulong startId = 1;
            foreach (var qTerm in QTerms)
            {
                if (!qTerm.Logical.Equals(string.Empty))
                {
                    var graph = GraphBuilder.BuildGraph(LexemAnalyze.AnalyzeTerm(qTerm.Logical), startId);
                    startId = graph.GetMaxId()+1;
                    _implementationPlan.Add(graph);
                }
                if (!qTerm.Definitive.Equals(string.Empty))
                {
                    var graph = GraphBuilder.BuildGraph(LexemAnalyze.AnalyzeTerm(qTerm.Definitive), startId);
                    startId = graph.GetMaxId() + 1;
                    _implementationPlan.Add(graph);
                }
            }
            CountTacts = GetMaxLevel();
            CountCPU = GetMaxOperationsInLevel();
        }

        public void OptimizePlan(ulong countCPU)
        {
            /*var globalGraph = new Graph();
            foreach (var graph in _implementationPlan)
            {
                globalGraph.Vertices.AddRange(graph.Vertices);
                globalGraph.Edges.AddRange(graph.Edges);
            }
            //globalGraph = Optimization.OptimizateGraph(globalGraph, countCPU);
            _implementationPlan.Clear();
            _implementationPlan.Add(globalGraph);
            CountTacts = globalGraph.GetMaxLevel();
            CountCPU = globalGraph.GetMaxOperationsInLevel();*/
        }

        public Graph GetPlan()
        {
            var globalGraph = new Graph();
            foreach (var graph in _implementationPlan)
            {
                globalGraph.Vertices.AddRange(graph.Vertices);
                globalGraph.Edges.AddRange(graph.Edges);
            }
            return globalGraph;
        }

        private ulong GetMaxLevel(Graph graph= null)
        {
            ulong result = 0;
            if (graph == null)
            {
                foreach (var partialGrapth in _implementationPlan)
                {
                    var localMaxLevel = partialGrapth.GetMaxLevel();
                    if (localMaxLevel > result)
                    {
                        result = localMaxLevel;
                    }
                }
                result = _implementationPlan.Max(x => x.GetMaxLevel());
            }
            else
            {
                result = graph.GetMaxLevel();
            }
            return result;
        }

        private ulong GetMaxOperationsInLevel()
        {
            return _implementationPlan.Aggregate<Graph, ulong>(0, (current, graph) => current + graph.GetMaxOperationsInLevel());
        }

    }
}
