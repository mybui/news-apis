using System;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace news_apis
{
    public abstract class BaseNewsApi
    {
        protected abstract string GetApiUrl();
        protected abstract RestRequest GetApiRequest(string supplier, string? country, string? language, int page);
        protected abstract List<NewsItem> ParseApiResponse(string response);


        public virtual async Task<List<NewsItem>> CallApi(string supplier, string? country, string? language, int page)
        {
            string? apiResponse = CallExternalApi(supplier, country, language, page)!.Item1;
            Console.WriteLine(apiResponse);
            if (apiResponse == null || !ContainsArticles(apiResponse))
            {
                if (language != null)
                {
                    string? apiResponse2 = CallExternalApi(supplier, country, null, page)!.Item1;
                    if (apiResponse2 == null || !ContainsArticles(apiResponse2)) return new List<NewsItem>();
                    var parsedResponse = ParseApiResponse(apiResponse2);
                    foreach (var article in parsedResponse)
                    {
                        var translator = new TranslationApi();
                        if (article.Title != null) article.Title = await translator.Translate(article.Title, language);
                    }
                    return parsedResponse;
                }
                else
                    return new List<NewsItem>();
            }
            return ParseApiResponse(apiResponse);
        }

        protected (string?, IReadOnlyCollection<HeaderParameter>) CallExternalApi(string supplier, string? country, string? language, int page)
        {
            using RestClient client = new(GetApiUrl());
            client.AddDefaultHeader("Cache-Control", "no-cache"); // prevent RestSharp dependencies caching API results
            RestResponse response = client.Execute(GetApiRequest(supplier, country, language, page));
            return (response.Content, response.Headers)!;
        }

        static protected bool ContainsArticles(string apiResponse)
        {
            return !(apiResponse.Contains("\"available\":0") || apiResponse.Contains("\"totalResults\":0") || apiResponse.Contains("\"found\":0") || apiResponse.Contains("\"value\": []"));
        }
    }

}