using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CustomRegex
{
    public class RegexLoader
    {
        private XElement m_main;

        public RegexLoader()
        {
        }

        public RegexLoader(string fileName)
        {
            loadFile(fileName);
        }

        private void loadFile(string fileName)
        {
            m_main = XElement.Load(@"regex/" + fileName);
        }

        public CustomRegexInfo load(string fileName, string codeName)
        {
            loadFile(fileName);
            return load(codeName);
        }

        internal List<string> loadRemoveUntils(XElement part)
        {
            return part
                        .Descendants("removeuntil")
                        .Select(e => (string)e)
                        .ToList();
        }

        internal List<string> loadRemoveFroms(XElement part)
        {
            return part
                        .Descendants("removefrom")
                        .Select(e => (string)e)
                        .ToList();
        }

        internal List<Tuple<string, string>> loadReplacementRegexes(XElement part)
        {
            return part
                        .Descendants("replace")
                        .Select(e => new Tuple<string, string>((string)e.Element("in"), (string)e.Element("out")))
                        .ToList();
        }

        internal Dictionary<string, string> loadCodeToExp(XElement part)
        {
            return 
                part
                    .Descendants("var")
                    .ToDictionary(e => (string)e.Attribute("name"),
                                  e => (string)e);
        }

        internal string loadMatchingRegex(XElement part)
        {
            Dictionary<string, string> codeToExp = loadCodeToExp(part);
            return string.Join("",
                    ((string)part.Element("main"))
                        .Split()
                        .Select(s => codeToExp.ContainsKey(s) ? codeToExp[s] : s));
        }

        public CustomRegexInfo load(string codeName)
        {
            try
            {
                CustomRegexInfo customRegex = new CustomRegexInfo();
                XElement part = 
                    m_main.Descendants("part")
                        .Where ( e => (string)e.Attribute("name") == codeName )
                        .First();

                customRegex.RemoveUntils = loadRemoveUntils(part);
                customRegex.RemoveFroms = loadRemoveFroms(part);
                customRegex.RemplacementRegexes = loadReplacementRegexes(part);
                customRegex.MatchingRegex = loadMatchingRegex(part);

                return customRegex;
            }
            catch (Exception e)
            {
                throw new Exception( "Regex loading error: " + e.Message, e );
            }
        }
    }
}
