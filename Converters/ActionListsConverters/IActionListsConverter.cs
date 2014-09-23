using System.Collections.Generic;
using Core;

namespace Converters.ActionListsConverters
{
    public interface IActionListsConverter:IConverter
    {
        List<Block> GetBlocks();
        List<Link> GetLinks();
        void SetBlocks(List<Block> blocks);
        void SetLinks(List<Link> links);
    }
}
