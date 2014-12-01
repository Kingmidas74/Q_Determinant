using System.Xml.Schema;
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
        //public List<Expressions> Op;
        public List<string> Vs;
        
        public AList(List<Block> blocks, List<Link> links, List<Operation> oper)
        {
            AL = new QDet {QDeterminant = new List<QTerm>()};

            var Op = new List<Expressions>();

            var Opl = new List<List<Expressions>>();

            Vs = new List<string>();

            Link FLink = null;
            var k = new QTerm();
         
        foreach (var ex in links)
            {
                if (ex.From == 1)
                {
                    FLink = ex; 
                }            
            }
        if (FLink != null)
        {
            addvars(blocks, links, FLink, FLink, Op);
            //QQ(blocks, links, FLink, k, Op);
        }
        }

        private void addvars(List<Block> Blocks, List<Link> Links, Link l, Link FLink, List<Expressions> Opx )
        {
            var Ops = new List<Expressions>(Opx);
            //clonelist(Opx, ref Ops);

            var k = new QTerm();
            Link t;
            t = null;
            var opex = new Expressions();
            var temp = new StringBuilder("");
            var y = Blocks.FirstOrDefault(e => e.Id == l.To);

            if (y != null)
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

                Ops.Add(opex);

                t = Links.FirstOrDefault(e => e.From == y.Id);
                addvars(Blocks, Links, t, FLink, Ops);

            }

            if ((y.Type == BlockTypes.Condition) || (y.Type == BlockTypes.Input) || (y.Type == BlockTypes.Output))
            {
                t = Links.FirstOrDefault(e => e.From == y.Id);
                addvars(Blocks, Links, t, FLink, Ops);
            }
            

            if (y.Type == BlockTypes.End)
                QQ(Blocks, Links, FLink, k, Ops);

            
                
        }
        private void QQ(List<Block> Blocks, List<Link> Links, Link l, QTerm x, List<Expressions> Ops)
        {           
            Link t;
            Link t1;
            t1 = null;
            t = null;
            //var Opx = Ops.FindAll(u => u.name.Length > 0);
                /*new List<Expressions>();
            foreach (var ex in Ops)
                Opx.Add(ex);*/
            var Opx = new List<Expressions>(Ops);
            //clonelist(Ops, ref Opx);

            var y = Blocks.FirstOrDefault(e => e.Id == l.To);

            if (y != null)
            {
                if (y.Type == BlockTypes.Process)
                {
                    Debug.WriteLine(y.Content);
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

                    Debug.WriteLine(y.Content);

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


//=============================================================================================================================================================
       /* private void QQFalse(List<Block> Blocks, List<Link> Links, Link l, QTerm x, List<Expressions> Ops)
        {
            Link t;
            Link t1;
            t1 = null;
            t = null;
            //var Opx = Ops.FindAll(u => u.name.Length > 0);
           
            var Opx = new List<Expressions>(Ops);
            //clonelist(Ops, ref Opx);

            var y = Blocks.FirstOrDefault(e => e.Id == l.To);

            if (y != null)
            {
                if (y.Type == BlockTypes.Process)
                {
                    Debug.WriteLine(y.Content);
                    // x.Definitive += '(';
                    // x.Definitive += y.Content;
                    //  x.Definitive += ')';
                    getvar(y.Content, Opx);
                    t = Links.FirstOrDefault(e => e.From == y.Id);
                    QQFalse(Blocks, Links, t, x, Opx);
                }

                if (y.Type == BlockTypes.Condition)
                {
                    foreach (var m in Links)
                        if (m.From == y.Id)
                            t = m;

                    foreach (var m in Links)
                        if ((m.From == y.Id) && (m != t))
                            t1 = m;

                    Debug.WriteLine(y.Content);

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
                        //QQ(Blocks, Links, t1, x, Opx);
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

        }*/
//=============================================================================================================================================================        
        private void QQFalse(List<Block> Blocks, List<Link> Links, Link l, QTerm z, string s, List<Expressions> Opx)
        {
            var Opf = new List<Expressions>();
            foreach (var ex in Opx)
            {
                var temp = new Expressions();
                var tempstrb = new StringBuilder("");
                tempstrb.Append(ex.name);
                temp.name = tempstrb.ToString();
                tempstrb.Clear();
                tempstrb.Append(ex.Exp);
                temp.Exp = tempstrb.ToString();
                tempstrb.Clear();
                Opf.Add(temp);
            }

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

            bool isweq = false;

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

                    break;
                }
            }

            va.Clear();

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

        private void getvar(string s, List<Expressions> Opz)
        {

            var Ops = new List<Expressions>(Opz);
            //clonelist(Opz, ref Ops);

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



            for (int i = 0; i < s.Length; i++)
            {
                if ((!s[i].Equals(':')) && (!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                {
                    va.Append(s[i]);
                }
                else
                {
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


            if (!isinops(vastr, Ops))
            {
                opex.name = vastr;


                //if (opex.name.Equals(vastr))

//==================================================================================================================================================================================

               /* for (int i = temp; i < s.Length; i++)
                {
                        va.Append(s[i]);
                        temp = i;
                }

                
//==================================================================================================================================================================================                
                opex.Exp = va.ToString();*/
                va.Clear();

                Ops.Add(opex);
                opex.name = null;
                opex.Exp = null;
            }

            if (!isweq)
            {
                skobesadd(Ops, vastr);
                opex.name = vastr;
                for (int i = temp; i < s.Length; i++)
                {
                    va.Append(s[i]);
                }
                opex.Exp = va.ToString();
                
                addtoOp(opex, Ops);
                opex.Exp = null;
                opex.name = null;
            }
            else
            {
                    //opexexp.Append(retvalue(vastr, Ops));


                for (int i = temp; i < s.Length; i++)
                {
                    if ((!s[i].Equals('+')) && (!s[i].Equals('-')) && (!s[i].Equals('*')) && (!s[i].Equals('/')))
                    {
                        va.Append(s[i]);
                       // temp = i;
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

                if (isinops(vastr2, Ops))
                {
                    if (!vastr.Equals(vastr2))
                    {
                        opexexp.Append(retvalue(vastr2, Ops));
                    }

                    //  Debug.WriteLine(retvalue(vastr2, Ops), "RETVAL VAR2");
                }
                else
                {
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
                    if ((c != 'e') || String.IsNullOrEmpty(retvalue(vastr, Ops)))
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

                    }
                    else
                        opexexp.Append(vastr3);
                }
                opex.name = vastr;

                opex.Exp = opexexp.ToString();
                opexexp.Clear();

                addtoOp(opex,  Ops);
                opex.Exp = null;
            }


        }

        private void clonelist(List<Expressions> Opx, ref List<Expressions> Opz)
        {
            var temp = new Expressions();
            var tempstbexp = new StringBuilder("");
            var tempstbnm = new StringBuilder("");
            foreach (var ex in Opx)
            {
                tempstbnm.Append(ex.name);
                tempstbexp.Append(ex.Exp);
                temp.name = tempstbnm.ToString();
                temp.Exp = tempstbexp.ToString();
                Opz.Add(temp);
                tempstbnm.Clear();
                tempstbexp.Clear();
            }
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
                    temp.Append(ex.Exp);
                    //temp.Append("");
                    ex.Exp = "";
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

        public void skobesadd(List<Expressions> Ops, string s)
        {
            var temp = new StringBuilder("");
            int tmp = 0;
            char c = 'e';

            foreach (var ex in Ops)
            {
                if (s.Equals(ex.name))
                {
                    for (int i = 0; i < ex.Exp.Length; i++)
                    {
                        if ((ex.Exp[i].Equals('+')) || (ex.Exp[i].Equals('-')) || (ex.Exp[i].Equals('*')) || (ex.Exp[i].Equals('/')))
                        {
                            tmp = tmp + 1;
                            c = ex.Exp[i];
                        }
                    }

                    if (tmp > 0)   //((tmp > 1) || ((tmp == 1) && ((c == '+') || (c == '-'))))
                    {
                        temp.Append("(");
                        temp.Append(ex.Exp);
                        temp.Append(")");
                        ex.Exp = "";
                        ex.Exp = temp.ToString();
                        temp.Clear();
                    }
                }
            }
        }

//==================================================================Cycles Metods======================================================================================

        public List<string> GetSignature()
        {
            var vars = new List<string>();
            return vars;
        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            
        }


//================================================================Cycles Check========================================================================================

        private void CheckCycles(List<Link> links, List<Block> blocks)
        {
            var cycleblockslist = new List<ulong>();
            string needvar1;
            string needvar2;
            string value1;
            string value2;
            string blockvalue;
            var temp = new StringBuilder("");
            int tmp = 0;

            foreach (var ex in links)
            {
                foreach (var ex1 in links)
                {
                    if (ex.To == ex1.To)
                    {
                        var y = blocks.FirstOrDefault(e => e.Id == ex.To);
                        cycleblockslist.Add(y.Id);
                    }
                }
            }

            foreach (var ex in cycleblockslist)
            {
                var b = blocks.FirstOrDefault(e => e.Id == ex);
                temp.Append(b.Content);
                blockvalue = temp.ToString();
                temp.Clear();
                for (int i = 0; i < blockvalue.Length; i++)
                {
                    if ((!blockvalue[i].Equals('<')) && (!blockvalue[i].Equals('>')) && (!blockvalue[i].Equals('!')) && (!blockvalue[i].Equals('=')))
                    {
                        temp.Append(blockvalue[i]);
                    }
                    else
                    {
                        tmp = i+1;
                        needvar1 = temp.ToString();
                        temp.Clear();
                        if (blockvalue[i + 1] == '=')
                            tmp = tmp + 1;
                        break;
                    }
                }

                for (int i = tmp; i < blockvalue.Length; i++)
                {
                    temp.Append(blockvalue[i]);
                }

                needvar2 = temp.ToString();
            }
        }

//====================================================================================================================================================================
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
