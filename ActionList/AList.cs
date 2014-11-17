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
            //addvars(Blocks, Links, FLink, FLink);
            QQ(Blocks, Links, FLink, k, Op);
        }
        }

        private void addvars(List<Block> Blocks, List<Link> Links, Link l, Link FLink)
        {
            var k = new QTerm();
            Link t;
            t = null;
            var opex = new Expressions();
            var temp = new StringBuilder("");
            var y = Blocks.FirstOrDefault(e => e.Id == l.To);
            if (y.Type == BlockTypes.Process)
            {
                for (int i = 0; i < y.Content.Length; i++)
                {
                    if (y.Content[i] != ':')
                        temp.Append(y.Content[i]);
                    else
                        break;
                }
                opex.name = temp.ToString();
                opex.Exp = null;
                Op.Add(opex);
            }
            

            if (y.Type == BlockTypes.End)
                QQ(Blocks, Links, FLink, k, Op);

            t = Links.FirstOrDefault(e => e.From == y.Id);
            addvars(Blocks, Links, t, FLink);
                
        }
        private void QQ(List<Block> Blocks, List<Link> Links, Link l, QTerm x, List<Expressions> Ops)
        {           
            Link t;
            Link t1;
            t1 = null;
            t = null;
            var Opx = new List<Expressions>();
            foreach (var ex in Ops)
                Opx.Add(ex);

            

            var y = Blocks.FirstOrDefault(e => e.Id == l.To);


            if (y.Type == BlockTypes.Process)
                foreach (var ex in Ops)
                {
                    Debug.WriteLine(ex.name);
                    Debug.WriteLine(ex.Exp);
                }

            if (y != null)
            {
                if (y.Type == BlockTypes.Process)
                {

                   // x.Definitive += '(';
                   // x.Definitive += y.Content;
                  //  x.Definitive += ')';
                    getvar(y.Content, Opx);
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t, x, Opx);
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
                        QQFalse(Blocks, Links, t, z, getvarcond(y.Content, Opx), Opx);
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content, Opx);
                        QQ(Blocks, Links, t1, x, Opx);
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
                        QQFalse(Blocks, Links, t1, z, getvarcond(y.Content, Opx), Opx);
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content, Opx);
                        QQ(Blocks, Links, t, x, Opx);
                    }

                }

                if (y.Type == BlockTypes.Input)
                {
                    Vs.Add(y.Content);
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQ(Blocks, Links, t, x, Opx);
                }

                if (y.Type == BlockTypes.Output)
                {
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    x.Definitive = retvalue(y.Content, Opx);
                    QQ(Blocks, Links, t, x, Opx);
                }

                if (y.Type == BlockTypes.End)
                {
                    AL.QDeterminant.Add(x);
                }
            }
   
        }

        private void QQFalse(List<Block> Blocks, List<Link> Links, Link l, QTerm z, string s, List<Expressions> Opx)
        {
            var Opf = new List<Expressions>();
            foreach (var ex in Opx)
                Opf.Add(ex);

            z.Logical += pars(getvarcond(s, Opf));
            QQ(Blocks, Links, l, z, Opf);
        }

        private string getvarcond(string s, List<Expressions> Ops)
        {
            var va = new StringBuilder("");
            var tmp = new StringBuilder("");
            var opex = new Expressions();
            string vastr = "";
            string vastr2 = "";

            string vastrtest = "";
            bool isweq = false;

            char c, c1;
            string ret = "";
            int t = 0;


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
                    //Debug.WriteLine(vastr, "CONDITION BLOCK!!! 1st VAR ");
                    if (s[t].Equals('='))
                    {
                        t = t + 1;
                        isweq = true;
                    }
                    if (isinops(vastr, Ops))
                    {
                        tmp.Append(retvalue(vastr, Ops));
                        tmp.Append(s[i]);
                        if (isweq)
                            tmp.Append(s[i + 1]);
                    }
                    else
                    {
                       // opex.name = vastr;
                       // opex.Exp = "";
                       // addtoOp(opex);
                        tmp.Append(vastr);
                        tmp.Append(s[i]);
                        if (isweq)
                            tmp.Append(s[i+1]);
                    }

                  /*  if (s[t].Equals('='))
                    {
                        tmp.Append(s[t]);
                        t = t + 1;
                    }*/

                    break;
                }
            }

            va.Clear();

            vastrtest = va.ToString();

            for (int i = t; i < s.Length; i++)
            {
                va.Append(s[i]);
            }
            vastr2 = va.ToString();

            if (isinops(vastr2, Ops))
            {
                tmp.Append(retvalue(vastr2, Ops));
            }
            else
                tmp.Append(vastr2);
            //Debug.WriteLine(vastr2, "CONDITION BLOCK!!! 2st VAR ");

            ret = tmp.ToString();

            return ret;
        }

        private void getvar(string s, List<Expressions> Ops)
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
            char c = 'e';
            //bool isempty = false;

            Debug.WriteLine("");
            Debug.WriteLine(s, "BLOCK DATA");

            for (int i = 0; i < s.Length; i++)
            {
                if ((!s[i].Equals(':')) && (!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                {
                    va.Append(s[i]);
                }
                else
                {

                    Debug.WriteLine(s[i], "1st OPERATION ");

                    temp = i;
                    if ((s[i].Equals(':')))
                    {
                        Debug.WriteLine(s[i + 1], "1st OP WEQ ");
                        temp = temp + 2;
                        isweq = true;
                    }

                    break;
                }
                
            }
 
            vastr = va.ToString();


            va.Clear();

            Debug.WriteLine(vastr, "1st VAR ");

            if (!isinops(vastr, Ops))
            {
                opex.name = vastr;
                Debug.WriteLine(opex.name, "TO OPS FIRST TIME");

                if (opex.name.Equals(vastr))
                    Debug.WriteLine("THEY ARE EQUALS!!!!!!!!!!!!!!!!!!!");

//==================================================================================================================================================================================

                for (int i = temp; i < s.Length; i++)
                {
                        va.Append(s[i]);
                        temp = i;
                }

                
//==================================================================================================================================================================================                
                opex.Exp = va.ToString();
                va.Clear();

                Ops.Add(opex);
                opex.name = null;
                opex.Exp = null;
                Debug.WriteLine("1st VAR NOT IN OPS");
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

                Debug.WriteLine(opex.Exp, "W/O EQ ");

                addtoOp(opex, Ops);
                opex.Exp = null;
                opex.name = null;
            }
            else
            {
                    //opexexp.Append(retvalue(vastr, Ops));

                    Debug.WriteLine(retvalue(vastr, Ops), "IN LIST");

                for (int i = temp; i < s.Length; i++)
                {
                    if ((!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                    {
                        va.Append(s[i]);
                        temp = i;
                    }
                    else
                    {
                        c = s[i];
                        //va.Append(s[i]);
                        temp = i + 1;
                        //char c = s[i];
                        isoperexist = true;
                        break;
                    } 
                }
 
                vastr2 = va.ToString();

                //opex.name = va.ToString();
                Debug.WriteLine(vastr2, "2nd VAR");

                if (isinops(vastr2, Ops))
                {
                    Debug.WriteLine("IS IN OPS");
                    if (!vastr.Equals(vastr2))
                    {
                        Debug.WriteLine("VAR 1 NOT EQ VAR 2");
                        opexexp.Append(retvalue(vastr2, Ops));
                    }

                    //  Debug.WriteLine(retvalue(vastr2, Ops), "RETVAL VAR2");
                }
                else
                {
                    Debug.WriteLine("IS NOT IN OPS");
                    opexexp.Append(vastr2);
                }

                /*    if (!isempty(vastr, Ops))
                        if (isoperexist)
                            opexexp.Append(s[temp - 1]);*/
                
                va.Clear();
                //va.Append("");
               // Debug.WriteLine(vastr2, "2nd VAR");

                if (isoperexist)
                {
                    if ((c != 'e') || (retvalue(vastr, Ops) != null))
                    {
                        opexexp.Append(c);    
                    }
                    
                    for (int i = temp; i < s.Length; i++)
                    {
                        va.Append(s[i]);
                    }

                    vastr3 = va.ToString();

                    if (isinops(vastr3, Ops))
                    {
                        if (!vastr.Equals(vastr3))
                        {
                            opexexp.Append(retvalue(vastr3, Ops));
                        }

                        //Debug.WriteLine(retvalue(vastr3, Ops), "RETVAL VAR3");
                    }
                    else
                        opexexp.Append(vastr3);
                }
                opex.name = vastr;

                opex.Exp = opexexp.ToString();
                opexexp.Clear();

                addtoOp(opex, Ops);
                opex.Exp = null;
            }

            //Debug.WriteLine(opexexp, "TO OPER");
            //Debug.WriteLine(vastr3, "3rd VAR");

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

        private bool isinops(string s, List<Expressions> Ops)
        {
            foreach (var ex in Ops)
            {
               // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                {
                    return true;
                }
            }
            return false;
        }

        private bool isempty(string s, List<Expressions> Ops)
        {
            foreach (var ex in Ops)
            {
                // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                {
                    if (String.IsNullOrEmpty(ex.Exp))
                    return true;
                }
            }
            return false;
        }

        private string retvalue(string s, List<Expressions> Ops)
        {
            foreach (var ex in Ops)
            {
               // if (String.Equals(s, ex.name, StringComparison.Ordinal))
                if (s.Equals(ex.name))
                    return ex.Exp;
            }
            return null;
        }

        private void addtoOp(Expressions s, List<Expressions> Ops)
        {
            var temp = new StringBuilder("");
            foreach (var ex in Ops)
            {
                //if (String.Equals(s.name, ex.name, StringComparison.Ordinal))
                if (s.name.Equals(ex.name))
                {
                    Debug.WriteLine("");
                    Debug.WriteLine(s.name, "TO OPS NAME");
                    Debug.WriteLine(s.Exp, "TO OPS EXP");

                    Debug.WriteLine(ex.Exp, "IN OPS EXP");
                    temp.Append(ex.Exp);
                    //temp.Append("");
                    ex.Exp = null;
                    Debug.WriteLine(ex.Exp, "IN OPS EXP AFTER NULL");
                    temp.Append(s.Exp);
                    ex.Exp = temp.ToString();

                    Debug.WriteLine(ex.Exp);
                    Debug.WriteLine("");
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
