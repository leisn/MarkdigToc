using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

using System;
using System.Linq;

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
            var autoIdExtension = pipeline.Extensions.Find<CustomAutoIdExtension>();
            if (autoIdExtension == null)
                throw new InvalidOperationException("CustomAutoIdExtension is null");
            autoIdExtension.OnHeadingParsed -= AutoIdExtension_OnHeadingParsed;
            autoIdExtension.OnHeadingParsed += AutoIdExtension_OnHeadingParsed;
            pipeline.BlockParsers.AddIfNotAlready(new TocBlockParser(Options));

            //clear headings after all rendered, not here
            //pipeline.DocumentProcessed -= Pipeline_DocumentProcessed;
            //pipeline.DocumentProcessed += Pipeline_DocumentProcessed;
        }

        //private void Pipeline_DocumentProcessed(MarkdownDocument document)
        //{
        //    //if there is no [toc] in markdown document, clear the headings.
        //    var tocBlocks = document.Where(block => block is TocBlock).ToList();
        //    if (tocBlocks == null || tocBlocks.Count < 1)
        //        Options.Headings.Clear();
        //}

        private void AutoIdExtension_OnHeadingParsed(HeadingInfo heading)
            => Options.AddHeading(heading);

        //register randerers, this method will execute after all parsers completed.
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer htmlRenderer)
            {
                //because TocBlock:HeadingBlock ,so renderer must before HeadingRenderer
                if (!htmlRenderer.ObjectRenderers.Contains<HtmlTocRenderer>())
                    htmlRenderer.ObjectRenderers.InsertBefore<HeadingRenderer>(new HtmlTocRenderer(Options));
            }
            
            renderer.ObjectWriteAfter += Renderer_ObjectWriteAfter;
        }

        private void Renderer_ObjectWriteAfter(IMarkdownRenderer arg1, MarkdownObject arg2)
        {
            //move HtmlTocRenderer -> Write -> 'Options.Headings.Clear()' to here
            //now it can be more than one [toc] in markdown document , and all of these can be rendered
            //though it's useless
            if (arg2 is MarkdownDocument)// when the document all writed
                Options.Headings.Clear();
        }
    }
}
