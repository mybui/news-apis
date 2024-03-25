using System;
namespace news_apis
{
    public class NewsService
    {
        private readonly IConfiguration Configuration;
        private readonly SupplierService _supplierService;
        private readonly CountryService _countryService;
        private readonly LanguageService _languageService;
        private readonly ApiSelectionService _apiSelectionService;

        public NewsService(IConfiguration configuration)
        {

            Configuration = configuration;
            _supplierService = new SupplierService();
            _countryService = new CountryService();
            _languageService = new LanguageService();
            _apiSelectionService = new ApiSelectionService(Configuration);
        }

        public async Task<List<NewsItem>> CallAllApis(string supplier, string? country, string? language, int page)
        {
            Console.WriteLine(supplier + page + country + language);
            if (!_supplierService.CheckSupplier(supplier))
                throw new ArgumentException("Invalid supplier");
            if (!string.IsNullOrEmpty(country) && !_countryService.CheckCountry(country))
                throw new ArgumentException("Invalid country");
            if (!string.IsNullOrEmpty(language) && !_languageService.CheckLanguage(language))
                throw new ArgumentException("Invalid language");

            BaseNewsApi[] apis = _apiSelectionService.GetNewsApiList();

            foreach (var api in apis)
            {
                List<NewsItem> result = await api.CallApi(supplier, country, language, page);
                foreach (var article in result)
                {
                    int snippetLength = 200;
                    int titleLength = 100;

                    // Deduplicate with titles
                    result = result.GroupBy(article => article.Title).Select(group => group.First()).ToList();

                    // Trim snippets and titles
                    result.ForEach(article =>
                    {
                        article.Snippet = article.Snippet?.Replace('"', '\"');
                        article.Snippet = article.Snippet?.Length > snippetLength ? string.Concat(article.Snippet.AsSpan(0, snippetLength - 3), "...") : article.Snippet;
                        article.Title = article.Title?.Length > titleLength ? string.Concat(article.Title.AsSpan(0, titleLength - 3), "...") : article.Title;
                    });
                }

                if (result.Count > 0)
                    return result;
            }
            return new List<NewsItem>();
        }
    }
}