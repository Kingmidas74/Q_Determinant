using System.Collections.Generic;
using Core;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan
{
    public class Plan:IPlan
    {
        public ulong CountCPU { get; private set; }

        public ulong CountTacts { get; private set; }

        private Graph _implementationPlan;


        public Plan(IEnumerable<QTerm> qTerms, List<Function> functions, AvoidDuplicationTypes avoidDuplicationType=AvoidDuplicationTypes.None)
        {
            _implementationPlan = new Graph();
            ReversePolishNotation.Functions = functions;
            ReversePolishNotation.RefreshId();
            
            foreach (var qTerm in qTerms)
            {
                var qTermGrapth = new Graph();
                var vertices = ReversePolishNotation.Translate(qTerm.Logical);
                var edges = Optimization.SetLinks(ref vertices);
                var graph = Optimization.SetLevels(new Graph(vertices, edges));
                if (avoidDuplicationType == AvoidDuplicationTypes.Term)
                {
                    graph = Optimization.RemoveDuplicateParameters(graph);
                    graph = Optimization.RemoveDuplicateFunctions(graph);
                    ReversePolishNotation.RefreshId();
                }
                qTermGrapth.Vertices.AddRange(graph.Vertices);
                qTermGrapth.Edges.AddRange(graph.Edges);

                vertices = ReversePolishNotation.Translate(qTerm.Definitive);
                edges = Optimization.SetLinks(ref vertices);
                graph = Optimization.SetLevels(new Graph(vertices, edges));
                if (avoidDuplicationType == AvoidDuplicationTypes.Term)
                {
                    graph = Optimization.RemoveDuplicateParameters(graph);
                    graph = Optimization.RemoveDuplicateFunctions(graph);
                    ReversePolishNotation.RefreshId();
                }
                qTermGrapth.Vertices.AddRange(graph.Vertices);
                qTermGrapth.Edges.AddRange(graph.Edges);
                if (avoidDuplicationType == AvoidDuplicationTypes.QTerm)
                {
                    qTermGrapth = Optimization.RemoveDuplicateParameters(qTermGrapth);
                    qTermGrapth = Optimization.RemoveDuplicateFunctions(qTermGrapth);
                    ReversePolishNotation.RefreshId();
                }
                _implementationPlan.Vertices.AddRange(qTermGrapth.Vertices);
                _implementationPlan.Edges.AddRange(qTermGrapth.Edges);
            }
            if (avoidDuplicationType == AvoidDuplicationTypes.QDeterminant)
            {
                _implementationPlan = Optimization.RemoveDuplicateParameters(_implementationPlan);
                _implementationPlan = Optimization.RemoveDuplicateFunctions(_implementationPlan);
                ReversePolishNotation.RefreshId();
            }
            CountTacts = _implementationPlan.GetMaxLevel();
            CountCPU = _implementationPlan.GetMaxOperationsInLevel();
        }

        public void OptimizePlan(ulong countCPU)
        {
            _implementationPlan = Optimization.OptimizateGraph(_implementationPlan, countCPU);
        }

        public Graph GetPlan()
        {
            return _implementationPlan;
        }
    }
}
