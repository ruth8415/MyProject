using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace lesson2_ruti_weisbeker.Parsing;

public static class HtmlSplitter
{
    private static readonly Regex TagRe =
        new(@"(<\/?[a-zA-Z][a-zA-Z0-9:-]*[^>]*>)", RegexOptions.Singleline | RegexOptions.Compiled);

    public static List<string> SplitToParts(string html)
    {
        var parts = new List<string>();
        if (string.IsNullOrWhiteSpace(html)) return parts;

        foreach (var chunk in TagRe.Split(html))
        {
            if (string.IsNullOrEmpty(chunk)) continue;
            var s = chunk.Trim();
            if (string.IsNullOrEmpty(s)) continue;

            if (s.StartsWith("<"))
            {
                parts.Add(s);
            }
            else
            {
                var compact = Regex.Replace(s, @"\s+", " ");
                if (!string.IsNullOrWhiteSpace(compact)) parts.Add(compact);
            }
        }
        return parts;
    }
}