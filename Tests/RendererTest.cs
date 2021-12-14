using Markdig;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    [TestClass]
    public class RendererTest
    {
        //HtmlDocument doc;
        //doc.LoadHtml();
        List<string> files;
        MarkdownPipeline lintPipeline;
        MarkdownPipeline noLintPipelint;

        public RendererTest()
        {

            lintPipeline = new MarkdownPipelineBuilder()
                .UseTableOfContent()
                .UseGenericAttributes()
                .Build();
            noLintPipelint = new MarkdownPipelineBuilder()
                            .UseTableOfContent(opt => opt.IsUlOnlyContainLi = false)
                            .UseGenericAttributes()
                            .Build();

            var dir = Directory.GetCurrentDirectory();
            var index = dir.IndexOf("bin");
            dir = dir[..index] + "TestFiles";

            files = new List<string>();
            var mdFiles = Directory.GetFiles(dir, "*.md");
            foreach (var item in mdFiles)
                files.Add(item[..item.LastIndexOf('.')]);
        }

        [TestMethod]
        public void MdFilesTest()
        {
            foreach (var item in files)
            {
                var md = File.ReadAllText(item + ".md");
                var lintActual = Markdown.ToHtml(md, lintPipeline);
                var lintExpected = File.ReadAllText(item + "-lint.html");
                Assert.AreEqual(lintExpected, lintActual, $"Failed lint :{item}");

                var noLintActual = Markdown.ToHtml(md, noLintPipelint);
                var noLintExpected = File.ReadAllText(item + "-nolint.html");
                Assert.AreEqual(noLintExpected, noLintActual, $"Failed nolint :{item}");
            }
        }

    }
}
