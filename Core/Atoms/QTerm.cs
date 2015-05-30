using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Atoms
{
    public class QTerm
    {
        public List<Block> LogicalExpression { get; set; }
        public List<Block> DefenitiveExcpression { get; set; }

        public string Variable { get; set; }

        public QTerm()
        {
            LogicalExpression = new List<Block>();
            DefenitiveExcpression = new List<Block>();
            Variable = string.Empty;
        }

        public double scalar(double[] X, double[] Y)
        {
            double result = 0;
            if (X.Length != Y.Length)
            {
                return result;
            }
            for (ulong i = ulong.MinValue; i < (ulong)X.Length; i = i + 1)
            {
                result = result + X[i]*Y[i];
            }
            return result;
        }
    }
}
