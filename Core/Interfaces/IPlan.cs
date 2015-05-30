using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Atoms;

namespace Core.Interfaces
{
    public interface IPlan:IStatus
    {
        Graph ImplementationPlan { get; }
        Graph GetImplementationPlan(List<QTerm> qDeterminant);
        Graph GetOptimizePlan(ulong countCPU);
        ulong GetMaxLevel(List<QTerm> qDeterminant = null);
        ulong GetCPUCount(List<QTerm> qDeterminant = null);
    }
}
