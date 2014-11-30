using System.Collections.Generic;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;

namespace Core.Adapter
{
    public class Adapter<D,P> where D:IDeterminant where P:IPlan 
    {

        private D _qDeterminantModule;
        private P _implementationPlanModule;

        private readonly List<QTerm> _qDerterminant;
        private readonly Graph _implementationPlan;

        public Adapter(D qDeterminant, P implementationPlan)
        {
            QDeterminantModule = qDeterminant;
            ImplementationPlanModule = implementationPlan;
            _qDerterminant=new List<QTerm>();
            _implementationPlan = new Graph();
        }

        public D QDeterminantModule
        {
            set { _qDeterminantModule = value; }
        }

        public P ImplementationPlanModule
        {
            set { _implementationPlanModule = value; }
        }

        public List<QTerm> QDerterminant
        {
            get { return _qDerterminant; }
        }

        public Graph ImplementationPlan
        {
            get { return _implementationPlan; }
        }

        public Graph FlowChart { get; set; }


        public StatusTypes Status { get; set; }

        public string StatusMessage { get; set; }

        public List<string> GetVariables()
        {
            return _qDeterminantModule.GetVariables();
        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            _qDeterminantModule.SetVariables(variables);
        }

        public List<QTerm> GetOptimizationDeterminant()
        {
            return _qDeterminantModule.GetOptimizationDeterminant();
        }

        public List<QTerm> GetDefaultDereminant()
        {
            return _qDeterminantModule.GetDefaultDereminant();
        }

        public void CalculateDeterminant()
        {
            _qDeterminantModule.CalculateDeterminant();
        }

        public ulong CountCPU
        {
            get { return _implementationPlanModule.CountCPU; }
        }

        public ulong CountTacts
        {
            get { return _implementationPlanModule.CountTacts; }
        }

        public void OptimizePlan(ulong countCPU)
        {
            _implementationPlanModule.OptimizePlan(countCPU);
        }

        public Graph GetPlan()
        {
            return _implementationPlanModule.GetPlan();
        }
    }
}