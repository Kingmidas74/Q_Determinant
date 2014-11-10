using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationPlan.ActionListTemp
{
    public class ActionList
    {
        public Element Header { get; set; }
        public List<ActionList> Body = new List<ActionList>();
        public List<ActionList> Tile = new List<ActionList>();
    }
}
