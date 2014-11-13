using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CustomRegex
{
    public class RegexRunner
    {
        static internal string RemoveUntils(CustomRegexInfo customRegex, string text)
        {
            foreach (string removeUntil in customRegex.RemoveUntils)
            {
                int index = text.IndexOf(removeUntil);
                text = text.Remove(0, index + removeUntil.Length);
            }
            return text;
        }

        static internal string RemoveFroms(CustomRegexInfo customRegex, string text)
        {
            foreach (string removeFrom in customRegex.RemoveFroms)
            {
                int index = text.IndexOf(removeFrom);
                text = text.Remove(index);
            }
            return text;
        }

        static internal string ReplaceRegexes(CustomRegexInfo customRegex, string text)
        {
            foreach (Tuple<string, string> remplacement in customRegex.RemplacementRegexes)
            {
                text = Regex.Replace(text, remplacement.Item1, remplacement.Item2);
            }
            return text;
        }

        static internal List<List<string>> Matching(CustomRegexInfo customRegex, string text)
        {
            var matches = Regex.Matches(text, customRegex.MatchingRegex);
            return matches.Cast<Match>()
                        .Take(Math.Max(10, matches.Count))
                        .Select(m => m
                            .Groups.Cast<Group>()
                            .Select(g => g.Value)
                            .ToList<string>())
                        .ToList<List<string>>();
        }

        static public List<List<string>> run(CustomRegexInfo customRegex, string text)
        {
            try
            {
                text = RemoveUntils(customRegex, text);
                text = RemoveFroms(customRegex, text);
                text = ReplaceRegexes(customRegex, text);
                return Matching(customRegex, text);
            }
            catch(Exception e)
            {
                throw new Exception( "Regex running error: " + e.Message, e );
            }
        }
    }
}
