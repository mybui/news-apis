using System.Text.Json.Serialization;
using System.Text.Json;
using RestSharp;

namespace news_apis
{
    public class DevelopmentApi : BaseNewsApi
    {

        private record EchoResponse
        {
            [JsonPropertyName("query")]
            public EchoQuery? Query { get; init; }
        }

        private record EchoQuery
        {
            [JsonPropertyName("supplier")]
            public string? Supplier { get; init; }

            [JsonPropertyName("page")]
            public string? Page { get; init; }
        }

        override protected string GetApiUrl()
        {
            return "https://echo.zuplo.io"; // an API that echoes back everything sent to it
        }

        override protected RestRequest GetApiRequest(string supplier, string? country, string? language, int page)
        {
            RestRequest request = new();
            request.AddQueryParameter("supplier", supplier);
            request.AddQueryParameter("page", page);
            return request;
        }

        override protected List<NewsItem> ParseApiResponse(string response)
        {
            EchoResponse echoResponse = JsonSerializer.Deserialize<EchoResponse>(response)!;
            if (echoResponse.Query == null || echoResponse.Query.Supplier == null || echoResponse.Query.Page == null) throw new Exception("Supplier not found");
            string supplier = echoResponse.Query.Supplier;
            int page = int.Parse(echoResponse.Query.Page);
            return GenerateNewsItems(supplier, page);
        }

        private List<NewsItem> GenerateNewsItems(string supplier, int page)
        {
            List<NewsItem> newsItems = new List<NewsItem> // a bunch of example news articles 🤩
            {
                new NewsItem
                {
                    Title = "New Partnership Announcement with " + supplier,
                    Snippet = "Exciting news! We are thrilled to announce our new partnership with " + supplier + ". Stay tuned for more updates.",
                },
                new NewsItem
                {
                    Title = "Exclusive Interview: " + supplier + " CEO Shares Insights",
                    Snippet = "Get insider insights! Read our exclusive interview with " + supplier + " CEO as they discuss the latest industry trends and innovations.",
                },
                new NewsItem
                {
                    Title = "Breaking: " + supplier + " Launches Revolutionary Product",
                    Snippet = "Big news alert! Learn more about the revolutionary product launched by " + supplier + " that's set to redefine the market.",
                },
                new NewsItem
                {
                    Title = supplier + " Receives Prestigious Industry Award",
                    Snippet = "Congratulations! " + supplier + " has been honored with a prestigious industry award for their outstanding contributions and achievements.",
                },
                new NewsItem
                {
                    Title = "Behind the Scenes: " + supplier + " Factory Tour",
                    Snippet = "Explore behind the scenes! Join us for an exclusive tour of " + supplier + "'s state-of-the-art factory and witness innovation in action.",
                },
                new NewsItem
                {
                    Title = "Expert Panel: Future Trends in " + supplier + " Industry",
                    Snippet = "Don't miss out! Join our expert panel discussion on the future trends shaping the " + supplier + " industry landscape.",
                },
                new NewsItem
                {
                    Title = "Innovative Solutions: " + supplier + " Leads the Way",
                    Snippet = "Discover innovation! Find out how " + supplier + " is leading the way with their cutting-edge solutions and technologies.",
                },
                new NewsItem
                {
                    Title = "Industry Insights: " + supplier + " Market Analysis",
                    Snippet = "Stay informed! Dive into our comprehensive market analysis report featuring insights on " + supplier + " and industry trends.",
                },
                new NewsItem
                {
                    Title = "Spotlight on Sustainability: " + supplier + "'s Green Initiatives",
                    Snippet = "Go green! Learn about " + supplier + "'s commitment to sustainability and their initiatives to create a greener future.",
                },
                new NewsItem
                {
                    Title = "Exclusive Deal: Special Offer from " + supplier,
                    Snippet = "Limited time offer! Take advantage of our exclusive deal with " + supplier + " and enjoy special discounts on select products.",
                },
                new NewsItem
                {
                    Title = "In-Depth Review: " + supplier + "'s Latest Release",
                    Snippet = "Get the scoop! Read our in-depth review of " + supplier + "'s latest release and discover its features and benefits.",
                },
                new NewsItem
                {
                    Title = "Breaking News: " + supplier + " Expands Global Reach",
                    Snippet = "Breaking news! Find out how " + supplier + " is expanding its global reach and making waves in new markets.",
                },
                new NewsItem
                {
                    Title = "Industry Leaders Gather at " + supplier + " Summit",
                    Snippet = "Save the date! Join industry leaders at the upcoming " + supplier + " Summit for networking, insights, and collaboration.",
                },
                new NewsItem
                {
                    Title = "Customer Spotlight: Success Stories with " + supplier,
                    Snippet = "Discover success! Hear from satisfied customers as they share their experiences and success stories with " + supplier + ".",
                },
                new NewsItem
                {
                    Title = "Tech Talk: " + supplier + " Unveils Next-Gen Innovations",
                    Snippet = "Stay ahead of the curve! Tune in as " + supplier + " unveils its latest next-generation innovations in our exclusive tech talk.",
                },
                new NewsItem
                {
                    Title = "Investor Update: " + supplier + "'s Financial Performance",
                    Snippet = "Get the latest! Read our investor update for insights into " + supplier + "'s financial performance and growth trajectory.",
                },
                new NewsItem
                {
                    Title = "Inside Look: " + supplier + " Corporate Culture",
                    Snippet = "Explore culture! Take an inside look at " + supplier + "'s corporate culture and values that drive their success.",
                },
                new NewsItem
                {
                    Title = "Industry Disruption: " + supplier + " Leads the Charge",
                    Snippet = "Disrupting the norm! Learn how " + supplier + " is leading the charge in industry disruption with groundbreaking innovations.",
                },
                new NewsItem
                {
                    Title = "Exclusive Event: " + supplier + " Product Launch Party",
                    Snippet = "Save the date! Join us for an exclusive product launch party hosted by " + supplier + " featuring live demos and surprises.",
                },
                new NewsItem
                {
                    Title = "Market Analysis: Trends Impacting " + supplier + " Industry",
                    Snippet = "Stay informed! Gain insights into the latest trends impacting the " + supplier + " industry with our comprehensive market analysis.",
                },
            };

            Random rnd = new Random();
            List<NewsItem> chosenItems = newsItems.OrderBy(x => rnd.Next()).Take(3).Select((item, index) => new NewsItem
            {
                Title = item.Title,
                Snippet = item.Snippet,
                PublishedAt = DateTime.Now.AddHours(-1 * ((page - 1) * 5 + index)),
                Language = "en",
                Url = "https://example.org",
                Thumbnail = new Thumbnail { Url = "https://picsum.photos/400/300", Width = 400, Height = 300 }
            }).ToList();

            return chosenItems;
        }

    }

}