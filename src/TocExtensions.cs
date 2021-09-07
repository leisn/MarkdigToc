using Leisn.MarkdigToc;

using Markdig.Extensions.AutoIdentifiers;
using Markdig.Extensions.GenericAttributes;

using System;

namespace Markdig
{
    public static class TocExtensions
    {
        /// <summary>
        /// Uses this extension to enable Table Of Content, by match '[TOC]'.<br/>
        /// <strong>Already auto</strong> insert before GenericAttributesExtension and replace AutoIdentifierExtension to CustomAutoIdExtension.<br/>
        /// <strong>Notice:</strong> Must after CustomAutoIdExtension,
        /// and before GenericAttributesExtension if use atttributes.
        /// <br/>
        /// </summary>
        public static MarkdownPipelineBuilder UseTableOfContent(
            this MarkdownPipelineBuilder pipeline,
            Action<TocOptions>? tocAction = null,
            Action<CustomAutoIdOptions>? idAction = null)
        {
            var options = new TocOptions();
            tocAction?.Invoke(options);

            var idoptions = new CustomAutoIdOptions();
            idAction?.Invoke(idoptions);

            pipeline.Extensions.ReplaceOrAdd<AutoIdentifierExtension>(new CustomAutoIdExtension(idoptions));

            var tocExtension = new TocExtension(options);
            if (pipeline.Extensions.Find<GenericAttributesExtension>() is not null)
                pipeline.Extensions.InsertBefore<GenericAttributesExtension>(tocExtension);
            else
                pipeline.Extensions.AddIfNotAlready(tocExtension);
            return pipeline;
        }
    }
}
