using System;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace news_apis
{
	public class CountryService
    {
        private readonly IEnumerable<Country> _records;

        public CountryService()
        {
            using (var reader = new StreamReader("Data/CountryCodes.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
            {

                _records = csv.GetRecords<Country>().ToList();
            }
        }

        public IEnumerable<Country> GetCountries()
        {
            return _records;
        }

        public bool CheckCountry(string alpha2)
        {
            var exists = _records.Any(country => country.Alpha2!.ToLower() == alpha2.ToLower());
            return exists;
        }
    }
}