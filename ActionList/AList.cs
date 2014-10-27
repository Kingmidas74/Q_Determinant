using Core;
using System.Collections.Generic;
using System.Linq;

namespace ActionList
{
    public class AList: IAList
    {
        public QDet AL;
        public AList(List<Block> Blocks, List<Link> Links, List<Operation> Oper)
        {

            Link FLink;
            FLink = null; 
           /* var x = new QTerm();
            var y = new QTerm();
            var z = new QTerm();
            x.Logical = "dx>=(5*a+2*(b-1))";
            x.Definitive = "(8+2*5)/(1+3*2-4)";
            AL.QDeterminant.Add(x);
            y.Definitive = "3*x+1";
            AL.QDeterminant.Add(y);
            foreach (var example in Blocks)
            {
                if (example.Type == BlockTypes.Condition)
                { 

                }
            }*/

        foreach (var ex in Links)
            {
                var x = new QTerm();
                var z = new QTerm();
                if (ex.From == 1)
                {
                    var y = Blocks.FirstOrDefault(e => e.Id == ex.To);
                    if (y != null)
                    { 
                        if (y.Type == BlockTypes.Process)
                        {
                            x.Definitive = y.Content;
                        }
                        if (y.Type == BlockTypes.Condition)
                        {
                            z = x;
                            x.Logical = y.Content;
                           // z.Logical = 
                        }
                    }
                    
                }
                FLink = ex;
            }
        if (FLink != null)
        {
            QQ(Blocks, Links, FLink);
        }
        }

        private void QQ(List<Block> Blocks, List<Link> Links, Link l)
        {
            var x = new QTerm();
            var z = new QTerm();
            Link t;
            Link t1;
            t1 = null;
            t = null;
            var y = Blocks.FirstOrDefault(e => e.Id == l.To);
            if (y != null)
            {
                if (y.Type == BlockTypes.Process)
                {
                    x.Definitive = y.Content;
                    AL.QDeterminant.Add(x);
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t);
                }
                if (y.Type == BlockTypes.Condition)
                {
                    foreach (var m in Links)
                    {
                        if (m.From == y.Id)
                            t = m;
                    }
                    foreach (var m in Links)
                        if ((m.From == y.Id) && (m != t))
                            t1 = m;
                    z = x;
                    x.Logical = y.Content;
                    AL.QDeterminant.Add(x);
                    // z.Logical = 
                    //AL.QDeterminant.Add(z);
                    QQ(Blocks, Links, t);
                    QQ(Blocks, Links, t1);
                }
                if ((y.Type == BlockTypes.Input) && (y.Type == BlockTypes.Output))
                {
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t);
                }
            }
   
        }

        private void pars()
        { 
        }

        public QDet getqdet()
        {
            return AL;
        }

       // public bool ex { get; set; }
    }

}
