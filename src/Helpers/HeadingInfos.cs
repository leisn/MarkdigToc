using Markdig.Renderers;

using System.Diagnostics;

namespace Leisn.MarkdigToc.Helpers
{
    [DebuggerDisplay("Level: {Level} Content: {Content}")]
    internal class HeadingInfos : LevelList<HeadingInfos>
    {
        public static HeadingInfos FromHeading(HeadingInfo info)
           => new HeadingInfos
           {
               Content = info.Content,
               Id = info.Id,
               Level = info.Level
           };

        public string Content { get; set; } = "";
        public string Id { get; set; } = "";

        public void RenderHtml(HtmlRenderer renderer, TocOptions options)
        {
            if (options.IsUlOnlyContainLi)
                renderHtmlLint(renderer, options);
            else
                renderHtmlMixed(renderer, options);
        }

        void renderHtmlMixed(HtmlRenderer renderer, TocOptions options)
        {//let children render itself
            if (!IsLocator)
            {
                renderer.Write("<li");
                if (options.liClass is not null)
                    renderer.Write($" class='{options.liClass}'");
                renderer.Write($"><a href='#{Id}'");
                if (options.aClass is not null)
                    renderer.Write($" class='{options.aClass}'");
                renderer.Write($">{Content}</a></li>");
            }

            if (Count < 1) return;

            renderer.Write("<ul");
            if (options.ulClass is not null)
                renderer.Write($" class='{options.ulClass}'");
            renderer.Write(">");
            foreach (var item in Children)
            {
                item.renderHtmlMixed(renderer, options);
            }
            renderer.Write($"</ul>");
        }

        void renderHtmlLint(HtmlRenderer renderer, TocOptions options)
        {//render children
            if (Count < 1) return;

            renderer.Write("<ul");
            if (options.ulClass is not null)
                renderer.Write($" class='{options.ulClass}'");
            renderer.Write(">");

            foreach (var item in Children)
            {
                renderer.Write("<li");
                if (options.liClass is not null)
                    renderer.Write($" class='{options.liClass}'");
                renderer.Write(">");

                if (!item.IsLocator)
                {
                    renderer.Write($"<a href='#{item.Id}'");
                    if (options.aClass is not null)
                        renderer.Write($" class='{options.aClass}'");
                    renderer.Write($">{item.Content}</a>");
                }

                item.renderHtmlLint(renderer, options);

                renderer.Write($"</li>");
            }
            renderer.Write($"</ul>");

        }
    }
}
