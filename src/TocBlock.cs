using Markdig.Parsers;
using Markdig.Syntax;

namespace Leisn.MarkdigToc
{
    public class TocBlock : HeadingBlock
    {
        public TocBlock(BlockParser parser) : base(parser)
        {
            ProcessInlines = true;
        }
    }
}
