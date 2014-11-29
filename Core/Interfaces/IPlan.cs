using Core.Atoms;

namespace Core.Interfaces
{
    public interface IPlan:IModule
    {
        ulong CountCPU { get; }
        ulong CountTacts { get; }
        void OptimizePlan(ulong countCPU);
        Graph GetPlan();

    }
}