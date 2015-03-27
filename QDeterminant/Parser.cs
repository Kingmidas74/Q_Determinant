using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Atoms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Serializers.SerializationModels.SolutionModels;

namespace QDeterminant
{
    internal class Parser
    {
        public List<string> Lexems { get; private set; }
        private static List<Function> _functions;
        private readonly int _maxLength;

        public const string OpenParentheses = "(";
        public const string CloseParentheses = ")";
        public const string SetOperation = "=";
        public const string Separator = ",";

        internal static void SetFunctions(List<Function> functions)
        {
            _functions = functions;
        }

        private static bool IsLexem(string expression)
        {
            return _functions.Select(x => x.Signature).ToList().Contains(expression) || expression.Equals(OpenParentheses) || expression.Equals(CloseParentheses) || expression.Equals(SetOperation);
        }

        internal Parser(string expression)
        {
            Lexems = new List<string>();
            _maxLength = _functions.Max(x => x.Signature.Length);
            Parse(expression, _maxLength, new StringBuilder(string.Empty));
        }

        private void Parse(string expression, int length, StringBuilder currentString)
        {
            if (expression.Length == 0)
            {
                if (currentString.Length > 0)
                {
                    Lexems.Add(currentString.ToString());
                }
                return;
            }
            if (length > expression.Length) { Parse(expression, length - 1, currentString); return; }
            var substring = expression.Substring(0, length);
            if (!IsLexem(substring))
            {
                if (length == 1)
                {
                    currentString.Append(substring);
                    expression = expression.Substring(length);
                    Parse(expression, _maxLength, currentString);
                }
                else
                {
                    Parse(expression, length - 1, currentString);
                }

            }
            else
            {
                var currentFunction = _functions.FirstOrDefault(x => x.Signature.Equals(substring));
                if (currentFunction != null && currentFunction.Priority == FunctionPriorities.Fifth)
                {
                    if (expression[substring.Length].ToString().Equals(OpenParentheses))
                    {
                        if (currentString.Length > 0)
                        {
                            Lexems.Add(currentString.ToString());
                        }
                        Lexems.Add(substring);
                        currentString.Clear();
                        expression = expression.Substring(length);
                        Parse(expression, _maxLength, currentString);
                    }
                    else
                    {
                        currentString.Append(substring);
                        expression = expression.Substring(length);
                        Parse(expression, _maxLength, currentString);
                    }
                }
                else
                {
                    if (currentString.Length > 0)
                    {
                        Lexems.Add(currentString.ToString());
                    }
                    Lexems.Add(substring);
                    currentString.Clear();
                    expression = expression.Substring(length);
                    Parse(expression, _maxLength, currentString);
                }
            }

        }
    }
}

