using Core.Atoms;

namespace Core
{
    public interface IFlowChart:IModule
    {
        Graph GetFlowChart();
    }
}