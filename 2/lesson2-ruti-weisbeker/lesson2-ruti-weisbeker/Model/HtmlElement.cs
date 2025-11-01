using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lesson2_ruti_weisbeker.Model;

public class HtmlElement
{
    public string? Id { get; set; }
    public string Name { get; set; } = "";
    public List<(string Name, string Value)> Attributes { get; } = new();
    public List<string> Classes { get; } = new();
    public string InnerHtml { get; set; } = "";

    public HtmlElement? Parent { get; set; }
    public List<HtmlElement> Children { get; } = new();

    // כל הצאצאים (כולל this) – עם Queue (ללא רקורסיה)
    public IEnumerable<HtmlElement> Descendants()
    {
        var q = new Queue<HtmlElement>();
        q.Enqueue(this);
        while (q.Count > 0)
        {
            var cur = q.Dequeue();
            yield return cur;
            foreach (var ch in cur.Children) q.Enqueue(ch);
        }
    }

    // כל האבות עד השורש
    public IEnumerable<HtmlElement> Ancestors()
    {
        var cur = Parent;
        while (cur != null) { yield return cur; cur = cur.Parent; }
    }
}