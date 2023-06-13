using System.Runtime.CompilerServices;
using HtmlAgilityPack;
using VkService.Models;
using VkService.Parsers.Abstractions;

namespace VkService.Parsers;

public class SiteParser : IMessageParser
{
    private readonly IHttpClientFactory _factory;

    public SiteParser(IHttpClientFactory factory)
    {
        _factory = factory;
    }

    public async IAsyncEnumerable<IEnumerable<RepostMessage>> Parse([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var client = _factory.CreateClient();

        for (var i = 0; i <= 4; i++)
        {
            var formContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new("page_num", i.ToString()),
                new("our", string.Empty),
                new("city_id", "5")
            });

            var result = await client.PostAsync("https://wingri.ru/main/getPosts", formContent, cancellationToken);

            var content = await result.Content.ReadAsStringAsync(cancellationToken);

            var doc = new HtmlDocument();
            doc.LoadHtml(content);
            if (doc?.DocumentNode == null) continue;

            var models = doc.DocumentNode?
                .SelectNodes(
                    "//div[@class='grid-item']/div[@class='post_container']/div[@class='post_footer']/a/@href")
                .Where(d => d.Attributes != null &&
                            d.Attributes.Any(h => h.Name == "href" && !string.IsNullOrEmpty(h.Value)))
                .Select(d => d.GetAttributeValue("href", "")?.Replace("https://vk.com/wall", "").Split('_'))
                .Where(h => h is { Length: > 1 })
                .Select(d => new RepostMessage(int.Parse(d![0]), int.Parse(d[1])))
                .ToArray();

            if (models == null || models.Length == 0)
                continue;

            yield return models;
        }
    }
}
