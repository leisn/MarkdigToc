
using Leisn.MarkdigToc.Helpers;

namespace Leisn.MarkdigToc
{
    internal struct HeadingInfo
    {
        public string Content { get; }
        public string Id { get; }
        public int Level { get; }

        public HeadingInfo(int level, string id, string content)
        {
            Content = content;
            Id = id;
            Level = level;
        }
    }

    /// <summary>
    /// Toc options for <see cref="TocExtension"/>
    /// </summary>
    public class TocOptions
    {
        internal HeadingInfos Headings { get; }

        public TocOptions()
        {
            Headings = new HeadingInfos() { IsLocator = true, Level = -1 };
        }

        /// <summary>
        /// According to webhint 'ul' and 'ol' must only directly contain 'li', 'script' or 'template' elements.<br/>
        /// <em>Default true</em>, set <strong>false</strong> to mix ul and li like others do.<br/>
        /// For more information see readme.<br/>
        /// seealso webhint: <a href="https://webhint.io/docs/user-guide/hints/hint-axe/structure/?source=devtools"/>
        /// </summary>
        public bool IsUlOnlyContainLi { get; set; } = true;

        #region toc container
        /// <summary>
        /// Put the tile in ContainerTag not inside the TocTag <em>(default false)</em>.<br/>
        /// Notice: working only <see cref="ContainerTag"/> is<strong> not null</strong>.<br/>
        /// </summary>
        public bool TitleAsConainerHeader { get; set; } = false;
        /// <summary>
        /// If this is not null, the toc will put in a element use ContainerTag <em>(default null)</em>.
        /// </summary>
        public string? ContainerTag { get; set; }
        /// <summary>
        /// Id for ContainerTag, work on ContainerTag is <strong> not null</strong>.
        /// </summary>
        public string? ContainerId { get; set; }
        /// <summary>
        /// Classes for ContainerTag, work on ContainerTag is <strong> not null</strong>. 
        /// </summary>
        public string? ContainerClass { get; set; }
        #endregion
        #region toc tag
        /// <summary>
        /// Tag name for toc element <em>(default nav)</em>.
        /// </summary>
        public string TocTag { get; set; } = "nav";
        /// <summary>
        /// Class names for toc element.<br/>
        /// Notice: If also defined in markdown document , this will be auto add without repeating.<br/>
        /// e.g.<br/>
        /// <code>[toc] {.tocClass1.tocClass2}</code>
        /// <code>TocClass="toc tocClass1";</code>
        /// final:<br/>
        /// <code>tocClass1 tocClass2 toc</code>
        /// </summary>
        public string? TocClass { get; set; }
        /// <summary>
        /// Id for toc element.<br/>
        /// Notice: If defined in markdown document , this will be <strong>ignored</strong>.<br/>
        /// e.g.<br/>
        /// <code>[toc] {#tocId}</code>
        /// <code>TocId="tocId";</code>
        /// </summary>
        public string? TocId { get; set; }
        #endregion
        #region toc title
        /// <summary>
        /// Force the title with this value.
        /// </summary>
        public string? OverrideTitle { get; set; }
        /// <summary>
        /// Title tag <em>(default p)</em>.<br/>
        /// e.g.<br/>
        /// <code>[toc] toc title {#tocid}</code>
        /// <code>TitleTag="summary"</code>
        /// For 'toc title' use tag 'summary'.
        /// </summary>
        public string TitleTag { get; set; } = "p";
        /// <summary>
        /// Id for TitleTag element.
        /// </summary>
        public string? TitleId { get; set; }
        /// <summary>
        /// Classes for TitleTag element.
        /// </summary>
        public string? TitleClass { get; set; }
        #endregion
        #region toc items
        /// <summary>
        /// Class names for <strong>ul</strong> emement.<br/>
        /// e.g.<br/>
        /// <code>ulClass="toc-ul toc-list";</code>
        /// </summary>
        public string? ulClass { get; set; }
        /// <summary>
        /// Class names for <strong>li</strong> emement.<br/>
        /// e.g.<br/>
        /// <code>liClass="toc-li toc-item";</code>
        /// </summary>
        public string? liClass { get; set; }
        /// <summary>
        /// Class names for <strong>a</strong> emement  inside li.<br/>
        /// e.g.<br/>
        /// <code>aClass="toc-a toc-item-link";</code>
        /// </summary>
        public string? aClass { get; set; }
        #endregion

        internal void AddHeading(HeadingInfo info)
            => Headings.Append(HeadingInfos.FromHeading(info));

    }
}
