using Microsoft.VisualStudio.TestTools.UnitTesting;

using Leisn.MarkdigToc.Helpers;
using System.Diagnostics;
using System.Text;
using Markdig.Helpers;

namespace Tests
{
    [TestClass]
    public class LevelListTest
    {
        [DebuggerDisplay("Level: {Level} Content: {Content}")]
        private class L : LevelList<L>
        {
            public string Content { get; set; }

            public string ToFormatedString(string prefix = "", StringBuilder sb = null)
            {
                if (sb == null)
                    sb = new StringBuilder();
                if (!IsLocator)
                {
                    sb.Append(prefix);
                    sb.Append(Content);
                    sb.AppendLine();
                }
                prefix += "  ";
                foreach (var item in Children)
                {
                    item.ToFormatedString(prefix, sb);
                }
                return sb.ToString();
            }
        }

        readonly L root;

        public LevelListTest()
        {
            root = new L { IsLocator = true, Content = "Root" };
        }

        [TestMethod]
        public void NormalListTest()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);

            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 2, Content = "## t1.1" });
            root.Append(new L { Level = 2, Content = "## t1.2" });
            root.Append(new L { Level = 3, Content = "### t1.2.1" });
            root.Append(new L { Level = 2, Content = "## t1.3" });
            root.Append(new L { Level = 1, Content = "# t2" });
            root.Append(new L { Level = 2, Content = "## t2.1" });
            root.Append(new L { Level = 1, Content = "# t3" });
            root.Append(new L { Level = 1, Content = "# t4" });

            Assert.IsTrue(root.Count == 4);
            Assert.IsTrue(root[0].Count == 3);
            Assert.IsTrue(root[0][0].Count == 0);
            Assert.IsTrue(root[0][1].Count == 1);
            Assert.IsTrue(root[0].Content == "# t1");
            Assert.IsTrue(root[0][0].Content == "## t1.1");
            Assert.IsTrue(root[0][1].Content == "## t1.2");
            Assert.IsTrue(root[0][1][0].Content == "### t1.2.1");
            Assert.IsTrue(root[0][2].Content == "## t1.3");
            Assert.IsTrue(root[1].Content == "# t2");
            Assert.IsTrue(root[1][0].Content == "## t2.1");
            Assert.IsTrue(root[2].Content == "# t3");
            Assert.IsTrue(root[3].Content == "# t4");
        }

        [TestMethod]
        public void AutoExtendRootTest()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);

            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 2, Content = "## t1.1" });
            root.Append(new L { Level = 3, Content = "### t1.1.1" });
            root.Append(new L { Level = 1, Content = "# t2" });

            root.Append(new L { Level = 0, Content = "~ 0" });
            Assert.IsTrue(root.Count == 2);
            Assert.IsTrue(root[0].Count == 2);
            Assert.IsTrue(root[0].IsLocator);
            Assert.IsTrue(root[0][0][0][0].Content == "### t1.1.1");
            Assert.IsTrue(root.Level == -1);

            root.Append(new L { Level = -1, Content = "~~ -1" });
            Assert.IsTrue(root.Level == -2);
            Assert.IsTrue(root.Count == 2);
            Assert.IsTrue(root[0][0].Count == 2);
            Assert.IsTrue(root[0][0][0][0][0].Content == "### t1.1.1");
            Assert.IsTrue(root[0][0][1].Content == "# t2");
        }

        [TestMethod]
        public void EmtpyNodeTest()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);

            root.Append(new L { Level = 2, Content = "## t0.1" });
            root.Append(new L { Level = 2, Content = "## t0.2" });
            root.Append(new L { Level = 2, Content = "## t0.3" });
            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 1, Content = "# t2" });
            root.Append(new L { Level = 1, Content = "# t3" });
            Assert.IsTrue(root.Count == 4);
            Assert.IsTrue(root[0].IsLocator);
            Assert.IsTrue(root[0].Count == 3);
            Assert.IsTrue(root[0][2].Content == "## t0.3");
            root.Append(new L { Level = 3, Content = "## t1.1.1" });
        }

        [TestMethod]
        public void จั_Test()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);
            root.Append(new L { Level = 5, Content = "# t5" });
            root.Append(new L { Level = 4, Content = "# t4" });
            root.Append(new L { Level = 3, Content = "# t3" });
            root.Append(new L { Level = 2, Content = "# t2" });
            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 2, Content = "# t2" });
            root.Append(new L { Level = 3, Content = "# t3" });
            root.Append(new L { Level = 4, Content = "# t4" });
            root.Append(new L { Level = 5, Content = "# t5" });
            var result = root.ToFormatedString();
            var expected =
@"          # t5
        # t4
      # t3
    # t2
  # t1
    # t2
      # t3
        # t4
          # t5
";
            Assert.IsTrue(result == expected);

        }

        [TestMethod]
        public void ฅี_Test()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);

            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 2, Content = "# t2" });
            root.Append(new L { Level = 3, Content = "# t3" });
            root.Append(new L { Level = 4, Content = "# t4" });
            root.Append(new L { Level = 5, Content = "# t5" });
            root.Append(new L { Level = 5, Content = "# r5" });
            root.Append(new L { Level = 4, Content = "# r4" });
            root.Append(new L { Level = 3, Content = "# r3" });
            root.Append(new L { Level = 2, Content = "# r2" });
            root.Append(new L { Level = 1, Content = "# r1" });
            var result = root.ToFormatedString();
            var expected = 
@"  # t1
    # t2
      # t3
        # t4
          # t5
          # r5
        # r4
      # r3
    # r2
  # r1
";
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void X_Test()
        {
            root.Clear();
            Assert.IsTrue(root.Count == 0);

            root.Append(new L { Level = 1, Content = "# t1" });
            root.Append(new L { Level = 4, Content = "# t2" });
            root.Append(new L { Level = 2, Content = "# t3" });
            root.Append(new L { Level = 4, Content = "# t4" });
            root.Append(new L { Level = 1, Content = "# t5" });
            root.Append(new L { Level = 5, Content = "# t6" });
            var result = root.ToFormatedString();
            Debug.WriteLine(result);
            var expected =
@"  # t1
        # t2
    # t3
        # t4
  # t5
          # t6
";
            Assert.IsTrue(result == expected);
        }
    }
}
