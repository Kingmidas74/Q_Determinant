using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan
{
    internal static class ReversePolishNotation
    {
        private static List<Function> _functions; 
        private static ulong _currentId;
        public static List<Function> Functions
        {
            get { return _functions; }
            set
            {
                _functions = value;
                _functions.Add(new Function() { Parameters = 0, Priority = 0, Signature = "(" });
                _functions.Add(new Function() { Parameters = 0, Priority = 0, Signature = ")" });
            }
        }

        public static List<Block> Translate(string term)
        {
            var result = new List<Block>();
            if (string.IsNullOrEmpty(term))
            {
                return result;
            }
            var currentSegment = new StringBuilder("");
            var stack = new Stack<Function>();
            foreach (var symbol in term)
            {
                if (IsParentheses(symbol) || symbol.Equals(','))
                {
                    if (symbol.Equals('('))
                    {
                        var currentFunction = _functions.First(x => x.Signature.Equals(symbol.ToString()));
                        stack.Push(currentFunction);
                    }
                    else if (symbol.Equals(')'))
                    {
                        AddBlockToRPN(currentSegment.ToString(), ref result, ref _currentId);
                        while (stack.Count > 0 && !(stack.Peek()).Signature.Equals("("))
                        {
                            
                            var content = stack.Pop();
                            AddBlockToRPN(content.Signature, ref result, ref _currentId);
                        }
                        var content2 = stack.Pop();
                        currentSegment.Clear();
                    }
                    else if (symbol.Equals(','))
                    {
                        AddBlockToRPN(currentSegment.ToString(), ref result, ref _currentId);
                        while (stack.Count > 0 && !(stack.Peek()).Signature.Equals("("))
                        {
                            var content = stack.Pop();
                            AddBlockToRPN(content.Signature, ref result, ref _currentId);
                        }
                        currentSegment.Clear();
                    }
                }
                else
                {
                    if (!IsFunction(symbol.ToString()))
                    {
                        currentSegment.Append(symbol);
                        if (IsFunction(currentSegment.ToString()))
                        {
                            var currentFunction = _functions.First(x => x.Signature.Equals(currentSegment.ToString()));
                            while (stack.Count > 0 && currentFunction.Priority <= (stack.Peek()).Priority &&
                                   !(stack.Peek()).Signature.Equals("("))
                            {
                                var content = stack.Pop();
                                AddBlockToRPN(content.Signature, ref result, ref _currentId);
                            }
                            stack.Push(currentFunction);
                            currentSegment.Clear();
                        }
                    }
                    else
                    {
                        currentSegment.Clear();
                        var currentFunction = _functions.First(x => x.Signature.Equals(symbol.ToString()));
                        while (stack.Count > 0 && currentFunction.Priority <= (stack.Peek()).Priority &&
                               !(stack.Peek()).Signature.Equals("("))
                        {
                            var content = stack.Pop();
                            AddBlockToRPN(content.Signature, ref result, ref _currentId);
                        }
                        stack.Push(currentFunction);
                    }
                }
            }
            AddBlockToRPN(currentSegment.ToString(), ref result, ref _currentId);
            while (stack.Count > 0)
            {
                var content = stack.Pop().Signature;
                AddBlockToRPN(content, ref result, ref _currentId);
            }
            foreach (var block in result)
            {
                Debug.Write(block.Content);
            }
            return result;
        }

        public static void RefreshId()
        {
            _currentId = 1;
        }

        public static bool IsFunction(string signature)
        {
            return Functions.Exists(function => function.Signature.Equals(signature));
        }

        private static void AddBlockToRPN(string content, ref List<Block> vertices, ref ulong currentId)
        {
            if (string.IsNullOrEmpty(content)) return;
            vertices.Add(new Block { Content = content, Id = currentId, Level = 0 });
            currentId++;
        }

        private static bool IsParentheses(char symbol)
        {
            return (symbol.Equals('(') || symbol.Equals(')'));
        }

        private static void InvertBoolVariable(ref bool variable)
        {
            variable = !variable;
        }
    }
}
