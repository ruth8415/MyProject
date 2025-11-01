using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using lesson2_ruti_weisbeker.Helpers;
using lesson2_ruti_weisbeker.Model;

namespace lesson2_ruti_weisbeker.Parsing;

public static class HtmlTreeBuilder
{
    private static readonly Regex TagMainRe =
        new(@"^<\s*(\/)?\s*([a-zA-Z][a-zA-Z0-9:-]*)\b([^>]*)>(\s*)$", RegexOptions.Singleline | RegexOptions.Compiled);

    private static readonly Regex AttrRe =
        new(@"([^\s=\/>]+)(?:\s*=\s*(?:""([^""]*)""|'([^']*)'|([^\s""'>/]+)))?", RegexOptions.Compiled);

    public static HtmlElement BuildTree(IList<string> parts, HtmlHelper helper)
    {
        var root = new HtmlElement { Name = "root" };
        var current = root;

        foreach (var part in parts)
        {
            if (part.StartsWith("<"))
            {
                var m = TagMainRe.Match(part);
                if (!m.Success) continue;

                bool isClosing = m.Groups[1].Success;
                var tagName = m.Groups[2].Value.ToLowerInvariant();
                var attrSrc = m.Groups[3].Value;
                bool selfClosing = part.EndsWith("/>") || helper.IsVoidTag(tagName);

                if (isClosing)
                {
                    current = current.Parent ?? root; // עלייה רמה
                    continue;
                }

                if (!helper.IsTag(tagName)) continue;

                var node = new HtmlElement { Name = tagName, Parent = current };

                foreach (Match am in AttrRe.Matches(attrSrc))
                {
                    var name = am.Groups[1].Value;
                    var val = am.Groups[2].Success ? am.Groups[2].Value
                          : am.Groups[3].Success ? am.Groups[3].Value
                          : am.Groups[4].Success ? am.Groups[4].Value
                          : "";

                    node.Attributes.Add((name, val));

                    if (name.Equals("id", StringComparison.OrdinalIgnoreCase)) node.Id = val;

                    if (name.Equals("class", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(val))
                    {
                        node.Classes.AddRange(
                            Regex.Split(val.Trim(), @"\s+").Where(s => !string.IsNullOrWhiteSpace(s)));
                    }
                }

                current.Children.Add(node);

                if (!selfClosing) current = node; // יורדים רמה אם לא self-closing
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    if (current.InnerHtml.Length > 0) current.InnerHtml += " ";
                    current.InnerHtml += part;
                }
            }
        }

        return root;
    }
}