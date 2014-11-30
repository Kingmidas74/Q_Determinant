using System.Collections.Generic;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Core.Interfaces
{
    public interface IDeterminant:IModule
    {
        List<string> GetVariables();

        void SetVariables(Dictionary<string, string> variables);

        List<QTerm> GetOptimizationDeterminant();

        List<QTerm> GetDefaultDereminant();

        void CalculateDeterminant();

        List<Function> Functions { set; }
        Graph FlowChart { set; }
    }
}