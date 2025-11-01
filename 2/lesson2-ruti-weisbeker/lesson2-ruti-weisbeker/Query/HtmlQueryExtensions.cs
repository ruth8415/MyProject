using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lesson2_ruti_weisbeker.Model;

namespace lesson2_ruti_weisbeker.Query;

public static class HtmlQueryExtensions
{
    public static IEnumerable<HtmlElement> FindBySelector(this HtmlElement root, Selector selector)
    {
        var result = new HashSet<HtmlElement>();
        Recurse(root, selector, result);
        return result;

        static void Recurse(HtmlElement scope, Selector sel, HashSet<HtmlElement> acc)
        {
            var filtered = scope.Descendants().Where(e => Matches(e, sel)).ToList();

            if (sel.Child == null)
            {
                foreach (var el in filtered) acc.Add(el);
            }
            else
            {
                foreach (var el in filtered) Recurse(el, sel.Child, acc);
            }
        }

        static bool Matches(HtmlElement el, Selector s)
        {
            if (s.TagName != null && !el.Name.Equals(s.TagName, StringComparison.OrdinalIgnoreCase)) return false;
            if (s.Id != null && !string.Equals(el.Id, s.Id, StringComparison.Ordinal)) return false;
            if (s.Classes.Count > 0 && !s.Classes.All(cls => el.Classes.Contains(cls))) return false;
            return true;
        }
    }
}
