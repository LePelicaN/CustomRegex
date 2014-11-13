using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRegex
{
    public struct CustomRegexInfo
    {
        public List<string> RemoveUntils;
        public List<string> RemoveFroms;
        public List<Tuple<string, string>> RemplacementRegexes;
        public string MatchingRegex;
    }
}
