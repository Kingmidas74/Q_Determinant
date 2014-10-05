using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;

namespace ImplementationPlan
{
    public class Plan
    {
        private List<Operation> _operations;

        public Plan()
        {
            _operations = new List<Operation>()
            {
                new
            };
        }

        /* private QDet qDet;
        private List<Graph> graphs;
        private readonly List<string> _operations;

        public Plan(QDet qDet)
        {
            this.qDet = qDet;
            _operations = new List<string>() { "+", "-", "*", "/", ">", "<", "=", "!=", ">=", "<=", "!" };
            graphs=new List<Graph>();
        }

        private void Abalyze()
        {
            foreach (var qTerm in qDet.QDeterminant)
            {
                graphs.Add(new Graph());
                if (!String.IsNullOrEmpty(qTerm.Logical))
                {
                    
                    
                }
            }
        }

        private void GetContentVertexFromString(string str) 
        {

        }*/
    }
}
