using System.Collections.Generic;

namespace Core.Atoms
{
    public class QTerm
    {
        public string Logical { get; set; }
        public string Definitive { get; set; }
        public ulong Index { get; set; }

        public List<string> LogicalChain { get; set; }
        public List<string> DefinitiveChain { get; set; }


    }
}
