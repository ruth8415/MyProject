using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lesson2_ruti_weisbeker.Helpers;

namespace lesson2_ruti_weisbeker.Query;

public class Selector
{
    public string? TagName { get; set; }
    public string? Id { get; set; }
    public List<string> Classes { get; } = new();

    public Selector? Parent { get; set; }
    public Selector? Child { get; set; }

    public static Selector Parse(string query, HtmlHelper helper)
    {
        var levels = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Selector? root = null;
        Selector? current = null;

        foreach (var level in levels)
        {
            var sel = new Selector();
            var buf = "";

            for (int i = 0; i < level.Length; i++)
            {
                char c = level[i];
                if (c == '#' || c == '.')
                {
                    if (!string.IsNullOrEmpty(buf) && helper.IsTag(buf))
                        sel.TagName = buf.ToLowerInvariant();
                    buf = "";

                    int j = i + 1;
                    while (j < level.Length && level[j] != '#' && level[j] != '.') j++;
                    var token = level[(i + 1)..j];

                    if (c == '#') sel.Id = token;
                    else sel.Classes.Add(token);

                    i = j - 1;
                }
                else buf += c;
            }

            if (!string.IsNullOrEmpty(buf) && helper.IsTag(buf))
                sel.TagName = buf.ToLowerInvariant();

            if (root == null) { root = sel; current = sel; }
            else
            {
                current!.Child = sel; sel.Parent = current; current = sel;
            }
        }
        return root ?? new Selector();
    }
}