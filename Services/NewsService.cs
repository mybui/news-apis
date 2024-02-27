using System;
namespace news_apis
{
    public class NewsService
    {
        private readonly IConfiguration Configuration;
        private readonly SupplierService _supplierService;
        private readonly CountryService _countryService;

        public NewsService(IConfiguration configuration)
        {
            Configuration = configuration;
            _supplierService = new SupplierService();
            _countryService = new CountryService();
        }

        public List<NewsItem> CallAllApis(string supplier, string? country, string? language, int page)
        {
            Console.WriteLine(supplier + page + country);
            if (!_supplierService.CheckSupplier(supplier))
                throw new ArgumentException("Invalid supplier");
            if (!string.IsNullOrEmpty(country) && !_countryService.CheckCountry(country))
                throw new ArgumentException("Invalid country");

            BaseNewsApi[] apis = { new DevelopmentApi() };

            foreach (var api in apis)
            {
                List<NewsItem> result = api.CallApi(supplier, country, language, page);
                foreach (var article in result)
                {
                    int length = 200;
                    article.Snippet = article.Snippet!.Replace('"', '\"');
                    article.Snippet = article.Snippet!.Length > length ? string.Concat(article.Snippet.AsSpan(0, length - 3), "...") : article.Snippet;
                }

                if (result.Count > 0)
                    return result;
            }

            return new List<NewsItem>();
        }
    }
}