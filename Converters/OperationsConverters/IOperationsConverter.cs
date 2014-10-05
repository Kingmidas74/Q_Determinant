using System.Collections.Generic;
using Core;

namespace Converters.OperationsConverters
{
    public interface IOperationsConverter:IConverter
    {
        List<Operation> GetBlocks();
        List<Link> GetLinks();
        void SetBlocks(List<Operation> blocks);
        void SetLinks(List<Link> links);
    }
}
