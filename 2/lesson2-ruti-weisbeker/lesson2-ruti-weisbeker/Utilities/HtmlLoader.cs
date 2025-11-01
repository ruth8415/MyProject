using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace lesson2_ruti_weisbeker.Utilities;

public static class HtmlLoader
{
    private static readonly HttpClient Client = new();

    public static async Task<string> LoadAsync(string url)
    {
        var res = await Client.GetAsync(url);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadAsStringAsync();
    }
}
