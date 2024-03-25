using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Globalization;
using System.Reflection;

namespace news_apis
{
    public class CurrencyService
    {
        private readonly IConfiguration Configuration;
        private readonly IEnumerable<Currency> _records;
        // private readonly SupplierService _supplierService;
        private readonly ApiSelectionService _apiSelectionService;

        public CurrencyService(IConfiguration configuration)
        {

            Configuration = configuration;
            // _supplierService = new SupplierService();
            _apiSelectionService = new ApiSelectionService(Configuration);
            using (var reader = new StreamReader("Data/Currency.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
            {

                _records = csv.GetRecords<Currency>().ToList();
            }
        }

        public IEnumerable<Currency> GetCurrencies()
        {
            return _records;
        }

        private async Task<decimal> GetExchangeRate(string oldCurrency, string newCurrency)
        {
            BaseFinanceApi[] apis = _apiSelectionService.GetFinanceApiList();
            foreach (var api in apis)
            {
                decimal result = await api.GetForexRate(oldCurrency, newCurrency);
                return result;
            }
            throw new Exception("No financial data found.");
        }

        private static bool IsNumeric(Type type)
        {
            return type == typeof(decimal) || type == typeof(int) || type == typeof(double)
                || type == typeof(float) || type == typeof(long) || type == typeof(short);
        }

        public async Task ConvertCurrency(object? obj, string targetCurrency)
        {
            if (obj == null) { return; }
            PropertyInfo? currencyProperty = obj.GetType().GetProperty("Currency");
            if (currencyProperty == null)
            {
                throw new Exception("Currency property not found in the object.");
            }

            string? sourceCurrency = currencyProperty.GetValue(obj) as string;
            if (string.IsNullOrEmpty(sourceCurrency)) { return; }

            decimal exchangeRate = await GetExchangeRate(sourceCurrency, targetCurrency);

            currencyProperty.SetValue(obj, targetCurrency);

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if (IsNumeric(property.PropertyType))
                {
                    object? value = property.GetValue(obj);
                    if (value == null) { continue; }
                    decimal numericValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
                    decimal convertedValue = numericValue * exchangeRate;
                    property.SetValue(obj, Convert.ChangeType(convertedValue, property.PropertyType));
                }
            }
        }
    }
}