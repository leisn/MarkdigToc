using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;

using System;

namespace Leisn.MarkdigToc
{
    /// <summary>
    /// Table of content extension
    /// </summary>
    public class TocExtension : IMarkdownExtension
    {
        /// <summary>
        /// Options of TocExtension
        /// </summary>
        public TocOptions Options { get; }

        /// <summary>
        /// Construct a new instance.
        /// </summary>
        /// <param name="options">Options of TocExtension</param>
        public TocExtension(TocOptions options)
        {
            Options = options ?? new TocOptions();
        }

        //register parsers
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            var cidE = pipeline.Extensions.Find<CustomAutoIdExtension>();
            if (cidE == null)
                throw new InvalidOperationException("CustomAutoIdExtension is null");
            cidE.OnHeadingParsed += (heading) => Options.AddHeading(heading);
            pipeline.BlockParsers.AddIfNotAlready(new TocBlockParser(Options));
        }

        //register randerers
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                //because TocBlock:HeadingBlock ,so renderer must before HeadingRenderer
                htmlRenderer.ObjectRenderers.InsertBefore<HeadingRenderer>(new HtmlTocRenderer(Options));
            }
        }

    }
}
