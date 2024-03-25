using RestSharp;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System;


namespace news_apis
{
    public class FinancialDevelopmentAPI : BaseFinanceApi
    {
        protected override string GetTickerUrl(string supplier) { return "abc"; }
        protected override RestRequest? GetTickerRequest(string supplier) { return null; }
        protected override Company? ParseTickerResponse(string response) { return new Company { Ticker = "ABC", Name = "ABC Oy.", Currency = "USD", Exchange = "New York" }; }

        protected override string GetStockUrl(Company supplier) { return ""; }
        protected override RestRequest? GetStockRequest(Company supplier) { return null; }
        protected override Stock? ParseStockResponse(Company supplier, string response) { return new Stock { Open = 1234, High = 1324, Low = 1234, Close = 1234, Currency = "USD" }; }

        protected override Task<Financials?> GetFinanceResponse(Company supplier)
        {
            return Task.FromResult<Financials?>(new Financials
            {
                Date = DateTime.Today,
                Cik = "CBA",
                Revenue = 1324,
                TotalAssets = 1234,
                Currency = "USD",
                Cash = 1234,
                CashAndEquivalent = 1234,
                Inventory = 1234,
                AccountsPayable = 1324
            });
        }

        public override Task<decimal> GetForexRate(string oldCurrency, string newCurrency) { return Task.FromResult(1.1M); }

    }
}