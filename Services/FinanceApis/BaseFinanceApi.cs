using System;
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
        protected abstract Stock? ParseStockResponse(string response);

        protected abstract Task<Financials> GetFinanceResponse(Company supplier);


        public async Task<FinanceInfo?> CallApi(string supplier)
        {
            Company? company = ParseTickerResponse(await CallExternalApi(GetTickerUrl(supplier), GetTickerRequest(supplier)));
            if (company == null || company.Ticker == null) return null;
            Stock? stock = ParseStockResponse(await CallExternalApi(GetStockUrl(company), GetStockRequest(company)));
            Financials financials = await GetFinanceResponse(company);
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
            return response.Content!;
        }
    }

}

