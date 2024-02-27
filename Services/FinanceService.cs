using System;
namespace news_apis
{
    public class FinanceService
    {
        private readonly IConfiguration Configuration;
        private readonly SupplierService _supplierService;

        public FinanceService(IConfiguration configuration)
        {

            Configuration = configuration;
            _supplierService = new SupplierService();
        }

        public async Task<FinanceInfo> CallAllApis(string supplier)
        {
            if (!_supplierService.CheckSupplier(supplier))
                throw new ArgumentException("Invalid supplier");

            BaseFinanceApi[] apis = { new FinancialDevelopmentAPI() };

            foreach (var api in apis)
            {
                FinanceInfo? result = await api.CallApi(supplier);
                if (result != null)
                    return result;
            }

            throw new Exception("No financial data found.");
        }
    }
}
