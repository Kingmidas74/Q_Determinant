using Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ActionList
{
    public class AList: IAList
    {
        public QDet AL;
        public AList(List<Block> Blocks, List<Link> Links, List<Operation> Oper)
        {
            AL = new QDet();
            AL.QDeterminant = new List<QTerm>();
            Link FLink;
            FLink = null;
            var k = new QTerm();

        foreach (var ex in Links)
            {
                if (ex.From == 1)
                {
                    FLink = ex; 
                }            
            }
        if (FLink != null)
        {
            QQ(Blocks, Links, FLink, k);
        }
        }

        private void QQ(List<Block> Blocks, List<Link> Links, Link l, QTerm x)
        {

            Link t;
            Link t1;
            t1 = null;
            t = null;

            var y = Blocks.FirstOrDefault(e => e.Id == l.To);
            if (y != null)
            {
                if (y.Type == BlockTypes.Process)
                {
                    x.Definitive += '(';
                    x.Definitive += y.Content;
                    x.Definitive += ')';
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t, x);
                }

                if (y.Type == BlockTypes.Condition)
                {
                    foreach (var m in Links)
                        if (m.From == y.Id)
                            t = m;
                    
                    foreach (var m in Links)
                        if ((m.From == y.Id) && (m != t))
                            t1 = m;

                    if (t.Type == LinkTypes.False)
                    {
                        var z = new QTerm();
                        var z1 = new StringBuilder("");
                        var z2 = new StringBuilder("");
                        z1.Append(x.Logical);
                        z2.Append(x.Definitive);
                        z.Logical = z1.ToString();
                        z.Definitive = z2.ToString();
                        QQFalse(Blocks, Links, t, z, y.Content);
                        x.Logical += y.Content;
                        QQ(Blocks, Links, t1, x);
                    }
                    else
                    {
                        var z = new QTerm();
                        var z1 = new StringBuilder("");
                        var z2 = new StringBuilder("");
                        z1.Append(x.Logical);
                        z2.Append(x.Definitive);
                        z.Logical = z1.ToString();
                        z.Definitive = z2.ToString();
                        QQFalse(Blocks, Links, t1, z, y.Content);
                        x.Logical += y.Content;
                        QQ(Blocks, Links, t, x);
                    }

                }

                if ((y.Type == BlockTypes.Input) || (y.Type == BlockTypes.Output))
                {
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t, x);
                }

                if (y.Type == BlockTypes.End)
                {
                    AL.QDeterminant.Add(x);
                }
            }
   
        }

        private void QQFalse(List<Block> Blocks, List<Link> Links, Link l, QTerm z, string s)
        {
            z.Logical += pars(s);
            QQ(Blocks, Links, l, z);
        } 

        private string pars(string x)
        {
            string z = "";

            int y;
            y = x.IndexOf('<');
            if (y != -1)
            {
                z = x.Replace("<", ">");
            }
            else
            {
                y = x.IndexOf('>');
                if (y != -1)
                    z = x.Replace(">", "<");
                else
                {
                    y = x.IndexOf('=');
                    if (y != -1)
                        z = x.Replace("=", "!=");
                    else
                        if (x[y - 1] == '!')
                            z = x.Replace("!=", "=");
                }
            }
            return z;
        }

        public QDet getqdet()
        {
            foreach (var t in AL.QDeterminant)
            {
                Debug.WriteLine(t.Logical);
                Debug.WriteLine(t.Definitive);
            }
            return AL;
        }

    }

}
