using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Atoms;
using Core.Enums;

namespace QDeterminant
{
    public class Determinant:IDeterminant
    {

        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private Dictionary<string,string> _variables;

        private List<QTerm> _qDeterminantStandart;
        private List<QTerm> _qDeterminantModern;
        private Graph _flowChart;


        public Determinant(Graph flowChart)
        {
            _qDeterminantStandart = new List<QTerm>();
            _flowChart = flowChart;
            _variables = new Dictionary<string, string>();
            CalculateDeterminant();
        }
        public List<string> GetVariables()
        {
            return _variables.Keys.ToList();
        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            _variables = variables;
        }

        public void CalculateDeterminant()
        {
            _qDeterminantStandart.Add(new QTerm {Definitive = "(5+3+8)/(3*5-7)+3*5", Logical = "5+5+5+5>=8*(5+5+7+1)"});
            TransformDeterminant();
        }

        public List<QTerm> GetOptimizationDeterminant()
        {
            return _qDeterminantModern;
        }

        public List<QTerm> GetDefaultDereminant()
        {
            return _qDeterminantStandart;
        }

        private void TransformDeterminant()
        {
            _qDeterminantModern = new List<QTerm>
            {
                new QTerm
                {
                    Definitive = "+(/(+(5,+(3,8)),-(*(3,5),7)),*(3,5))",
                    Logical = ">=(+(+(5,5),+(5,5)),*(8,+(+(5,5),+(7,1))))"
                }
            };
        }
    }
}
