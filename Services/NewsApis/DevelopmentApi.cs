using System.Text.Json.Serialization;
using System.Text.Json;
using RestSharp;

namespace news_apis
{
    public class DevelopmentApi : BaseNewsApi
    {

        private record ApplianceItem
        {
            [JsonPropertyName("id")]
            public int Id { get; init; }

            [JsonPropertyName("uid")]
            public string? Uid { get; init; }

            [JsonPropertyName("brand")]
            public string? Brand { get; init; }

            [JsonPropertyName("equipment")]
            public string? Equipment { get; init; }
        }

        override protected string GetApiUrl()
        {
            return "https://random-data-api.com/api/v2/appliances";
        }

        override protected RestRequest GetApiRequest(string supplier, string? country, string? language, int page)
        {
            RestRequest request = new();
            request.AddQueryParameter("size", "5");
            return request;
        }

        override protected List<NewsItem> ParseApiResponse(string response)
        {
            List<ApplianceItem> appliances = JsonSerializer.Deserialize<List<ApplianceItem>>(response)!;
            List<NewsItem> newsItems = appliances.Select(a => new NewsItem
            {
                Title = a.Equipment,
                Snippet = a.Brand,
                Url = a.Uid,

            }).ToList<NewsItem>();
            return newsItems;
        }
    }

}
