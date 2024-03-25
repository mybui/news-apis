namespace news_apis
{

    public class NewsApiSelection
    {
        public bool BingNewsApi { get; set; }
        public bool DevelopmentApi { get; set; }
        public bool GoogleNewsApi { get; set; }
        public bool TheNewsApi { get; set; }
        public bool WorldNewsApi { get; set; }
        public bool SecApi { get; set; }
    }

    public class FinanceApiSelection
    {
        public bool FinancialModelingPrep { get; set; }
        public bool FinancialDevelopmentApi { get; set; }
    }

    public class ApiSelectionService
    {
        private readonly IConfiguration Configuration;
        public ApiSelectionService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public BaseNewsApi[] GetNewsApiList()
        {

            var apiSelection = new NewsApiSelection();
            Configuration.GetSection("NewsApiSelection").Bind(apiSelection);
            BaseNewsApi[] apis = new BaseNewsApi[]
            {
                apiSelection.DevelopmentApi ? new DevelopmentApi() : null
            };
            // Filter out null values (APIs that were not selected)
            apis = apis.Where(api => api != null).ToArray();

            return apis;
        }

        public BaseFinanceApi[] GetFinanceApiList()
        {

            var apiSelection = new FinanceApiSelection();
            Configuration.GetSection("FinanceApiSelection").Bind(apiSelection);
            BaseFinanceApi[] apis = new BaseFinanceApi[]
            {
                apiSelection.FinancialDevelopmentApi ? new FinancialDevelopmentAPI() : null
            };
            // Filter out null values (APIs that were not selected)
            apis = apis.Where(api => api != null).ToArray();

            return apis;
        }
    }
}