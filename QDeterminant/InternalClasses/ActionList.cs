using System.Collections.Generic;

namespace QDeterminant.InternalClasses
{
    internal class ActionList
    {
        internal LinearPart Header { get; set; }
        internal List<ActionList> Body { get; set; }
        internal ActionList Tail { get; set; }

        internal ActionList()
        {
            Header = new LinearPart();
            Body = new List<ActionList>();
        }
    }
}
