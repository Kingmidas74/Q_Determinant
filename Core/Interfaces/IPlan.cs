using System.Collections.Generic;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Core.Interfaces
{
    public interface IPlan:IModule
    {
        ulong CountCPU { get; }
        ulong CountTacts { get; }
        void OptimizePlan(ulong countCPU);
        Graph GetPlan();

        IEnumerable<QTerm> QTerms { set; }
        List<Function> Functions { set; }

        void FindPlan();

    }
}