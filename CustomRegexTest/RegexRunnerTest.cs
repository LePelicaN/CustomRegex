using CustomRegex;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CustomRegexTest
{
    [TestClass]
    public class RegexRunnerTest
    {
        [TestMethod]
        public void TestRemoveUntil()
        {
            CustomRegexInfo customRegex = new CustomRegexInfo();
            customRegex.RemoveUntils = new List<string>{ "test1" };
            Assert.AreEqual("test2test1", RegexRunner.RemoveUntils(customRegex, "abcd test1test2test1"));
        }

        [TestMethod]
        public void TestRemoveFrom()
        {
            CustomRegexInfo customRegex = new CustomRegexInfo();
            customRegex.RemoveFroms = new List<string>{ "test1" };
            Assert.AreEqual("abcd ", RegexRunner.RemoveFroms(customRegex, "abcd test1test2test1"));
        }

        [TestMethod]
        public void TestReplacementRegex()
        {
            CustomRegexInfo customRegex = new CustomRegexInfo();
            customRegex.RemplacementRegexes = new List<Tuple<string, string>>{ Tuple.Create("t.*?t1", "test2") };
            Assert.AreEqual("abcd test2test2", RegexRunner.ReplaceRegexes(customRegex, "abcd teeesssst1test3test1"));
        }

        [TestMethod]
        public void TestMatching()
        {
            CustomRegexInfo customRegex = new CustomRegexInfo();
            customRegex.MatchingRegex = "(t.*?t1)test3(t.*?t2)";
            Assert.AreEqual("teeesssst1test3test2,teeesssst1,test2|test1test3tttttt2,test1,tttttt2",
                            String.Join("|", RegexRunner.Matching(customRegex,
                                                 "abcd teeesssst1test3test2 abcd test1test3tttttt2").Select(l => String.Join(",", l))));
        }
    }
}
