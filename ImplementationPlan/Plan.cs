using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan
{
    public class Plan:IPlan
    {
        public ulong CountCPU { get; private set; }

        public ulong CountTacts { get; private set; }

        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private List<Graph> _implementationPlan;
        private AvoidDuplicationTypes AvoidDuplicationType { get; set; }
        

        public Plan(IEnumerable<QTerm> qTerms, List<Function> functions, AvoidDuplicationTypes avoidDuplicationType=AvoidDuplicationTypes.None)
        {
            AvoidDuplicationType = avoidDuplicationType;
            _implementationPlan = new List<Graph>();
            ReversePolishNotation.Functions = functions;
            ReversePolishNotation.RefreshId();
            
            foreach (var qTerm in qTerms)
            {
                _implementationPlan.AddRange(new List<Graph> {ParseTerm(qTerm.Logical), ParseTerm(qTerm.Definitive)});
            }
            CountTacts = GetMaxLevel();
            CountCPU = GetMaxOperationsInLevel();
        }

        public void OptimizePlan(ulong countCPU)
        {
            var globalGraph = new Graph();
            foreach (var graph in _implementationPlan)
            {
                globalGraph.Vertices.AddRange(graph.Vertices);
                globalGraph.Edges.AddRange(graph.Edges);
            }
            globalGraph = Optimization.OptimizateGraph(globalGraph, countCPU);
            foreach (var vertex in globalGraph.Vertices)
            {
                Debug.WriteLine(vertex.Content, "VERTEX_CONTENT");
                Debug.WriteLine(vertex.Level, "LEVEL");
            }
            CountTacts = globalGraph.GetMaxLevel();
            CountCPU = globalGraph.GetMaxOperationsInLevel();
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
                    Debug.WriteLine(localMaxLevel,"TACTS");
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
            ulong result = 0;
            foreach (var graph in _implementationPlan)
            {
                Debug.WriteLine(graph.GetMaxOperationsInLevel(),"CPU: ");
                /*if (graph.GetMaxOperationsInLevel() > result)
                {
                    result = graph.GetMaxOperationsInLevel();
                }*/
                result += graph.GetMaxOperationsInLevel();
            }
            return result;
        }

        private Graph ParseTerm(string term)
        {
            if(String.IsNullOrEmpty(term)) return new Graph();
            var vertices = ReversePolishNotation.Translate(term);
            var edges = Optimization.SetLinks(ref vertices);
            var graph = Optimization.SetLevels(new Graph(vertices, edges));
            graph = Optimization.RemoveDuplicateParameters(graph);
            graph = Optimization.RemoveDuplicateFunctions(graph);
            return graph;
        }

        private void MessageHandler(string message, StatusTypes type=StatusTypes.Success)
        {
            Status = type;
            StatusMessage = message;
        }
        

        
    }
}
