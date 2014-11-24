using System;
using System.Collections.Generic;
using Core;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan
{
    public class Plan:IPlan
    {
        public bool AvoidDuplication { get; set; }

        public ulong CountCPU { get; private set; }

        public ulong CountTacts { get; private set; }

        private Graph _implementationPlan;
        private List<Function> _functions;



        public Plan(IEnumerable<QTerm> qTerms, List<Function> functions)
        {
            _functions = functions;
            _implementationPlan = new Graph();
            ReversePolishNotation.Functions = _functions;
            foreach (var qTerm in qTerms)
            {
                ReversePolishNotation.Translate(qTerm.Logical);
            }
        }

        public void OptimizePlan(ulong countCPU)
        {
            throw new NotImplementedException();
        }

        public Graph GetPlan()
        {
            return _implementationPlan;
        }
    }
}
