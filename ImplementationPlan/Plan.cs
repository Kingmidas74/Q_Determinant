using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Converters;
using Core;
using System;

namespace ImplementationPlan
{
    public class Plan
    {
        private List<Operation> _operations;
        private readonly QDet _qDet;
        private List<Graph> _graphs;

        public Plan(List<Operation> operations, QDet qDet)
        {
            _operations = operations;
            _qDet = qDet;
            _graphs = new List<Graph>();
            CountProcessors = 0;
            CountTacts = 0;
            ParseQDetInGraphs();
            CalculateProperties();

        }

        public ulong CountProcessors { get; private set; }

        public ulong CountTacts { get; private set; }

        private void CalculateProperties()
        {
            foreach (var graph in _graphs)
            {
                CountTacts = Max(graph.GetMaxLevel(), CountTacts);
                CountProcessors = CountProcessors + graph.GetMaxOperationsInLevel();
            }
        }

        private void ParseQDetInGraphs()
        {
            foreach (var qTerm in _qDet.QDeterminant)
            {
                if (!String.IsNullOrEmpty(qTerm.Logical))
                {
                    _graphs.Add(ParseQTermInGraph(qTerm.Logical));
                }
                _graphs.Add(ParseQTermInGraph(qTerm.Definitive));
            }
        }

        public void SavePlans()
        {
            var converter = Manufactory.CreateImplementationPlanConverter(ConverterTypes.JSON);
            int i = 0;
            foreach (var graph in _graphs)
            {
                converter.SetBlocks(graph.GetVertexGraphs());
                converter.SetLinks(graph.GetEdgeGraphs());
                converter.SaveToFile(@"D:\tempforQ\plans\"+i.ToString()+".json");
                i++;
            }
        }

        private Graph ParseQTermInGraph(string qTerm)
        {
            var outputString = new StringBuilder("");
            var result = new Graph();
            var stack = new Stack<Operation>();

            var tempString = new StringBuilder("");
            var opFlag = false;
            foreach (var symbol in qTerm)
            {
                if (symbol.Equals('(') == false && symbol.Equals(')') == false)
                {
                    if (opFlag == IsOperand(symbol.ToString(CultureInfo.InvariantCulture)))
                    {
                        tempString.Append(symbol);
                    }
                    else
                    {
                        opFlag = !opFlag;
                        if (!IsOperand(tempString.ToString()))
                        {
                            outputString.Append(tempString);
                            result.AddVertex(0,tempString.ToString());
                        }
                        else
                        {
                            var currentOperation = _operations.First(x => x.Signature.Equals(tempString.ToString()));
                            while (stack.Count > 0 && !((stack.Peek()).Signature.Equals("(")) && currentOperation.Priority <= (stack.Peek()).Priority)
                            {
                                //outputString.Append(stack.Pop().Signature);
                                var content = stack.Pop().Signature;
                                outputString.Append(content);
                                result.AddVertex(0, content);
                            }
                            stack.Push(currentOperation);
                        }
                        tempString.Clear().Append(symbol);
                    }
                }
                else
                {
                    if (symbol.Equals('('))
                    {
                        if (tempString.Length > 0)
                        {
                            var currentOperation = _operations.First(x => x.Signature.Equals(tempString.ToString()));
                            while (stack.Count > 0 && !((stack.Peek()).Signature.Equals("(")) &&
                                   currentOperation.Priority <= (stack.Peek()).Priority)
                            {
                                //outputString.Append(stack.Pop().Signature);
                                var content = stack.Pop().Signature;
                                outputString.Append(content);
                                result.AddVertex(0, content);
                            }
                            stack.Push(currentOperation);
                            tempString.Clear();
                        }
                        opFlag = false;
                        stack.Push(_operations.First(x => x.Signature.Equals("(")));
                    }
                    else if (symbol.Equals(')'))
                    {

                        outputString.Append(tempString);
                        result.AddVertex(0, tempString.ToString());
                        tempString.Clear();
                        opFlag = true;
                        var flag = false;
                        while (flag == false)
                        {
                            var currentOperation = stack.Pop();
                            if (!currentOperation.Signature.Equals("("))
                            {
                                //outputString.Append(currentOperation.Signature);

                                var content = currentOperation.Signature;
                                outputString.Append(content);
                                result.AddVertex(0, content);
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            outputString.Append(tempString);
            result.AddVertex(0, tempString.ToString());
            while (stack.Count > 0)
            {
                //outputString.Append(stack.Pop().Signature);
                var content = stack.Pop().Signature;
                outputString.Append(content);
                result.AddVertex(0, content);
            }
            File.WriteAllText(@"D:\ou1",outputString.ToString());
            var vertexs = result.GetVertexGraphs();
            var links = new List<Link>();
            for (var i=0; i<vertexs.Count; i++)
            {
                if (IsOperand(vertexs[i].Content))
                {
                    vertexs[i].Level = Max(vertexs[i - 1].Level,vertexs[i - 2].Level)+1;
                }
            }
            for (var i = 0; i < vertexs.Count; i++)
            {
                if (IsOperand(vertexs[i].Content))
                {
                    var priorityFlag = false;
                    for (int k = i - 1, countLink = 0; k >= 0 && countLink<2; k--)
                    {
                        if (links.Count(x => x.From == vertexs[k].Id) == 0)
                        {
                            if (priorityFlag == false)
                            {
                                links.Add(new Link() {From = vertexs[k].Id, To = vertexs[i].Id, Type = LinkTypes.False});
                                priorityFlag = true;
                            }
                            else
                            {
                                links.Add(new Link() {From = vertexs[k].Id, To = vertexs[i].Id, Type = LinkTypes.True});
                                priorityFlag = false;
                            }
                            countLink++;
                        }
                    }
                }
            }
            return new Graph(vertexs,links);
        }

        private bool IsOperand(string s)
        {
            return _operations.Find(x => x.Signature.Equals(s)) != null;
        }

        private ulong Max(ulong a, ulong b)
        {
            return a > b ? a : b;
        }
    }
}
