using System.Collections.Generic;
using System.Linq;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using QDeterminant.InternalClasses;

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
        private List<Function> _functions;
        private List<ActionList> _actionLists; 

        public Graph FlowChart
        {
            set { _flowChart = value; }
        }

        public List<Function> Functions
        {
            set { _functions=value; }
        }

        public Determinant()
        {
            _qDeterminantStandart = new List<QTerm>();
            _actionLists = new List<ActionList>();
            _variables = new Dictionary<string, string>();
            Status = StatusTypes.Success;
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
            _variables.Add("N", "");
            var s = _variables["N"];
            for (int i = 0; i < 1000; i++)
            {
                int lol = 0;
                lol = lol + i;
            }
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
                    Definitive = "+(/(+(+(5.02,3),8),-(*(3,5),7)),*(3,5))",
                    Logical = ">=(+(+(9,a),+(5,5)),*(8,+(+(5,5),+(7,1))))"
                }
            };
        }
    }
}
