using System.Collections.Generic;
using Core;

namespace Converters.FlowchartsConverters
{
    public interface IFlowchartsConverter:IConverter
    {
        List<Block> GetBlocks();
        List<Link> GetLinks();
        void SetBlocks(List<Block> blocks);
        void SetLinks(List<Link> links);
    }
}
