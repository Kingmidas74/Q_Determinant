using System.Collections.Generic;

namespace QDeterminant.InternalClasses
{
    internal class LinearPart
    {
        internal string dU { get; set; }
        internal string dW { get; set; }
        internal List<string> Morf { get; set; }

        internal LinearPart()
        {
            dU = string.Empty;
            dW = string.Empty;
            Morf = new List<string>();
        }
    }
}
