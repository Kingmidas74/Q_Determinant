using Core.Atoms;

namespace Core.Interfaces
{
    public interface IFlowChart:IModule
    {
        Graph GetFlowChart();
    }
}