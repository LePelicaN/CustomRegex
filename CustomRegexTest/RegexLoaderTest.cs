using CustomRegex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CustomRegexTest
{
    [TestClass]
    public class RegexLoaderTest
    {
        private delegate IEnumerable<string> RemoveLoaderDelegate(XElement part);

        [TestMethod]
        public void LoadRemoveUntil()
        {
            string tag = "removeuntil";
            RegexLoader loader = new RegexLoader();
            RemoveLoaderDelegate loadMethod = new RemoveLoaderDelegate(loader.loadRemoveUntils);
            LoadRemove(tag, loadMethod);
        }

        [TestMethod]
        public void LoadRemoveFrom()
        {
            string tag = "removefrom";
            RegexLoader loader = new RegexLoader();
            RemoveLoaderDelegate loadMethod = new RemoveLoaderDelegate(loader.loadRemoveFroms);
            LoadRemove(tag, loadMethod);
        }

        private static void LoadRemove(string tag, RemoveLoaderDelegate loadMethod)
        {
            string removeText = "here1";
            XElement test =
                new XElement("part",
                    new XElement("removes",
                        new XElement(tag, removeText)));
            IEnumerable<string> removes = loadMethod(test);
            Assert.AreEqual(1, removes.Count());
            Assert.IsTrue(removes.SequenceEqual(new List<string>{ removeText }));
        }

        [TestMethod]
        public void LoadRemplacementRegex()
        {
            string replaceTextIn = "here1";
            string replaceTextOut = "here2";
            XElement test =
                new XElement("part",
                    new XElement("replacements",
                    new XElement("replace",
                        new XElement("in", replaceTextIn),
                        new XElement("out", replaceTextOut))));
            RegexLoader loader = new RegexLoader();
            IEnumerable<Tuple<string, string>> replacement = loader.loadReplacementRegexes(test);
            Assert.AreEqual(1, replacement.Count());
            Assert.IsTrue(replacement.SequenceEqual( new List<Tuple<string, string>>{ Tuple.Create( replaceTextIn, replaceTextOut ) } ) );
        }

        [TestMethod]
        public void LoadCodeToExp()
        {
            string code = "code";
            string expression = "expression";
            XElement test =
                new XElement("part",
                    new XElement("vars",
                        new XElement("var",
                            expression,
                            new XAttribute("name", code))));
            RegexLoader loader = new RegexLoader();
            Dictionary<string, string> codeToExpression = loader.loadCodeToExp(test);
            Assert.IsTrue(codeToExpression.ContainsKey(code));
            Assert.AreEqual(expression, codeToExpression[code]);
        }

        [TestMethod]
        public void LoadMatchingRegex()
        {
            string var1Code = "@@var1";
            string var1Exp = "here1";
            string main = var1Code +" here2";
            XElement test =
                new XElement("part",
                    new XElement("vars",
                        new XElement("var",
                            var1Exp,
                            new XAttribute("name", var1Code))),
                    new XElement("main", main));
            RegexLoader loader = new RegexLoader();
            string regex = loader.loadMatchingRegex(test);
            Assert.AreEqual(var1Exp +"here2", regex);
        }
    }
}
