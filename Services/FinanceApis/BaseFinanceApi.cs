using System;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace news_apis
{
    public abstract class BaseFinanceApi
    {
        protected abstract string GetTickerUrl(string supplier);
        protected abstract RestRequest? GetTickerRequest(string supplier);
        protected abstract Company? ParseTickerResponse(string response);

        protected abstract string GetStockUrl(Company supplier);
        protected abstract RestRequest? GetStockRequest(Company supplier);
        protected abstract Stock? ParseStockResponse(Company Supplier, string response);

        protected abstract Task<Financials?> GetFinanceResponse(Company supplier);

        public abstract Task<decimal> GetForexRate(string oldCurrency, string newCurrency);


        public async Task<FinanceInfo?> CallApi(string supplier)
        {
            Company? company = ParseTickerResponse(await CallExternalApi(GetTickerUrl(supplier), GetTickerRequest(supplier)));
            if (company == null || company.Ticker == null) return null;
            Stock? stock = ParseStockResponse(company, await CallExternalApi(GetStockUrl(company), GetStockRequest(company)));
            Financials? financials = await GetFinanceResponse(company);

            return new FinanceInfo
            {
                Company = company,
                Financials = financials,
                Stock = stock
            };
        }

        protected async Task<string> CallExternalApi(string url, RestRequest? request)
        {
            if (request == null) return "";
            using RestClient client = new(url);
            client.AddDefaultHeader("Cache-Control", "no-cache"); // prevent RestSharp dependencies caching API results

            RestResponse response = await client.ExecuteAsync(request);
            string content = response.Content!;
            Console.WriteLine(content);

            if (response.Content!.Contains("\"Error Message\""))
            {
                var errorResponse = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
                if (errorResponse!.TryGetValue("Error Message", out var errorMessage))
                {
                    throw new Exception("Unexpected error with financials: " + errorMessage);
                }
            }

            int titleStartIndex = content.IndexOf("<title>");
            if (titleStartIndex != -1)
            {
                int titleEndIndex = content.IndexOf("</title>", titleStartIndex);
                string title = content.Substring(titleStartIndex + "<title>".Length, titleEndIndex - titleStartIndex - "<title>".Length);
                throw new Exception("Unexpected error with financials: " + title);
            }

            return content.Replace("null", "0");
        }
    }

}