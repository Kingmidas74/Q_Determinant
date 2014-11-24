using Core.Atoms;

namespace Core
{
    public interface IPlan
    {
        bool AvoidDuplication { get;  }
        ulong CountCPU { get; }
        ulong CountTacts { get; }
        void OptimizePlan(ulong countCPU);
        Graph GetPlan();
    }
}