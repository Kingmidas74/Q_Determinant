using System.Collections.Generic;
using System.Diagnostics;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace Core.Adapter
{
    public class Adapter<D,P> where D:IDeterminant where P:IPlan
    {
        private D QDeterminantModule { get; set; }

        private P ImplementationPlanModule { get; set; }

        public List<Function> FunctionsList
        {
            set
            {
                ImplementationPlanModule.Functions = value;
                QDeterminantModule.Functions = value;
            }
        }
        

        private readonly List<QTerm> _qDerterminant;
        public List<QTerm> QDerterminant
        {
            get { return _qDerterminant; }
        }

        private readonly Graph _implementationPlan;
        public Graph ImplementationPlan
        {
            get { return _implementationPlan; }
        }


        #region Fields

        public StatusTypes Status { get; set; }

        public string StatusMessage { get; set; }

        public ulong CountCPU
        {
            get { return ImplementationPlanModule.CountCPU; }
        }

        public ulong CountTacts
        {
            get { return ImplementationPlanModule.CountTacts; }
        }

        public Graph FlowChart
        {
            set { QDeterminantModule.FlowChart = value; }
        }
        #endregion


        public Adapter(D qDeterminant, P implementationPlan)
        {
            QDeterminantModule = qDeterminant;
            ImplementationPlanModule = implementationPlan;
            _qDerterminant = new List<QTerm>();
            _implementationPlan = new Graph();
        }

        public List<string> GetVariables()
        {
            return QDeterminantModule.GetVariables();
        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            QDeterminantModule.SetVariables(variables);
        }

        public void CalculateDeterminant()
        {
            QDeterminantModule.CalculateDeterminant();
            Status = QDeterminantModule.Status;
        }

        public void FindPlan()
        {
            ImplementationPlanModule.QTerms = QDeterminantModule.GetOptimizationDeterminant();
            ImplementationPlanModule.FindPlan();
            Status = ImplementationPlanModule.Status;
        }

        public void OptimizePlan(ulong countCPU)
        {
            ImplementationPlanModule.OptimizePlan(countCPU);
            Status = ImplementationPlanModule.Status;
        }

        public Graph GetPlan()
        {
            return ImplementationPlanModule.GetPlan();
        }

       /* public List<QTerm> GetOptimizationDeterminant()
        {
            return _qDeterminantModule.GetOptimizationDeterminant();
        }

        public List<QTerm> GetDefaultDereminant()
        {
            return _qDeterminantModule.GetDefaultDereminant();
        }
        */
    }
}