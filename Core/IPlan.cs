using Core.Atoms;

namespace Core
{
    public interface IPlan
    {
        ulong CountCPU { get; }
        ulong CountTacts { get; }
        void OptimizePlan(ulong countCPU);
        Graph GetPlan();
    }
}