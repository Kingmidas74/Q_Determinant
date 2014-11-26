using Core.Atoms;
using Core.Enums;

namespace Core
{
    public interface IPlan:IModule
    {
        ulong CountCPU { get; }
        ulong CountTacts { get; }
        void OptimizePlan(ulong countCPU);
        Graph GetPlan();

    }
}