using System;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace news_apis
{
	public class SupplierService
	{
        private readonly IEnumerable<Supplier> _records;

        public SupplierService()
        {
            using (var reader = new StreamReader("Data/SupplierGroup.csv"))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ";" }))
            {

                _records = csv.GetRecords<Supplier>().OrderBy(s=>s.Name).ToList();
            }
        }

        public IEnumerable<Supplier> GetSuppliers()
        {
            return _records;
        }

        public bool CheckSupplier(string supplierName)
        {
            var exists = _records.Any(supplier => supplier.Name!.ToLower() == supplierName.ToLower());
            return exists;
        }
    }
}