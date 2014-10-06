using System.Collections.Generic;
using Core;

namespace Converters.ImplementationPlanConverters
{
    public interface IImplementationPlanConverter:IConverter
    {
        List<Block> GetBlocks();
        List<Link> GetLinks();
        void SetBlocks(List<Block> blocks);
        void SetLinks(List<Link> links);
    }
}
