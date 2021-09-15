using Markdig.Renderers;
using Markdig.Renderers.Html;

using System;

namespace Leisn.MarkdigToc
{
    public class HtmlTocRenderer : HtmlObjectRenderer<TocBlock>
    {
        public TocOptions Options { get; }

        public HtmlTocRenderer(TocOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));
        }

        protected override void Write(HtmlRenderer renderer, TocBlock obj)
        {
            if (Options.Headings.Count < 1)
                return;
            renderer.EnsureLine();

            var attr = obj.GetAttributes();
            if (attr.Id is null)
                attr.Id = Options.TocId;
            if (Options.TocClass is not null)
            {
                var classes = Options.TocClass.Split(' ');
                foreach (var item in classes)
                {
                    var c = item?.Trim();
                    if (!string.IsNullOrEmpty(c))
                        attr.AddClass(c!);
                }
            }

            if (renderer.EnableHtmlForBlock)
            {
                if (Options.ContainerTag is not null)
                {
                    renderer.Write($"<{Options.ContainerTag}");
                    if (Options.ContainerId is not null)
                        renderer.Write($" id='{Options.ContainerId}'");
                    if (Options.ContainerClass is not null)
                        renderer.Write($" class='{Options.ContainerClass}'");
                    renderer.Write(">");

                    if (Options.TitleAsConainerHeader)
                        WriteTitle(renderer, obj);
                }

                renderer.Write($"<{Options.TocTag}")
                    .WriteAttributes(obj)
                    .Write($">");
            }

            if (Options.ContainerTag is null || !Options.TitleAsConainerHeader)
                WriteTitle(renderer, obj);

            if (renderer.EnableHtmlForBlock)
            {
                Options.Headings.RenderHtml(renderer, Options);
                renderer.Write($"</{Options.TocTag}>");
                if (Options.ContainerTag is not null)
                    renderer.Write($"</{Options.ContainerTag}>");
            }
            renderer.EnsureLine();
        }

        void WriteTitle(HtmlRenderer renderer, TocBlock obj)
        {
            //no title 
            if (obj.Inline?.FirstChild == null && Options.OverrideTitle == null)
                return;
            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write($"<{Options.TitleTag}");
                if (Options.TitleId is not null)
                    renderer.Write($" id='{Options.TitleId}'");
                if (Options.TitleClass is not null)
                    renderer.Write($" class='{Options.TitleClass}'");
                renderer.Write(">");
            }

            if (Options.OverrideTitle is not null)
                renderer.Write(Options.OverrideTitle);
            else
                renderer.WriteLeafInline(obj);

            if (renderer.EnableHtmlForBlock)
            {
                renderer.Write($"</{Options.TitleTag}>");
            }
        }
    }
}
