using System;
using System.Collections.Generic;

namespace CustomRegex
{
    public struct CustomRegexInfo
    {
        public IEnumerable<string> RemoveUntils;
        public IEnumerable<string> RemoveFroms;
        public IEnumerable<Tuple<string, string>> RemplacementRegexes;
        public string MatchingRegex;
    }
}
