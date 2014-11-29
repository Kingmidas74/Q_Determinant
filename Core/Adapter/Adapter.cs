using System.Collections.Generic;
using Core.Atoms;
using Core.Interfaces;

namespace Core.Adapter
{
    public class Adapter<D,P> where D:IDeterminant where P:IPlan
    {

        private D _qDeterminantModule;
        private P _implementationPlanModule;

        private List<QTerm> _qDerterminant;
        private Graph _implementationPlan;
        private Graph _flowChart;

        public Adapter(D qDeterminant, P implementationPlan)
        {
            _qDeterminantModule = qDeterminant;
            _implementationPlanModule = implementationPlan;
            _qDerterminant=new List<QTerm>();
            _implementationPlan = new Graph();
        }

        public void SetFlowChart(Graph flowChart)
        {
            _flowChart = flowChart;
        }




    }
}