using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace QDeterminant
{
    public class Determinant:IDeterminant
    {
        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private Dictionary<string,string> _variables;

        private int numberofrepeat;
        private List<QTerm> _qDeterminantStandart;
        private List<QTerm> _qDeterminantModern = new List<QTerm>();
        private Graph _flowChart;
        private List<Function> _functions;


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
            
            var setqdeterminantActions = new Dictionary<FlowChartType, Action>()
            {
                {FlowChartType.Scalarproductofvectors, ScalarproductofvectorsAction},
                {FlowChartType.Thesumofthenumber, ThesumofthenumberAction}
            };
            Debug.WriteLine(Flowcharttypedetector());
            setqdeterminantActions[Flowcharttypedetector()]();
        }

        private void ThesumofthenumberAction()
        {
            var temp = new StringBuilder("");
            var prefixtemp = new StringBuilder("");
            var qTerm = new QTerm {Logical = temp.Append("<(1/").Append(numberofrepeat).Append(")").ToString()};
            temp.Clear();
            temp.Append("+(");
            for (var i = 2; i < numberofrepeat; i+=2)
            {
                temp.Append("(+(").Append(Math.Pow(-1, i - 1)).Append(",1/").Append((i - 1)).Append(")").Append(",(").Append(Math.Pow(-1, i)).Append(",(1/").Append(i).Append(")),");
            }
            temp.Append(")");
            qTerm.Definitive = temp.ToString();
            temp.Clear();
            Debug.WriteLine(qTerm.Definitive);
            _qDeterminantModern.Add(qTerm);
        }

        private void ScalarproductofvectorsAction()
        {
            var temp = new StringBuilder("");
            var qTerm = new QTerm();
            temp.Append("+(");
            for (int i = 0; i < numberofrepeat; i++)
            {
                temp.Append("*(x[").Append(i).Append("],y[").Append(i).Append("]),+("); 
            }
            temp.Length = temp.Length - 3;
            temp.Remove(temp.Length - 14, 2);
            for (int i = 0; i < numberofrepeat - 1; i++)
            {
                temp.Append(")");
            }
            qTerm.Definitive = temp.ToString();
            Debug.WriteLine(qTerm.Definitive);
            temp.Clear();
            _qDeterminantModern.Add(qTerm);
        }

        private FlowChartType Flowcharttypedetector()
        {
            var number = new StringBuilder("");
            foreach (var ex in _flowChart.Vertices.Where(y => y.Type == BlockTypes.Input || y.Type == BlockTypes.Process))
            {
                if (ex.Content[0] == 'n')
                {
                    for (int i = 2; i < ex.Content.Length; i++)
                    {
                        number.Append(ex.Content[i]);
                    }
                    numberofrepeat = Convert.ToInt32(number.ToString());
                    return FlowChartType.Scalarproductofvectors;
                }
                if (ex.Content[1] == 'm')
                {
                    for (int i = 2; i < ex.Content.Length; i++)
                    {
                        number.Append(ex.Content[i]);
                    }
                    numberofrepeat = Convert.ToInt32(number.ToString());
                    return FlowChartType.Thesumofthenumber;
                }
            }
            return FlowChartType.Error;
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
            
        }
    }
}
