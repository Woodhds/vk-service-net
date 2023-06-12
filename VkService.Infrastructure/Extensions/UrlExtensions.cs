using System.Collections.Specialized;
using Microsoft.AspNetCore.WebUtilities;

namespace VkService.Infrastructure.Extensions;

public static class UrlExtensions
{
    public static string BuildUrl(this NameValueCollection? @params, string url)
    {
        @params ??= new NameValueCollection();

        if (string.IsNullOrEmpty(url))
            throw new ArgumentNullException(nameof(url));

        foreach (var c in Enumerable.Range(0, @params.Count))
        {
            url = QueryHelpers.AddQueryString(url, @params.GetKey(c), @params.Get(c));
        }

        return url;
    }
}
