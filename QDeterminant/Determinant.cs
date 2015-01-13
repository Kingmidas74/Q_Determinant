using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Core.Atoms;
using Core.Enums;
using Core.Interfaces;
using Core.Serializers.SerializationModels.SolutionModels;
using QDeterminant.InternalClasses;

namespace QDeterminant
{
    public class Determinant:IDeterminant
    {
        public List<string> Vs;
        public List<ulong> ViewedBlocks;
        public Dictionary<string, ulong> Vars;
        private Dictionary<ulong, string> VBlocksOpers;
        private Dictionary<ulong, string> FirstVars;
        private Dictionary<ulong, string> SecondVars;

        private Dictionary<ulong, ulong> CommitTest; // must be deleted 

        public StatusTypes Status { get; private set; }

        public string StatusMessage { get; private set; }

        private Dictionary<string,string> _variables;

        private List<QTerm> _qDeterminantStandart;
        private List<QTerm> _qDeterminantModern;
        private Graph _flowChart;
        private List<Function> _functions;
        private List<ActionList> _actionLists; 

        public Graph FlowChart
        {
            set { _flowChart = value; }
        }

        public List<Function> Functions
        {
            set { _functions=value; }
        }

        public Determinant()
        {
            _qDeterminantStandart = new List<QTerm>();
            _actionLists = new List<ActionList>();
            _variables = new Dictionary<string, string>();
            Status = StatusTypes.Success;
        }
        public List<string> GetVariables()
        {
            return _variables.Keys.ToList();
        }

        public void SetVariables(Dictionary<string, string> variables)
        {
            _variables = variables;
        }

        public void CalculateDeterminant()
        {
            var Op = new List<Expressions>();
            ViewedBlocks = new List<ulong>();
            Vars = new Dictionary<string, ulong>();
            VBlocksOpers = new Dictionary<ulong, string>();
            FirstVars = new Dictionary<ulong, string>();
            SecondVars = new Dictionary<ulong, string>();

           /* _qDeterminantStandart.Add(new QTerm {Definitive = "(5+3+8)/(3*5-7)+3*5", Logical = "5+5+5+5>=8*(5+5+7+1)"});
            TransformDeterminant();*/

            var fLink = _flowChart.Edges.FirstOrDefault(edge => edge.From == 1);

            
            if (fLink != null)
            {
                addvars(_flowChart.Vertices, _flowChart.Edges, fLink, fLink, Op);
                //QQ(blocks, links, FLink, k, Op);
            }

        }

        private void addvars(List<Block> Blocks, List<Link> Links, Link l, Link FLink, List<Expressions> Opx)
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
            int tp = 1;
            int fp = 1;

            var Opx = new List<Expressions>(Ops);

            var y = Blocks.FirstOrDefault(e => e.Id == l.To);
            bool iscycled = false;

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
                    foreach (var ex in ViewedBlocks)
                    {
                        if (y.Id.Equals(ex))
                        {
                            iscycled = true;
                        }
                    }

                    if (!iscycled)
                        ViewedBlocks.Add(y.Id);

                    if (iscycled)
                    {
                        switch (VBlocksOpers[y.Id])
                        {
                            case ">":
                                if (Vars[FirstVars[y.Id]] > Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;

                            case "<":
                                if (Vars[FirstVars[y.Id]] < Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;

                            case "<=":
                                if (Vars[FirstVars[y.Id]] <= Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;

                            case ">=":
                                if (Vars[FirstVars[y.Id]] >= Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;

                            case "=":
                                if (Vars[FirstVars[y.Id]] == Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;

                            case "!=":
                                if (Vars[FirstVars[y.Id]] != Vars[SecondVars[y.Id]])
                                {
                                    fp = 0;
                                }
                                else
                                {
                                    tp = 0;
                                }
                                break;
                        }
                    }

                    foreach (var m in Links)
                        if (m.From == y.Id)
                            t = m;

                    foreach (var m in Links)
                        if ((m.From == y.Id) && (m != t))
                            t1 = m;

                    Debug.WriteLine(y.Content);

                    if (t.Type == LinkTypes.FalseBrunch)
                    {
                        var z = new QTerm();
                        var z1 = new StringBuilder("");
                        var z2 = new StringBuilder("");
                        z1.Append(x.Logical);
                        z2.Append(x.Definitive);
                        z.Logical = z1.ToString();
                        z.Definitive = z2.ToString();
                        if (fp == 1)
                            QQFalse(Blocks, Links, t, z, getvarcond(y.Content, Opx, iscycled, y.Id), Opx);
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content, Opx, iscycled, y.Id);
                        if (tp == 1)
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
                        if (fp == 1)
                            QQFalse(Blocks, Links, t1, z, getvarcond(y.Content, Opx, iscycled, y.Id), Opx);
                        //x.Logical += y.Content;
                        x.Logical += getvarcond(y.Content, Opx, iscycled, y.Id);
                        if (tp == 1)
                            QQ(Blocks, Links, t, x, Opx);
                    }

                }

                if (y.Type == BlockTypes.Input)
                {
                    int k = 0;
                    var inptmp = new StringBuilder("");
                    var inpvar = new StringBuilder("");
                    for (int i = 0; i < y.Content.Length; i++)
                    {
                        inpvar.Append(y.Content[i]);
                        if (y.Content[i].Equals('='))
                        {
                            k = i + 1;
                            break;
                        }
                    }
                    if (k != 0)
                    {
                        for (int j = k; j < y.Content.Length; j++)
                        {
                            inptmp.Append(y.Content[j]);
                        }
                        if (IsDigit(inptmp.ToString()))
                            Vars.Add(inpvar.ToString(), Convert.ToUInt64(inptmp.ToString()));
                    }

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
                    _qDeterminantModern.Add(x);
                }
            }

        }

        private void QQFalse(List<Block> Blocks, List<Link> Links, Link l, QTerm z, string s, List<Expressions> Opx)
        {
            bool iscycled = false;
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

            z.Logical += pars(getvarcond(s, Opf, iscycled, 0));
            QQ(Blocks, Links, l, z, Opf);
        }

        private string getvarcond(string s, List<Expressions> Ops, bool iscycled, ulong id)
        {
            var va = new StringBuilder("");
            var tmp = new StringBuilder("");
            var opex = new Expressions();
            string vastr = "";
            string vastr2 = "";

            bool isweq = false;

            string ret = "";
            int t = 0;
            var opertemp = new StringBuilder("");
            string oper;

            for (int i = 0; i < s.Length; i++)
            {

                if ((!s[i].Equals('<')) && (!s[i].Equals('>')) && (!s[i].Equals('!')) && (!s[i].Equals('=')))
                {
                    va.Append(s[i]);
                }
                else
                {
                    opertemp.Append(s[i]);
                    t = i + 1;
                    vastr = va.ToString();
                    //Debug.WriteLine(vastr, "CONDITION BLOCK!!! 1st VAR ");
                    if (s[t].Equals('='))
                    {
                        opertemp.Append("=");
                        t = t + 1;
                        isweq = true;
                    }
                    if (!isinops(vastr, Ops))
                        tmp.Append('(');

                    if (isinops(vastr, Ops))
                    {
                        tmp.Append(retvalue(vastr, Ops));
                        if (!isinops(vastr, Ops))
                            tmp.Append(')');

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
                        tmp.Append(')');
                        tmp.Append(s[i]);
                        if (isweq)
                            tmp.Append(s[i + 1]);
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

            /*  if(!isinops(vastr2, Ops))
                  tmp.Append('(');*/

            if (isinops(vastr2, Ops))
            {
                tmp.Append(retvalue(vastr2, Ops));
            }
            else
                tmp.Append(vastr2);
            //Debug.WriteLine(vastr2, "CONDITION BLOCK!!! 2st VAR ");
            /*  if (!isinops(vastr2, Ops))
                  tmp.Append(')');*/

            ret = tmp.ToString();

            oper = opertemp.ToString();
            if (iscycled)
            {
                FirstVars.Add(id, vastr);
                SecondVars.Add(id, vastr2);

                if (_variables.ContainsKey(vastr))
                {
                    if (!String.IsNullOrEmpty(_variables[vastr]))
                    {
                        if (!Vars.ContainsKey(vastr))
                            Vars.Add(vastr, Convert.ToUInt64(_variables[vastr]));
                    }
                }
                else
                {
                    if (!Vars.ContainsKey(vastr))
                        _variables.Add(vastr, "");
                }

                if (_variables.ContainsKey(vastr2))
                {
                    if (!String.IsNullOrEmpty(_variables[vastr2]))
                    {
                        if (!Vars.ContainsKey(vastr2))
                            Vars.Add(vastr, Convert.ToUInt64(_variables[vastr2]));
                    }
                }
                else
                {
                    if (!Vars.ContainsKey(vastr2))
                        _variables.Add(vastr2, "");
                }

                if (!VBlocksOpers.ContainsKey(id))
                    VBlocksOpers.Add(id, oper);
            }

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
            string oper1;
            string oper2;
            string oper3;
            var oper1stb = new StringBuilder("");
            var oper2stb = new StringBuilder("");
            var tmp = new StringBuilder("");

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
                    else
                    {
                        if ((s[i] == '+') && (s[i + 1] == '+'))
                        {
                            if (Vars.ContainsKey(va.ToString()))
                                Vars[va.ToString()] = Vars[va.ToString()] + 1;
                        }
                        else
                            if ((s[i] == '-') && (s[i + 1] == '-'))
                            {
                                if (Vars.ContainsKey(va.ToString()))
                                    Vars[va.ToString()] = Vars[va.ToString()] - 1;
                            }
                            else
                            {
                                oper1stb.Append(s[i]);
                                oper1 = oper1stb.ToString();
                                for (int j = i + 1; j < s.Length; j++)
                                {
                                    tmp.Append(s[j]);
                                }
                                if (IsDigit(tmp.ToString()))
                                {
                                    switch (oper1)
                                    {
                                        case "+":
                                            Vars[va.ToString()] = Vars[va.ToString()] + Convert.ToUInt64(tmp.ToString());
                                            break;
                                        case "*":
                                            Vars[va.ToString()] = Vars[va.ToString()] * Convert.ToUInt64(tmp.ToString());
                                            break;
                                        case "/":
                                            Vars[va.ToString()] = Vars[va.ToString()] / Convert.ToUInt64(tmp.ToString());
                                            break;
                                        case "-":
                                            Vars[va.ToString()] = Vars[va.ToString()] - Convert.ToUInt64(tmp.ToString());
                                            break;
                                    }
                                }
                                else
                                {
                                    if (Vars.ContainsKey(tmp.ToString()))
                                    {
                                        switch (oper1)
                                        {
                                            case "+":
                                                Vars[va.ToString()] = Vars[va.ToString()] + Convert.ToUInt64(Vars[tmp.ToString()]);
                                                break;
                                            case "*":
                                                Vars[va.ToString()] = Vars[va.ToString()] * Convert.ToUInt64(Vars[tmp.ToString()]);
                                                break;
                                            case "/":
                                                Vars[va.ToString()] = Vars[va.ToString()] / Convert.ToUInt64(Vars[tmp.ToString()]);
                                                break;
                                            case "-":
                                                Vars[va.ToString()] = Vars[va.ToString()] - Convert.ToUInt64(Vars[tmp.ToString()]);
                                                break;
                                        }
                                    }
                                }

                            }

                    }

                    break;
                }

            }

            vastr = va.ToString();


            va.Clear();


            if (!isinops(vastr, Ops))
            {
                opex.name = vastr;

                va.Clear();

                Ops.Add(opex);
                opex.name = null;
                opex.Exp = null;
            }

            if (!isweq)
            {
                //skobesadd(Ops, vastr);
                opex.name = vastr;
                if (String.IsNullOrEmpty(retvalue(vastr, Ops)))
                    temp = temp + 1;
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
                        oper2stb.Append(s[i]);
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
                if (!vastr2.Equals("0"))
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



                va.Clear();
                //va.Append("");
                // Debug.WriteLine(vastr2, "2nd VAR");

                if (isoperexist)
                {
                    skobesadd(Ops, vastr);
                    oper2 = oper2stb.ToString();
                    if (String.IsNullOrEmpty(retvalue(vastr, Ops)))
                        c = 'e';
                    if ((c != 'e'))
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

                    if (Vars.ContainsKey(vastr))
                    {
                        if (vastr.Equals(vastr2))
                        {
                            if (Vars.ContainsKey(vastr3))
                            {
                                switch (oper2)
                                {
                                    case "+":
                                        Vars[vastr] = Vars[vastr] + Vars[vastr3];
                                        break;
                                    case "*":
                                        Vars[vastr] = Vars[vastr] * Vars[vastr3];
                                        break;
                                    case "/":
                                        Vars[vastr] = Vars[vastr] / Vars[vastr3];
                                        break;
                                    case "-":
                                        Vars[vastr] = Vars[vastr] - Vars[vastr3];
                                        break;
                                }
                            }
                            else
                            {
                                if (IsDigit(vastr3))
                                {
                                    switch (oper2)
                                    {
                                        case "+":
                                            Vars[vastr] = Vars[vastr] + Convert.ToUInt64(vastr3);
                                            break;
                                        case "*":
                                            Vars[vastr] = Vars[vastr] * Convert.ToUInt64(vastr3);
                                            break;
                                        case "/":
                                            Vars[vastr] = Vars[vastr] / Convert.ToUInt64(vastr3);
                                            break;
                                        case "-":
                                            Vars[vastr] = Vars[vastr] - Convert.ToUInt64(vastr3);
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // here'll be counted all variants of replacement vastr variable
                        }
                    }
                    //skobesadd(Ops, vastr);
                }
                else
                {
                    if (Vars.ContainsKey(vastr))
                        if (IsDigit(vastr2))
                            Vars[vastr] = Convert.ToUInt64(vastr2);
                        else
                        {
                            if (Vars.ContainsKey(vastr2))
                                Vars[vastr] = Vars[vastr2];
                        }
                }



                opex.name = vastr;
                opex.Exp = opexexp.ToString();
                opexexp.Clear();

                addtoOp(opex, Ops);
                opex.Exp = null;

                if (isinops(vastr, Ops))
                    skobesadd(Ops, vastr);
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
                    //tmp = 1;
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

        bool IsDigit(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i])) ;
                return false;
            }
            return true;
        }

//====================================================================================================================================================================
        private void scobesPN(string s, int n)
        {

            var tmp = new StringBuilder("");
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '(')
                {
                    tmp.Append(chains(s, i));
                    s = "";
                    s = tmp.ToString();
                    tmp.Clear();
                }
            }
        }

        private string chains(string s, int n)
        {
            int opscount = 0;
            string oper = "";
            var tmp = new StringBuilder("");
            var strpart1 = new StringBuilder("");
            var strpart2 = new StringBuilder("");

            /*  for (int i = n; i < s.Length; i++)
              {
                  if (s[i] == ')')
                      break;
                  if ((s[i] == '<') || (s[i] == '>') || (s[i] == '+') || (s[i] == '-') || (s[i] == '*') || (s[i] == '/'))
                      opscount ++;
              }*/

            if (opscount > 1)
            {
                for (int i = n; i < s.Length; i++)
                {
                    if (s[i] == ')')
                        break;
                    if ((s[i] == '*') || (s[i] == '/'))
                    {
                        tmp.Append(s[i]);

                        oper = tmp.ToString();
                        tmp.Clear();
                    }
                }
            }
            return s;
        }
        private void OPN(string s)
        {

            var ops = new StringBuilder("");
            var scobes = new StringBuilder("");

            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] == '*') || (s[i] == '+'))
                {
                    ops.Append(s[i]);
                }

                if ((s[i]) == '(')
                {
                    scobes.Append('(');
                }
            }
        }

        private void chain(string s)
        {
            var opn = new StringBuilder("");
            var ops = new Dictionary<int, string>();
            var tmp = new StringBuilder("");
            var opstemp = new StringBuilder("");
            var vars = new Dictionary<int, string>();
            int k = 0;
            int t = 0;
            int l = 0;
            for (int i = 0; i < s.Length; i++)
            {
                tmp.Append(s[i]);

                if ((s[i] == '*') || (s[i] == '+'))
                {
                    opstemp.Append(s[i]);
                    if (s[i + 1].Equals('='))
                    {
                        opstemp.Append(s[i + 1]);
                        i = i + 1;
                        ops.Add(t, opstemp.ToString());
                        t = t + 1;
                        opstemp.Clear();
                    }
                    vars.Add(k, tmp.ToString());
                    k = k + 1;
                    tmp.Clear();
                }

            }

            for (int i = 0; i < k; i++)
            {
                opn.Insert(0, ops[i]);
                opn.Insert(1, '(');
                opn.Append(vars[l]);
                l = l + 1;
                opn.Append(',');
                opn.Append(vars[l]);
                l = l + 1;
                opn.Append(')');
            }
        }


//====================================================================================================================================================================

        public List<QTerm> GetOptimizationDeterminant()
        {
            return _qDeterminantModern;
        }

        public List<QTerm> GetDefaultDereminant()
        {
            return _qDeterminantStandart;
        }

        private void TransformDeterminant()
        {
            
            /*_qDeterminantModern = new List<QTerm>
            {
                new QTerm
                {
                    Definitive = "+(/(+(+(5,1),8),-(*(1,5),7)),*(1,5))",
                    Logical = ">=(+(+(9,a),+(5,5)),*(8,+(+(5,5),+(7,1))))",
                    Index = 0
                }/*,
                new QTerm
                {
                    Definitive = "+(/(+(+(5,2),8),-(*(2,5),7)),*(2,5))",
                    Logical = ">=(+(+(9,a),+(5,5)),*(8,+(+(5,5),+(7,1))))",
                    Index = 0
                },
                new QTerm
                {
                    Definitive = "+(/(+(+(5,N),8),-(*(N,5),7)),*(N,5))",
                    Logical = ">=(+(+(9,a),+(5,5)),*(8,+(+(5,5),+(7,1))))",
                    Index = 1
                }*/
          //  };
        }
    }
}
