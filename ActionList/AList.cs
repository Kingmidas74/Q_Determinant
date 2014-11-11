using Core;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;

namespace ActionList
{
    public class AList: IAList
    {
        public QDet AL;
        public List<Expressions> Op;
        public List<string> Vs;

        public AList(List<Block> Blocks, List<Link> Links, List<Operation> Oper)
        {
            AL = new QDet();
            AL.QDeterminant = new List<QTerm>();

            Op = new List<Expressions>();

            Vs = new List<string>();

            Link FLink;
            FLink = null;
            var k = new QTerm();

         /*   var test = new Block();
            foreach (var ex in Blocks)
            {
                if (ex.Id == 14)
                    test = ex;
            }

            Debug.WriteLine(test.Content);

            for (int i = 0; i < test.Content.Length; i++)
            {
                if (!test.Content[i].Equals(':'))
                    Debug.WriteLine(i);
            }
            string stri = "abcde";
            for (int i = 0; i < stri.Length; i++)
			{
                if (!stri[i].Equals('d'))
                    Debug.WriteLine(stri[i]);
                else
                    break;
			}*/
           
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

                   // x.Definitive += '(';
                   // x.Definitive += y.Content;
                  //  x.Definitive += ')';
                    getvar(y.Content);
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
                        QQFalse(Blocks, Links, t, z, getvarcond(y.Content));
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content);
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
                        QQFalse(Blocks, Links, t1, z, getvarcond(y.Content));
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content);
                        QQ(Blocks, Links, t, x);
                    }

                }

                if (y.Type == BlockTypes.Input)
                {
                    Vs.Add(y.Content);
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t, x);
                }

                if (y.Type == BlockTypes.Output)
                {
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    x.Definitive = retvalue(y.Content);
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
            z.Logical += pars(getvarcond(s));
            QQ(Blocks, Links, l, z);
        }

       /* private string QtPars(string s)
        {
            string vari = "";
            //vari = getvar(s);

            if (isinops(vari))
            { 
                
            }
            return s;
        }*/

        private string getvarcond(string s)
        {
            var va = new StringBuilder("");
            var tmp = new StringBuilder("");
            var opex = new Expressions();
            string vastr = "";
            string vastr2 = "";

            string vastrtest = "";

            char c, c1;
            string ret = "";
            int t = 0;

            //Debug.WriteLine(s);


            for (int i = 0; i < s.Length; i++)
            {
                if ((!s[i].Equals('<')) && (!s[i].Equals('>')) && (!s[i].Equals('!')) && (!s[i].Equals('=')))
                {
                    va.Append(s[i]);
                }
                else
                {
                    t = i+1;
                    vastr = va.ToString();
                    if (isinops(vastr))
                    {
                        tmp.Append(retvalue(vastr));
                        tmp.Append(s[i]);
                    }
                    else
                    {
                       // opex.name = vastr;
                       // opex.Exp = "";
                       // addtoOp(opex);
                        tmp.Append(vastr);
                        tmp.Append(s[i]);
                    }

                    if (s[t].Equals('='))
                    {
                        tmp.Append(s[t]);
                        t = t + 1;
                    }

                    break;
                }
            }

            va.Clear();

            vastrtest = va.ToString();

           // Debug.WriteLine(s.Length);

            for (int i = t; i < s.Length; i++)
            {
                va.Append(s[i]);
            }
            vastr2 = va.ToString();

            if (isinops(vastr2))
            {
                tmp.Append(retvalue(vastr2));
            }
            else
                tmp.Append(vastr2);

            ret = tmp.ToString();

           // Debug.WriteLine(ret);


            return ret;
        }

        private void getvar(string s)
        {
            var opex = new Expressions();
           // string c = "";
            int temp = 0;
            var va = new StringBuilder("");
            var opexexp = new StringBuilder("");
            string vastr = "";
            string vastr2 = "";
            string vastr3 = "";
            bool isweq = false;
            bool isoperexist = false;
            //bool isempty = false;

            Debug.WriteLine(s, "BLOCK DATA");

            for (int i = 0; i < s.Length; i++)
            {
                //c = s[i];
                if ((!s[i].Equals(':')) && (!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                {
                    //Debug.WriteLine(s[i]);
                    va.Append(s[i]);
                }
                else
                {
                    //Debug.WriteLine(s[i]);
                    temp = i;
                    if ((s[i].Equals(':')))
                    {
                        temp = temp + 2;
                        isweq = true;
                    }

                    break;
                }
                
            }
 
            vastr = va.ToString();


            va.Clear();

            if (!isinops(vastr))
            {
                //Debug.WriteLine(vastr);
                opex.name = vastr;
                opex.Exp = "";
                Op.Add(opex);
                //Debug.WriteLine(opex.name);
                opex.name = "";
            }

            if (!isweq)
            {
                opex.name = vastr;
                //va.Append("");
                for (int i = temp; i < s.Length; i++)
                {
                    va.Append(s[i]);
                }
                opex.Exp = va.ToString();
                addtoOp(opex);
                opex.Exp = "";
                opex.name = "";
            }
            else
            {
                //if (retvalue(vastr).Equals(""))
                   // isempty = true;

                
                    opexexp.Append(retvalue(vastr));

                 //   int l = retvalue(vastr).Length;

                    
                

                for (int i = temp; i < s.Length; i++)
                {
                    //c = s[i];
                    if ((!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                    {
                        va.Append(s[i]);
                        temp = i;
                    }
                    else
                    {
                        //va.Append(s[i]);
                        temp = i + 1;
                        //char c = s[i];
                        isoperexist = true;
                        break;
                    } 
                }
 
                vastr2 = va.ToString();

                //opex.name = va.ToString();


               
                    if (isinops(vastr2))
                        opexexp.Append(retvalue(vastr2));
                    else
                        opexexp.Append(vastr2);

                    if (!isempty(vastr))
                        if (isoperexist)
                            opexexp.Append(s[temp - 1]);
                
                va.Clear();
                //va.Append("");

                if (isoperexist)
                {
                    for (int i = temp; i < s.Length; i++)
                    {
                        va.Append(s[i]);
                    }

                    vastr3 = va.ToString();

                    if (isinops(vastr3))
                        opexexp.Append(retvalue(vastr3));
                    else
                        opexexp.Append(vastr3);
                }
                opex.name = vastr;

                opex.Exp = opexexp.ToString();

                addtoOp(opex);
            }

            //if (isweq)

            /*if (vastr.Length != 0)
            {
                    if (isinops(vastr))
                    {
                        for (int i = temp; i < s.Length; i++)
                        {
                            c = s[i];
                            if ((c != '+') || (c != ':') || (c != '-') || (c != '*') || (c != '/') || (c != '<') || (c != '>'))
                            {
                                va.Append(c);
                            }
                        }
                    }                
            }*/

        }

        private bool isinops(string s)
        {
            foreach (var ex in Op)
            {
               // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                {
                    //Debug.WriteLine("IIO TRUE");
                    return true;
                }
            }
           // Debug.WriteLine("IIO FALSE");
            return false;
        }

        private bool isempty(string s)
        {
            foreach (var ex in Op)
            {
                // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                {
                    //Debug.WriteLine("IIO TRUE");
                    if (ex.Exp.Length == 0)
                    return true;
                }
            }
            // Debug.WriteLine("IIO FALSE");
            return false;
        }

        private string retvalue(string s)
        {
            foreach (var ex in Op)
            {
               // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                    return ex.Exp;
            }
            return null;
        }

        private void addtoOp(Expressions s)
        {
            var temp = new StringBuilder("");
            foreach (var ex in Op)
            {
                //if (String.Equals(s.name, ex.name, StringComparison.Ordinal))
                if (s.name.Equals(ex.name))
                {
                    temp.Append(ex.Exp);
                    //temp.Append("");
                    temp.Append(s.Exp);
                    ex.Exp = temp.ToString();
                }
            }
        }

        private bool isnotinvs(string s)
        {
            foreach (var ex in Vs)
            {
               // if (String.Equals(s, ex, StringComparison.Ordinal))
                if (s.Equals(ex))
                    return false;
            }
            return true;
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
               // Debug.WriteLine(t.Logical);
               // Debug.WriteLine(t.Definitive);
            }
            return AL;
        }

    }

}
