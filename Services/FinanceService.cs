using System;
namespace news_apis
{
    public class FinanceService
    {
        private readonly IConfiguration Configuration;
        private readonly SupplierService _supplierService;
        private readonly CurrencyService _currencyService;
        private readonly ApiSelectionService _apiSelectionService;


        public FinanceService(IConfiguration configuration)
        {

            Configuration = configuration;
            _supplierService = new SupplierService();
            _currencyService = new CurrencyService(configuration);
            _apiSelectionService = new ApiSelectionService(Configuration);
        }

        public async Task<FinanceInfo> CallAllApis(string supplier, string currency)
        {
            if (!_supplierService.CheckSupplier(supplier))
                throw new ArgumentException("Invalid supplier");

            BaseFinanceApi[] apis = _apiSelectionService.GetFinanceApiList();

            foreach (var api in apis)
            {
                FinanceInfo? result = await api.CallApi(supplier);
                if (result != null)
                {
                    await _currencyService.ConvertCurrency(result.Financials, currency);
                    await _currencyService.ConvertCurrency(result.Stock, currency);
                    return result;
                }
            }

            throw new Exception("No financial data found.");
        }
    }
}