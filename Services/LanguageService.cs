using System;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace news_apis
{
    public class LanguageService
    {
        private readonly IEnumerable<Language> _records;

        public LanguageService()
        {
            using (var reader = new StreamReader("Data/LanguageList.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
            {

                _records = csv.GetRecords<Language>().ToList();
            }
        }

        public IEnumerable<Language> GetLanguages()
        {
            return _records;
        }

        public bool CheckLanguage(string LanguageCode)
        {
            var exists = _records.Any(Language => Language.Code!.ToLower() == LanguageCode.ToLower());
            return exists;
        }

    }
}