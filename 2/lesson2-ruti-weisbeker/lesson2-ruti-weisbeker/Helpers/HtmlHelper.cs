using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace lesson2_ruti_weisbeker.Helpers;

public sealed class HtmlHelper
{
    public string[] HtmlTags { get; }
    public string[] HtmlVoidTags { get; }

    private static readonly Lazy<HtmlHelper> _instance = new(() => new HtmlHelper());
    public static HtmlHelper Instance => _instance.Value;

    private HtmlHelper()
    {
        var tagsJson = File.ReadAllText(Path.Combine("Data", "HtmlTags.json"));
        var voidJson = File.ReadAllText(Path.Combine("Data", "HtmlVoidTags.json"));

        HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson) ?? Array.Empty<string>();
        HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidJson) ?? Array.Empty<string>();
    }

    public bool IsTag(string word) =>
        HtmlTags.Contains(word, StringComparer.OrdinalIgnoreCase);

    public bool IsVoidTag(string word) =>
        HtmlVoidTags.Contains(word, StringComparer.OrdinalIgnoreCase);
}