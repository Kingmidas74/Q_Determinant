using System.Collections.Generic;
using Core.Atoms;

namespace Core.Interfaces
{
    public interface IDeterminant:IModule
    {
        List<string> GetVariables();

        void SetVariables(Dictionary<string, string> variables);

        List<QTerm> GetOptimizationDeterminant();

        List<QTerm> GetDefaultDereminant();

        void CalculateDeterminant();
    }
}