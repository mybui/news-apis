using System;
using RestSharp;

namespace news_apis
{
    public abstract class BaseNewsApi
    {
        protected abstract string GetApiUrl();
        protected abstract RestRequest GetApiRequest(string supplier, string? country, string? language, int page);
        protected abstract List<NewsItem> ParseApiResponse(string response);

        public List<NewsItem> CallApi(string supplier, string? country, string? language, int page)
        {
            string? apiResponse = CallExternalApi(supplier, country, language, page);
            if (apiResponse == null) return new List<NewsItem>();
            return ParseApiResponse(apiResponse);
        }

        private string? CallExternalApi(string supplier, string? country, string? language, int page)
        {
            using RestClient client = new(GetApiUrl());
            client.AddDefaultHeader("Cache-Control", "no-cache"); // prevent RestSharp dependencies caching API results
            RestResponse response = client.Execute(GetApiRequest(supplier, country, language, page));
            return response.Content;
        }
    }

}