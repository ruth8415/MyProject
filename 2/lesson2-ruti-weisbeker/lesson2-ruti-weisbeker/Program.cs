using lesson2_ruti_weisbeker.Model;
using lesson2_ruti_weisbeker.Utilities;
using lesson2_ruti_weisbeker.Helpers;
using lesson2_ruti_weisbeker.Parsing;
using lesson2_ruti_weisbeker.Query;

class Program
{
    static async Task Main(string[] args)
    {
        // 1) טעינת HTML (אפשר גם ממחרוזת ידנית בשביל ניסוי מהיר)
        //string html = await HtmlLoader.LoadAsync("https://example.com");
        string html = @"<div id=""app""><h1 class=""title main"">Hello</h1><p>World</p><img src=""x.jpg""/></div>";

        // 2) פירוק לחלקים (תגיות/טקסטים)
        var parts = HtmlSplitter.SplitToParts(html);

        // 3) בניית עץ האובייקטים
        var helper = HtmlHelper.Instance;
        HtmlElement root = HtmlTreeBuilder.BuildTree(parts, helper);

        // 4) שאילתות (CSS Selector בסיסי)
        var sel1 = Selector.Parse("div", helper);
        var sel2 = Selector.Parse("#app .title", helper);
        var sel3 = Selector.Parse("div #app .title", helper);

        var r1 = root.FindBySelector(sel1).ToList();
        var r2 = root.FindBySelector(sel2).ToList();
        var r3 = root.FindBySelector(sel3).ToList();

        Console.WriteLine($"div count: {r1.Count}");
        Console.WriteLine($"#app .title count: {r2.Count}");
        Console.WriteLine($"div #app .title count: {r3.Count}");

        foreach (var el in r2)
            Console.WriteLine($"<{el.Name} id='{el.Id}' class='{string.Join(" ", el.Classes)}'> inner='{el.InnerHtml}'");
    }
}
