using Core;
using System.Collections.Generic;
namespace ActionList
{
    public class AList: IAList
    {
        public QDet AL;
        public AList(List<Block> blocks, List<Link> Links, List<Operation> Oper)
        {
            AL = new QDet();
            var x = new QTerm();
            var y = new QTerm();
            var z = new QTerm();
            x.Logical = "dx>=5*a+2*(b-1)";
            x.Definitive = "Sin((8+2*5)/(1+3*2-4))";
            AL.QDeterminant.Add(x);
            y.Definitive = "3*x+1";
            y.Logical = "5>1+1+1+1";
            AL.QDeterminant.Add(y);
        }

        public QDet getqdet()   
        {
            return AL;
        }
    }

}
