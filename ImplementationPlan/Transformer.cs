using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Core;

namespace ImplementationPlan
{
    internal class Transformer
    {
        private QDet _qDet;
        internal Graph Graph { get; private set; }
        private readonly List<Operation> _operations;

        private ulong _currentId;
        public Transformer(QDet qDet, List<Operation> operations)
        {
            _qDet = qDet;
            _operations = operations;
            _currentId = 1;
            var allBlocks = new List<Block>();
            foreach (var qTerm in qDet.QDeterminant)
            {
                allBlocks.AddRange(ConvertQTermToRPN(qTerm.Logical));
                allBlocks.AddRange(ConvertQTermToRPN(qTerm.Definitive));
            }
            var allLinks = SetLink(allBlocks);
            SetLevels(allBlocks,allLinks);
        }

        private List<Link> SetLink(List<Block> blocks)
        {
            var result = new List<Link>();
            for (var i = 0; i < blocks.Count; i++)
            {
                if (IsOperand(blocks[i].Content))
                {
                    var priorityFlag = false;
                    for (int k = i - 1, countLink = 0; k >= 0 && countLink < 2; k--)
                    {
                        if (result.Count(x => x.From == blocks[k].Id) == 0)
                        {
                            if (priorityFlag == false)
                            {
                                result.Add(new Link() { From = blocks[k].Id, To = blocks[i].Id, Type = LinkTypes.False });
                                priorityFlag = true;
                            }
                            else
                            {
                                result.Add(new Link() { From = blocks[k].Id, To = blocks[i].Id, Type = LinkTypes.True });
                                priorityFlag = false;
                            }
                            countLink++;
                        }
                    }
                }
            }
            return result;
        }

        private void SetLevels(List<Block> blocks, List<Link> links)
        {
            for (var i = 0; i < blocks.Count; i++)
            {
                if (IsOperand(blocks[i].Content))
                {
                    var linkTrueId = links.FirstOrDefault(x => x.To == blocks[i].Id && x.Type == LinkTypes.True).From;
                    var linkFalseId = links.FirstOrDefault(x => x.To == blocks[i].Id && x.Type == LinkTypes.False).From;
                    var levelTrue = blocks.FirstOrDefault(x => x.Id == linkTrueId).Level;
                    var levelFalse = blocks.FirstOrDefault(x => x.Id == linkFalseId).Level;
                    blocks[i].Level = Math.Max(levelFalse, levelTrue) + 1;
                }
            }
            Graph=new Graph(blocks,links);
        }

        private bool IsOperand(string s)
        {
            return _operations.Find(x => x.Signature.Equals(s)) != null;
        }

        private void AddBlockToRPN(string content, ref List<Block> blocks)
        {
            if (string.IsNullOrEmpty(content)) return;
            blocks.Add(new Block() {Content = content, Id = _currentId, Level = 0});
            _currentId++;
        }

        private IEnumerable<Block> ConvertQTermToRPN(string expression)
        {
            var result = new List<Block>();
            if (string.IsNullOrEmpty(expression))
            {
                return result;
            }
            var tempString = new StringBuilder("");
            var stack = new Stack<Operation>();
            var opFlag = false;
            foreach (var symbol in expression)
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
                            AddBlockToRPN(tempString.ToString(), ref result);
                        }
                        else
                        {
                            var currentOperation = _operations.First(x => x.Signature.Equals(tempString.ToString()));
                            while (stack.Count > 0 && !((stack.Peek()).Signature.Equals("(")) && currentOperation.Priority <= (stack.Peek()).Priority)
                            {
                                var content = stack.Pop().Signature;
                                AddBlockToRPN(content, ref result);
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
                                var content = stack.Pop().Signature;
                                AddBlockToRPN(content, ref result);
                            }
                            stack.Push(currentOperation);
                            tempString.Clear();
                        }
                        opFlag = false;
                        stack.Push(_operations.First(x => x.Signature.Equals("(")));
                    }
                    else if (symbol.Equals(')'))
                    {   
                        AddBlockToRPN(tempString.ToString(), ref result);
                        tempString.Clear();
                        opFlag = true;
                        var flag = false;
                        while (flag == false)
                        {
                            var currentOperation = stack.Pop();
                            if (!currentOperation.Signature.Equals("("))
                            {
                                var content = currentOperation.Signature;
                                AddBlockToRPN(content, ref result);
                            }
                            else
                            {
                                flag = true;
                            }
                        }
                    }
                }
            }
            AddBlockToRPN(tempString.ToString(), ref result);
            while (stack.Count > 0)
            {
                var content = stack.Pop().Signature;
                AddBlockToRPN(content, ref result);
            }
            return result;
        }
    }
}
