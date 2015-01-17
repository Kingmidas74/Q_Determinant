using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core.Atoms;
using Core.Serializers.SerializationModels.SolutionModels;

namespace ImplementationPlan.InternalClasses
{
    public class LexemAnalyze
    {
        private List<Function> _lexems;
        private List<Block> _result; 

        public LexemAnalyze(List<Function> functions)
        {
            _lexems = functions;
        }

        public List<Block> AnalyzeTerm(string Term)
        {
            _result = new List<Block>();
            Parse(Term);
            return _result;
        } 

        private void Parse(string term)
        {
            var i = 0;
            var currentLexem = new StringBuilder(string.Empty);
            while (!IsTerminal(term[i]))
            {
                currentLexem.Append(term[i]);
                i++;
            }
            if (term[i].Equals(','))
            {
                _result.Add(new Block { Content = currentLexem.ToString(), Id = 0, Level = 0 });
                Debug.WriteLine(currentLexem, ",");
                Parse(term.Remove(0, i + 1));
            }
            if (term[i].Equals('('))
            {
                if (IsLexem(currentLexem.ToString()))
                {
                    _result.Add(new Block { Content = currentLexem.ToString(), Id = 0, Level = 0 });
                    Debug.WriteLine(currentLexem, "(");
                    Parse(term.Remove(0, i + 1));
                }
                else
                {
                    throw new Exception("unknown lexem: " + currentLexem);
                }
            }
            if (term[i].Equals(')'))
            {
                    _result.Add(new Block { Content = currentLexem.ToString(), Id = 0, Level = 0 });
                    Debug.WriteLine(currentLexem, ")");
                    ParseFromCS(term.Remove(0, i));
            }
        }

        private bool IsTerminal(char symbol)
        {
            return symbol.Equals(')') || symbol.Equals('(') || symbol.Equals(',');
        }

        private void ParseFromCS(string term)
        {
            Debug.WriteLine(term,"TERM");
            if (term[0].Equals(')'))
            {
                if (term.Length > 1)
                {
                    ParseFromCS(term.Remove(0, 1));
                }
            }
            if (term[0].Equals(','))
            {
                Parse(term.Remove(0,1));
            }
        }

        

        private bool IsLexem(string part)
        {
            return _lexems.FirstOrDefault(function => function.Signature.Equals(part)) != null;
        }
    }
}
