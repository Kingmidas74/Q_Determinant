using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;

namespace QDeterminant
{
    public class QDet : IDeterminant
    {

        #region public_variables

        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        public Graph FlowChart { get; set; }

        public List<Function> Functions { get; set; }

        #endregion

        #region private_variables

        private Dictionary<string, string> _variables;
        private Dictionary<string, double> _variableValue;
        private List<QTerm> _qDeterminant;
        private Dictionary<BlockTypes, Action<Block, QTerm>> _blockActions;

        #endregion

        #region private_methods

        private void FlowChartBypass(ulong To, QTerm qTerm)
        {
            var block = FlowChart.Vertices.FirstOrDefault(y => y.Id == To);
            _blockActions[block.Type](block, qTerm);
        }

        private void ProcessAction(Block obj, QTerm qTerm)
        {
            Debug.WriteLine(obj.Content);

            var parsedstring = new Parser(obj.Content);
            if (parsedstring.Lexems.Count == 3)
            {
                AddVarInProcess(parsedstring.Lexems[0], parsedstring.Lexems[2], null, null);
                CountInProcess(parsedstring.Lexems[0], parsedstring.Lexems[2], null, null);
            }
            else
            {
                AddVarInProcess(parsedstring.Lexems[0], parsedstring.Lexems[2], parsedstring.Lexems[4],
                    parsedstring.Lexems[3]);
                CountInProcess(parsedstring.Lexems[0], parsedstring.Lexems[2], parsedstring.Lexems[4], parsedstring.Lexems[3]);
            }
            FlowChartBypass(FlowChart.Edges.FirstOrDefault(y => y.From == obj.Id).To, qTerm);
        }

        private void CountInProcess(string var1, string var2, string var3, string action)
        {
            double flag;

            if (var3 == null)
            {
                if (_variableValue.ContainsKey(var1))
                {
                    if (_variableValue.ContainsKey(var2))
                    {
                        _variableValue[var1] = _variableValue[var2];
                    }
                    else
                    {
                        if (Double.TryParse(var2, out flag))
                        {
                            _variableValue[var1] = flag;
                        }
                    }
                }
                else
                {
                    if (_variableValue.ContainsKey(var2))
                    {
                        _variableValue.Add(var1, _variableValue[var2]);
                    }
                    else
                    {
                        if (Double.TryParse(var2, out flag))
                        {
                            _variableValue.Add(var1, flag); ;
                        }
                    }
                }
            }
            else
            {
                if (var1.Equals(var2))
                {
                    if (_variableValue.ContainsKey(var1))
                    {
                        if (_variableValue.ContainsKey(var3))
                        {
                            CountVarsProcess(var1, _variableValue[var1], _variableValue[var3], action);
                        }
                        else
                        {
                            if (Double.TryParse(var3, out flag))
                            {
                                CountVarsProcess(var1, _variableValue[var1], flag, action);
                            }
                        }
                    }
                    else
                    {
                        _variableValue.Add(var1, 0);

                        if (_variableValue.ContainsKey(var3))
                        {
                            CountVarsProcess(var1, _variableValue[var1], _variableValue[var3], action);
                        }
                        else
                        {
                            if (Double.TryParse(var3, out flag))
                            {
                                CountVarsProcess(var1, _variableValue[var1], flag, action);
                            }
                        }
                    }
                }
                else
                {
                    if (!_variableValue.ContainsKey(var1))
                    {
                        _variableValue.Add(var1, 0);
                    }

                    if (_variableValue.ContainsKey(var2))
                    {
                        if (_variableValue.ContainsKey(var3))
                        {
                            CountVarsProcess(var1, _variableValue[var2], _variableValue[var3], action);
                        }
                        else
                        {
                            if (Double.TryParse(var3, out flag))
                            {
                                CountVarsProcess(var1, _variableValue[var2], flag, action);
                            }
                        }
                    }
                    else
                    {
                        if (Double.TryParse(var2, out flag))
                        {
                            if (_variableValue.ContainsKey(var3))
                            {
                                CountVarsProcess(var1, flag, _variableValue[var3], action);
                            }
                            else
                            {
                                double flag2;

                                if (Double.TryParse(var3, out flag2))
                                {
                                    CountVarsProcess(var1, flag, flag2, action);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CountVarsProcess(string var, double var1, double var2, string action)
        {
            switch (action)
            {
                case "+":
                    _variableValue[var] = var1 + var2;
                    break;
                case "-":
                    _variableValue[var] = var1 - var2;
                    break;
                case "*":
                    _variableValue[var] = var1 * var2;
                    break;
                case "/":
                    _variableValue[var] = var1 / var2;
                    break;
                default:
                    break;
            }
        }
        private void AddVarInProcess(string var1, string var2, string var3, string action)
        {
            var flag = new StringBuilder("");

            if (var3 == null)
            {
                if (_variables.ContainsKey(var1))
                {
                    if (_variables.ContainsKey(var2))
                    {
                        _variables[var1] = _variables[var2];
                    }
                    else
                    {
                        _variables[var1] = var2;
                    }
                }
                else
                {
                    if (_variables.ContainsKey(var2))
                    {
                        _variables.Add(var1, _variables[var2]);
                    }
                    else
                    {
                        _variables.Add(var1, var2);
                    }
                }
            }
            else
            {
                if (!var1.Equals(var2))
                {
                    flag.Append(action).Append(Parser.OpenParentheses);

                    if (_variables.ContainsKey(var2))
                    {
                        flag.Append(_variables[var2]).Append(Parser.Separator);
                    }
                    else
                    {
                        flag.Append(var2).Append(Parser.Separator);
                    }

                    Debug.WriteLine(var3);

                    if (_variables.ContainsKey(var3))
                    {
                        flag.Append(_variables[var3]).Append(Parser.CloseParentheses);
                    }
                    else
                    {
                        flag.Append(var3).Append(Parser.CloseParentheses);
                    }

                    if (_variables.ContainsKey(var1))
                    {
                        _variables[var1] = flag.ToString();
                    }
                    else
                    {
                        _variables.Add(var1, flag.ToString());
                    }

                    flag.Clear();

                }
                else
                {
                    if (_variables.ContainsKey(var1))
                    {
                        flag.Append(action)
                            .Append(Parser.OpenParentheses)
                            .Append(_variables[var1])
                            .Append(Parser.Separator);
                        if (_variables.ContainsKey(var3))
                        {
                            flag.Append(_variables[var3]).Append(Parser.CloseParentheses);
                        }
                        else
                        {
                            flag.Append(var3).Append(Parser.CloseParentheses);
                        }

                        _variables[var1] = flag.ToString();
                    }
                    else
                    {
                        if (_variables.ContainsKey(var3))
                        {
                            flag.Append(action)
                                .Append(Parser.OpenParentheses)
                                .Append(var2)
                                .Append(Parser.Separator)
                                .Append(_variables[var3])
                                .Append(Parser.CloseParentheses);
                            _variables.Add(var1, flag.ToString());
                        }
                        else
                        {
                            flag.Append(action)
                                .Append(Parser.OpenParentheses)
                                .Append(var2)
                                .Append(Parser.Separator)
                                .Append(var3)
                                .Append(Parser.CloseParentheses);
                            _variables.Add(var1, flag.ToString());
                        }
                    }
                }
            }
        }

        private void ConditionalAction(Block obj, QTerm qTerm)
        {
            // var qTermFalse = new QTerm();
            var temp = new StringBuilder("");
            // qTermFalse = qTerm;
            var cycleMethod = new CycleHeadler();
            var falseLink = FlowChart.Edges.FirstOrDefault(y => y.From == obj.Id && y.Type == LinkTypes.FalseBrunch);
            var trueLink = FlowChart.Edges.FirstOrDefault(y => y.From == obj.Id && y.Type == LinkTypes.TrueBrunch);
            Debug.WriteLine(obj.Content);

            var parsedstring = new Parser(obj.Content);
            bool flag;
            flag = cond(parsedstring.Lexems[0], parsedstring.Lexems[2], parsedstring.Lexems[1]);

            if (cycleMethod.IsCycled(obj.Id, FlowChart, falseLink))
            {
                Debug.WriteLine("LOL1");
                if (!flag)
                {
                    FlowChartBypass(falseLink.To, qTerm);
                }
                else
                {
                    /*   temp.Append(qTerm.Logical)
                           .Append(Parser.OpenParentheses)
                           .Append(obj.Content)
                           .Append(Parser.CloseParentheses);
                       qTerm.Logical = temp.ToString();*/
                    FlowChartBypass(trueLink.To, qTerm);
                }
            }
            else
            {
                if (cycleMethod.IsCycled(obj.Id, FlowChart, trueLink))
                {
                    Debug.WriteLine("LOL1");

                    if (flag)
                    {
                        FlowChartBypass(trueLink.To, qTerm);
                    }
                    else
                    {
                        /*      temp.Append(qTerm.Logical)
                                  .Append(Parser.OpenParentheses)
                                  .Append(FalseCond(obj.Content))
                                  .Append(Parser.CloseParentheses);
                              qTerm.Logical = temp.ToString();*/
                        FlowChartBypass(falseLink.To, qTerm);
                    }
                }
                else
                {
                    Debug.WriteLine("OK!");
                    var qTermFalse = new QTerm();
                    qTermFalse.Logical = qTerm.Logical;
                    qTermFalse.Definitive = qTerm.Definitive;

                    bool tmp = false;

                    if (!String.IsNullOrEmpty(qTerm.Logical))
                    {
                        tmp = true;
                        temp.Append("&&").Append(Parser.OpenParentheses).Append(qTerm.Logical).Append(Parser.Separator);
                    }

                    temp.Append(parsedstring.Lexems[1])
                        .Append(Parser.OpenParentheses).Append(parsedstring.Lexems[0]).Append(Parser.Separator).Append(parsedstring.Lexems[2]).Append(Parser.CloseParentheses);
                    if (tmp)
                    {
                        temp.Append(Parser.CloseParentheses);
                    }

                    tmp = false;

                    qTerm.Logical = temp.ToString();
                    temp.Clear();

                    FlowChartBypass(trueLink.To, qTerm);

                    if (!String.IsNullOrEmpty(qTermFalse.Logical))
                    {
                        tmp = true;
                        temp.Append("&&").Append(Parser.OpenParentheses).Append(qTermFalse.Logical).Append(Parser.Separator);
                    }

                    temp.Append("!").Append(Parser.OpenParentheses)
                        .Append(parsedstring.Lexems[1])
                        .Append(Parser.OpenParentheses).Append(parsedstring.Lexems[0]).Append(Parser.Separator).Append(parsedstring.Lexems[2]).Append(Parser.CloseParentheses).Append(Parser.CloseParentheses);

                    if (tmp)
                    {
                        temp.Append(Parser.CloseParentheses);
                    }

                    qTermFalse.Logical = temp.ToString();
                    temp.Clear();

                    FlowChartBypass(falseLink.To, qTermFalse);
                }
            }
        }

        private string FalseCond(string s)
        {
            string str = s;
            if (str.Contains(">"))
            {
                str.Replace(">", "<");
            }
            if (str.Contains("<"))
            {
                str.Replace("<", ">");
            }
            if (str.Contains("!"))
            {
                str.Replace("=", "=");
            }
            if (str.Contains("=="))
            {
                str.Replace("==", "!=");
            }

            return str;
        }

        private bool cond(string var1, string var2, string action)
        {
            double v1 = _variableValue[var1];
            double v2 = _variableValue[var2];

            switch (action)
            {
                case "<":
                    if (v1 < v2)
                    {
                        return true;
                    }
                    break;
                case ">":
                    if (v1 > v2)
                    {
                        return true;
                    }
                    break;
                case "<=":
                    if (v1 <= v2)
                    {
                        return true;
                    }
                    break;
                case ">=":
                    if (v1 >= v2)
                    {
                        return true;
                    }
                    break;
                case "==":
                    if (v1 == v2)
                    {
                        return true;
                    }
                    break;
                case "!=":
                    if (v1 != v2)
                    {
                        return true;
                    }
                    break;
                default: return false;
            }

            return false;
        }

        private void EndAction(Block obj, QTerm qTerm)
        {
        }

        private void OutputAction(Block obj, QTerm qTerm)
        {
            Debug.WriteLine(obj.Content);
            var parsedstring = new Parser(obj.Content);
            qTerm.Definitive = _variables[parsedstring.Lexems[0]];
            _qDeterminant.Add(qTerm);
            FlowChartBypass(FlowChart.Edges.FirstOrDefault(y => y.From == obj.Id).To, qTerm);
        }

        private void InputAction(Block obj, QTerm qTerm)
        {
            Debug.WriteLine(obj.Content);

            var parsedstring = new Parser(obj.Content);
            if (parsedstring.Lexems.Count > 1)
            {
                if (parsedstring.Lexems[1] == "=")
                {
                    _variables.Add(parsedstring.Lexems[0], parsedstring.Lexems[2]);
                    _variableValue.Add(parsedstring.Lexems[0], Convert.ToDouble(parsedstring.Lexems[2]));
                }
                else
                {
                    _variables.Add(parsedstring.Lexems[0], "");
                    _variableValue.Add(parsedstring.Lexems[0], 0);
                }
            }

            FlowChartBypass(FlowChart.Edges.FirstOrDefault(y => y.From == obj.Id).To, qTerm);
        }

        #endregion

        #region public_methods

        public List<string> GetVariables()
        {

            return _variables.Keys.Where(x => String.IsNullOrEmpty(_variables[x])).ToList();

        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            _variables = variables;
        }

        public List<QTerm> GetOptimizationDeterminant()
        {
            return null;
        }

        public List<QTerm> GetDefaultDereminant()
        {
            return _qDeterminant;
        }

        public void CalculateDeterminant()
        {
            var ConvertToPrefix = new ConvertToPrefix();
            var qTerm = new QTerm();
            _variables = new Dictionary<string, string>();
            _variableValue = new Dictionary<string, double>();
            _qDeterminant = new List<QTerm>();

            Parser.SetFunctions(Functions);

            Debug.WriteLine(qTerm);
            //Debug.WriteLine();

            FlowChartBypass(FlowChart.Edges.FirstOrDefault(y => y.From == FlowChart.Vertices.FirstOrDefault(x => x.Type == BlockTypes.Start).Id).To, qTerm);
            ConvertToPrefix.Covert(FlowChart, FlowChart.Edges.FirstOrDefault(y => y.From == FlowChart.Vertices.FirstOrDefault(x => x.Type == BlockTypes.Start).Id).To);
        }



        #endregion

        public QDet()
        {
            _blockActions = new Dictionary<BlockTypes, Action<Block, QTerm>>()
                {
                    {BlockTypes.Condition, ConditionalAction},
                    {BlockTypes.Process, ProcessAction},
                    {BlockTypes.Input, InputAction},
                    {BlockTypes.Output, OutputAction},
                    {BlockTypes.End, EndAction}
                };
        }


    }

}