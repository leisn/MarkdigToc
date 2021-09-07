using Microsoft.VisualStudio.TestTools.UnitTesting;

using Leisn.MarkdigToc.Helpers;
using System.Diagnostics;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.IO;
using Markdig;
using Leisn.MarkdigToc;

namespace Tests
{
    [TestClass]
    public class OptionsTest
    {
        HtmlDocument doc;
        string[] files;

        public OptionsTest()
        {
            doc = new HtmlDocument();

            var dir = Directory.GetCurrentDirectory();
            var index = dir.IndexOf("bin");
            dir = dir[..index] + "TestFiles";

            files = Directory.GetFiles(dir, "*.md");
        }


        [TestMethod]
        public void IsUlOnlyContainLi_True()
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(opt => opt.IsUlOnlyContainLi = true)
                .Build();

            var tocOpt = pipeline.Extensions.Find<TocExtension>().Options;

            foreach (var item in files)
            {
                var content = File.ReadAllText(item);
                var result = Markdown.ToHtml(content, pipeline);
                doc.LoadHtml(result);
                var uls = doc.DocumentNode.SelectNodes($"//{tocOpt.TocTag}//ul");
                // only directly contain 'li', 'script' or 'template'
                var tags = new List<string> { "li", "script", "template" };
                uls.ForEach<HtmlNodeCollection, HtmlNode>((ul) =>
                {
                    Assert.IsNull(ul.SelectNodes("./ul"));
                    foreach (var n in ul.ChildNodes)
                        Assert.IsTrue(tags.Contains(n.Name.ToLower()));
                });
            }
        }

        [TestMethod]
        public void ContainerTest()
        {
            var containerTag = "section";
            var containerTagId = "toc-wrapper";
            var containerTagClass = "toc-container toc-wrapper";
            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(opt =>
                {
                    opt.ContainerTag = containerTag;
                    opt.ContainerId = containerTagId;
                    opt.ContainerClass = containerTagClass;
                }).Build();
            var tocOpt = pipeline.Extensions.Find<TocExtension>().Options;

            foreach (var item in files)
            {
                var content = File.ReadAllText(item);
                var result = Markdown.ToHtml(content, pipeline);
                doc.LoadHtml(result);
                var container = doc.DocumentNode.SelectSingleNode($"/{containerTag}[@id='{containerTagId}']");
                Assert.IsNotNull(container);
                Assert.AreEqual(containerTagClass, container.Attributes["class"].Value);
                Assert.IsNotNull(container.SelectSingleNode($"./{tocOpt.TocTag}"));
            }
        }

        [TestMethod]
        public void TitleAsConainerHeader_True()
        {
            var pipeline = new MarkdownPipelineBuilder()
               .UseTableOfContent(opt =>
               {
                   opt.ContainerTag = "TocContainer";
                   opt.TitleAsConainerHeader = true;
                   opt.TitleTag = "TocTitle";
                   opt.TocTag = "TOC";
               })
               .UseGenericAttributes()
               .Build();

            foreach (var item in files)
            {
                if (!item.Contains("with-title"))
                    continue;
                var result = Markdown.ToHtml(File.ReadAllText(item), pipeline);
                doc.LoadHtml(result);

                var container = doc.DocumentNode.SelectSingleNode($"/TocContainer".ToLower());
                Assert.IsNotNull(container);
                Assert.IsNotNull(container.SelectSingleNode($"./TocTitle".ToLower()));
                var toc = container.SelectSingleNode($"./TOC".ToLower());
                Assert.IsNotNull(toc);
                Assert.IsNull(toc.SelectSingleNode($"./TocTitle".ToLower()));
            }
        }
        [TestMethod]
        public void TitleAsConainerHeader_False()
        {
            var pipeline = new MarkdownPipelineBuilder()
               .UseTableOfContent(opt =>
               {
                   opt.ContainerTag = "TocContainer";
                   opt.TitleAsConainerHeader = false;
                   opt.TitleTag = "TocTitle";
                   opt.TocTag = "TOC";
               })
               .UseGenericAttributes()
               .Build();

            foreach (var item in files)
            {
                if (!item.Contains("with-title"))
                    continue;
                var result = Markdown.ToHtml(File.ReadAllText(item), pipeline);
                doc.LoadHtml(result);

                var container = doc.DocumentNode.SelectSingleNode($"/TocContainer".ToLower());
                Assert.IsNotNull(container);
                Assert.IsNull(container.SelectSingleNode($"./TocTitle".ToLower()));
                var toc = container.SelectSingleNode($"./TOC".ToLower());
                Assert.IsNotNull(toc);
                Assert.IsNotNull(toc.SelectSingleNode($"./TocTitle".ToLower()));
            }
        }

        [TestMethod]
        public void TocTagTest()
        {
            var tocTag = "toc";
            var tocId = "tocid-spec";
            var tocClass = "toc1 toc2";

            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(opt =>
                {
                    opt.TocTag = tocTag;
                    opt.TocId = tocId;
                    opt.TocClass = tocClass;
                }).Build();

            foreach (var item in files)
            {
                var content = File.ReadAllText(item);
                var result = Markdown.ToHtml(content, pipeline);
                doc.LoadHtml(result);
                var toc = doc.DocumentNode.SelectSingleNode($"/{tocTag}[@id='{tocId}']");
                Assert.IsNotNull(toc);
                Assert.AreEqual(tocClass, toc.Attributes["class"].Value);
            }
        }

        [TestMethod]
        public void TocTitleTest()
        {
            var tocTag = "toc";
            var titleTag = "ttitle";
            var titleId = "tocid-spec";
            var titleClass = "toc1 toc2";
            var overrideTitle = "目录";
            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(opt =>
                {
                    opt.TocTag = tocTag;
                    opt.TitleTag = titleTag;
                    opt.TitleId = titleId;
                    opt.TitleClass = titleClass;
                    opt.OverrideTitle = overrideTitle;
                }).Build();

            foreach (var item in files)
            {
                var content = File.ReadAllText(item);
                var result = Markdown.ToHtml(content, pipeline);
                doc.LoadHtml(result);
                var title = doc.DocumentNode.SelectSingleNode($"/{tocTag}/{titleTag}[@id='{titleId}']");
                Assert.IsNotNull(title);
                Assert.AreEqual(titleClass, title.Attributes["class"].Value);
                Assert.AreEqual(overrideTitle, title.InnerText);
            }
        }

        [TestMethod]
        public void TocItemsTest()
        {
            var tocTag = "toc";
            var ulClass = "ulClass ul";
            var liClass = "liClass li";
            var aClass = "aClass a";
            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(opt =>
                {
                    opt.TocTag = tocTag;
                    opt.ulClass = ulClass;
                    opt.liClass = liClass;
                    opt.aClass = aClass;
                }).Build();

            foreach (var item in files)
            {
                var content = File.ReadAllText(item);
                var result = Markdown.ToHtml(content, pipeline);
                doc.LoadHtml(result);
                var toc = doc.DocumentNode.SelectSingleNode($"/{tocTag}");
                Assert.IsNotNull(toc);
                toc.SelectNodes(".//ul")
                    .ForEach<HtmlNodeCollection, HtmlNode>((ul) =>
                {
                    Assert.AreEqual(ulClass, ul.Attributes["class"].Value);
                });
                toc.SelectNodes(".//li")
                    .ForEach<HtmlNodeCollection, HtmlNode>((li) =>
                    {
                        Assert.AreEqual(liClass, li.Attributes["class"].Value);
                    });
                toc.SelectNodes(".//li//a")
                    .ForEach<HtmlNodeCollection, HtmlNode>((a) =>
                    {
                        Assert.AreEqual(aClass, a.Attributes["class"].Value);
                    });
            }
        }

        [TestMethod]
        public void HeadingIdGeneratorest()
        {
            var tocTag = "toc";
            var pipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent(
                tocAction: opt => opt.TocTag = tocTag,
                idAction: opt =>
                {
                    opt.HeadingIdGenerator = (level, content, id) =>
                    {
                        if (id != null)
                            return id;
                        else return level + "-" + content;
                    };
                }).Build();
            var content = @"
[TOC]
# t1
## t2
### t3
## t4
# t5
## t6
# t7
";
            var result = Markdown.ToHtml(content, pipeline);
            doc.LoadHtml(result);
            var aTags = doc.DocumentNode.SelectNodes($"/{tocTag}//li/a");
            aTags.ForEach<HtmlNodeCollection, HtmlNode>((a) =>
            {
                var href = a.Attributes["href"]?.Value;
                Assert.IsNotNull(href);
                Assert.IsTrue(href.StartsWith('#'));
                var id = href[1..];
                var level = id[0..1];
                Assert.IsNotNull(doc.DocumentNode.SelectSingleNode($"//h{level}[@id='{id}']"));
            });
        }
    }
}
