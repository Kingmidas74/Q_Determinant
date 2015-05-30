using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Atoms;

namespace Core.Interfaces
{
    interface IDeterminant:IStatus
    {
        List<QTerm> QDeterminant { get; }
        List<QTerm> GetDeterminant(Graph syntaxTree);
    }
}
